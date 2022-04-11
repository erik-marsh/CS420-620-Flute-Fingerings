using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public class Hi5_Hand_CollisionData
    {
        public Hi5_Hand_Collider_Visible_Finger collider_finger;
        public ContactPoint[] constactPoint;
    }
    public class Hi5_Hand_Collider_Visible_Finger : Hi5_Glove_Interaction_Collider//Hi5_Glov_Interavtion_Collider
    {
        public Hi5_Hand_Visible_Finger mFinger = null;
        internal bool isDrawLine = false;
        internal Hi5_Glove_Finger_bone_Type mFingerBoneType;
        internal float mZMinAngel = 0.0f;
        internal float mZMaxAngel = 0.0f;
        internal protected Hi5_Hand_Visible_Hand mHand;
        internal Transform brother = null;
        internal GameObject follow = null;
        internal Transform pointSphere = null;
        internal Transform[] childs = null;

        //pre
        //controller zRotion frequency
        private bool mIsCollider = false;
        //internal float saveTime = 0.0f;
        ContactPoint[] contactPoints = null;
       internal  Queue<Hi5_Position_Record> mQueuePositionRecord = new Queue<Hi5_Position_Record>();
        Vector3 prePositionRecord;
        public bool IsCollider
        {
            get { return mIsCollider; }
        }
        //pre

        private void Awake()
        {
            base.Awake();
            if (this.GetComponent<Rigidbody>() != null)
            {
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }



            //pre
            mIsCollider = false;
            prePositionRecord = transform.position;

            follow = new GameObject("Follow");
            follow.transform.parent = transform.parent;
            if (pointSphere == null)
            {
                //Object temp = Resources.Load("Hi5TestPoin", typeof(GameObject));

                //GameObject cube = Instantiate(temp, Vector3.zero, Quaternion.identity, transform.parent) as GameObject;
                //pointSphere = cube.transform;
            }
          ;
        }

       
		protected bool isAttributeSet = false;
        public virtual void setFinger(Hi5_Hand_Visible_Hand hand,
                                Hi5_Hand_Visible_Finger finger,
                                Hi5_Glove_Finger_bone_Type finger_bone_Type,
                                float zMaxAngel,
                                float zMinAngel)
        {
            mHand = hand;
            //if(finger == null)
            mFinger = finger;
            mFingerBoneType = finger_bone_Type;
            mZMinAngel = zMinAngel;
            mZMaxAngel = zMaxAngel;
			isAttributeSet = true;
        }

        private void FixedUpdate()
        {
            RecordPosition(Time.deltaTime);
        }

		bool isSetLayer = false;


        override protected void OnCollisionStay(Collision collision)
        {
			
            mIsCollider = true;
            //if (mFinger == null)
            //    Debug.Log("mFinger" + mFingerBoneType);
            childs = mFinger.GetChildNodes(mFingerBoneType).ToArray() ;
            ContactPoint[] contactPoints = collision.contacts;
            Hi5_Hand_CollisionData data = new Hi5_Hand_CollisionData();
            data.collider_finger = this;
            data.constactPoint = contactPoints;
            //caculateRotation(contactPoints);
            if (Hi5_Interaction_Const.IsUseVisibleHand)
                mFinger.AddDicCollision(mFingerBoneType, data);
        }

        override protected void OnCollisionExit(Collision collision)
        {
            //mIsCollider = false;
            ////mFixUpdateCount = 0;
            //mFinger.SetChildBoneLock(mIsCollider, mFingerBoneType);

        }

        int countTwo = 0;
		private void CalculateTwoRotation(ContactPoint[] contacts)
        {
            if (countTwo != 0)
                return;
            countTwo++;

            Transform parent = transform.parent.transform;
            Transform parentparent = transform.parent.parent.transform;
            Transform parentparentparent = transform.parent.parent.parent.transform;
            ContactPoint[] contactPoints = contacts;
            if (contactPoints != null && contactPoints.Length > 0)
            {
                float separation = contactPoints[0].separation;
                Vector3 contactPointNormal = contactPoints[0].normal;
                contactPointNormal.Normalize();

                Vector3 separationVector = (contactPointNormal) * separation;
                Vector3 contactMovePosition = contactPoints[0].point - separationVector;

                if (pointSphere != null && pointSphere.childCount == 8)
                {
                    pointSphere.GetChild(0).transform.position = contactPoints[0].point;
                    if (pointSphere.childCount > 5)
                    {
                        pointSphere.GetChild(5).transform.position = contactPoints[0].point;
                        pointSphere.GetChild(5).transform.parent = parent;

                    }
                }
                //测试点

                follow.transform.position = contactPoints[0].point;
                follow.transform.parent = parent;
                if (pointSphere != null && pointSphere.childCount == 8)
                {
                    if (pointSphere.childCount > 1)
                        pointSphere.GetChild(1).transform.position = contactMovePosition;
                }


                //求xz横截面
                Vector3 planeNormal;
                {
                    Vector3 x = parent.forward;
                    Vector3 y = parentparent.position - parent.position;
                    Vector3 z = Vector3.Cross(x, y);
                    Vector3 middlePoint = new Vector3(parent.position.x + (parentparent.position.x - parentparent.position.x) / 2,
                        parent.position.y + (parentparent.position.y - parent.position.y) / 2,
                        parent.position.z + (parentparent.position.z - parent.position.z) / 2);
                    Vector3 middleNoralPoint = middlePoint + z;
                    brother = mFinger.GetBrother(mFingerBoneType);
                    Vector3 e1 = middleNoralPoint - brother.position;
                    Vector3 e2 = middleNoralPoint - parent.position;
                    planeNormal = Vector3.Cross(e1, e2);
                    if (isDrawLine)
                    {
                        Debug.DrawLine(middlePoint, middleNoralPoint, Color.gray, 10000);
                        Debug.DrawLine(parent.position, parentparent.position, Color.gray, 10000);
                        Debug.DrawLine(parent.position, middleNoralPoint, Color.gray, 10000);
                        Debug.DrawLine(parentparent.position, middleNoralPoint, Color.gray, 10000);
                    }

                    planeNormal = planeNormal.normalized;
                    if (isDrawLine)
                        Debug.DrawLine(middleNoralPoint, middleNoralPoint + planeNormal, Color.gray, 10000);
                }

                //偏移投影到 XY平面 偏移量
                Vector3 moveXY, moveXY2;
                Vector3 v1;
                Vector3 v2;
                bool isRotation = false;
                Vector3 Vtemp1;
                float angleXY;
                {
                    Vector3 a = parent.position - contactPoints[0].point;
                    v1 = planeNormal * (Vector3.Dot(planeNormal, a)) + contactPoints[0].point;

                    Vector3 b = parent.position - contactMovePosition;
                    v2 = planeNormal * (Vector3.Dot(planeNormal, b)) + contactMovePosition;
                    if (isDrawLine)
                    {
                        Debug.DrawLine(parent.position, v1, Color.gray, 10000);
                        Debug.DrawLine(parentparent.position, v1, Color.gray, 10000);
                        Debug.DrawLine(parent.position, v2, Color.gray, 10000);
                        Debug.DrawLine(parentparent.position, v2, Color.gray, 10000);
                    }
                    if (pointSphere != null)
                    {
                        if (pointSphere.childCount > 2)
                            pointSphere.GetChild(2).transform.position = v1;
                        if (pointSphere.childCount > 3)
                            pointSphere.GetChild(3).transform.position = v2;
                    }
                    if (isDrawLine)
                        Debug.DrawLine(v1, v2, Color.green, 10000);
                    moveXY = v2 - v1;

                    Vector3 c = parent.position - follow.transform.position;
                    Vector3 v3 = planeNormal * (Vector3.Dot(planeNormal, c)) + follow.transform.position;
                    if (pointSphere != null && pointSphere.childCount > 0)
                    {
                        if (pointSphere.childCount > 4)
                            pointSphere.GetChild(4).transform.position = v3;
                    }
                    if (isDrawLine)
                    {
                        //Debug.DrawLine(v1, v3, Color.green, 10000);
                        //Debug.DrawLine(v2, v3, Color.green, 10000);
                    }

                    //xy平面碰撞偏移量
                    moveXY2 = v2 - v3;
                    if (Vector3.Dot(moveXY2, parent.up) <= 0.0f)
                    {
                        moveXY2 = v3 - v2;
                    }
                    angleXY = Vector3.Angle(parent.up, moveXY2);
                    if (isDrawLine)
                    {
                        Debug.DrawLine(parent.transform.position, v2, Color.red, 10000);
                        Debug.DrawLine(parent.transform.position, v3, Color.yellow, 10000);
                    }

                    isRotation = isFixed;
                    Vtemp1 = childs[1].transform.position - parent.transform.position;
                    if (Vector3.Dot(moveXY2, parent.up) >= 0.0f)
                    {
                        //float a1 = parent.transform.localEulerAngles.z;
                        //parent.transform.Rotate(-Vector3.forward, angleXY, Space.Self);
                        //float a2 = parent.transform.localEulerAngles.z;
                        //parent.transform.Rotate(-Vector3.forward, -angleXY, Space.Self);
                        ChangeFingerRotationFixed(angleXY);
                    }
                    else
                    {
                        //parent.transform.Rotate(-Vector3.forward, -angleXY, Space.Self);
                        //ChangeFingerRotationFixed(-angleXY);
                        ChangeFingerAngel(-angleXY);
                    }


                }

                Vector3 Vtemp2 = childs[1].transform.position - parent.transform.position;
                //调整第三根角度
                {
                    float angel1 = Vector3.Angle(Vtemp2, Vtemp1);

                    childs[0].transform.localRotation = Quaternion.Euler(childs[0].localEulerAngles.x,
                                                  childs[0].localEulerAngles.y,
                                                  childs[0].localEulerAngles.z + angel1);
                }
                    float angleParent = 0.0f;
                ////求移动后夹角 调整父节点位置
                //{
                //    Vector3 parentMovePosition = parent.transform.position - moveXY2;
                //    Vector3 R1 = parentMovePosition - parentparent.transform.position;
                //    Vector3 R2 = parent.transform.position - parentparent.transform.position;

                //    angleParent = Vector3.Angle(R2, R1);

                //    //parentparent.transform.localRotation = Quaternion.Euler(parentparent.transform.localEulerAngles.x,
                //    //                         parentparent.transform.localEulerAngles.y,
                //    //                         parentparent.transform.localEulerAngles.z - angleParent);
                //    mFinger.ParentRotationZFixed(-angleParent*2/3, Hi5_Glove_Finger_bone_Type.EOne);
                //}

                ////调整第三根角度
                //{
                //    //parent.transform.localRotation = Quaternion.Euler(parent.transform.localEulerAngles.x,
                //    //                        parent.transform.localEulerAngles.y,
                //    //                        parent.transform.localEulerAngles.z + angleParent);

                //    if (!isRotation)
                //        ChangeFingerAngelFixed(angleParent/3);
                //}

                //{
                //    //childs[0].transform.localRotation = Quaternion.Euler(childs[0].localEulerAngles.x,
                //    //                              childs[0].localEulerAngles.y,
                //    //                              childs[0].localEulerAngles.z + angleXY);
                //    //mFinger.
                //    //ChangeFingerAngel();
                //    float angleXYThree = Vector3.Angle(childs[0].transform.up, moveXY2);
                //    if (Vector3.Dot(moveXY2, parent.up) >= 0.0f)
                //    {
                //        mFinger.ParentRotationZ(-angleXY, Hi5_Glove_Finger_bone_Type.EThree);
                //    }
                //    else
                //    {
                //        mFinger.ParentRotationZ(angleXY, Hi5_Glove_Finger_bone_Type.EThree);
                //    }
                //}

            }
        }


        int countThree = 0;
		private void CalculateThreeRotation(ContactPoint[] contacts)
        {

            if (countThree != 0)
                return;
            countThree++;
            Transform parent = transform.parent.transform;
            Transform parentparent = transform.parent.parent.transform;
            Transform parentparentparent = transform.parent.parent.parent.transform;

            ContactPoint[] contactPoints = contacts;
            if (contactPoints != null && contactPoints.Length > 0)
            {
                //collision contact  Normal
                float separation = contactPoints[0].separation;
                Vector3 contactPointNormal = contactPoints[0].normal;
                contactPointNormal.Normalize();

                Vector3 separationVector = (contactPointNormal) * separation;
                Vector3 contactMovePosition = contactPoints[0].point - separationVector;
                if (pointSphere != null && pointSphere.childCount > 0)
                {
                    //测试点
                    if (pointSphere.childCount > 0)
                        pointSphere.GetChild(0).transform.position = contactPoints[0].point;
                    if (pointSphere.childCount > 5)
                    {
                        pointSphere.GetChild(5).transform.position = contactPoints[0].point;
                        pointSphere.GetChild(5).transform.parent = parent;

                    }
                }
                follow.transform.position = contactPoints[0].point;
                follow.transform.parent = parent;
                if (pointSphere != null && pointSphere.childCount > 0)
                {
                    if (pointSphere.childCount > 1)
                        pointSphere.GetChild(1).transform.position = contactMovePosition;
                }
                //求xz横截面
                Vector3 planeNormal;
                {
                    Vector3 x = parent.forward;
                    Vector3 y = parentparent.position - parent.position;
                    Vector3 z = Vector3.Cross(x, y);
                    Vector3 middlePoint = new Vector3(parent.position.x + (parentparent.position.x - parentparent.position.x) / 2,
                        parent.position.y + (parentparent.position.y - parent.position.y) / 2,
                        parent.position.z + (parentparent.position.z - parent.position.z) / 2);
                    Vector3 middleNoralPoint = middlePoint + z;
                    brother = mFinger.GetBrother(mFingerBoneType);
                    Vector3 e1 = middleNoralPoint - brother.position;
                    Vector3 e2 = middleNoralPoint - parent.position;
                    planeNormal = Vector3.Cross(e1, e2);
                    if (isDrawLine)
                    {
                        Debug.DrawLine(middlePoint, middleNoralPoint, Color.gray, 10000);
                        Debug.DrawLine(parent.position, parentparent.position, Color.gray, 10000);
                        Debug.DrawLine(parent.position, middleNoralPoint, Color.gray, 10000);
                        Debug.DrawLine(parentparent.position, middleNoralPoint, Color.gray, 10000);
                    }

                    planeNormal = planeNormal.normalized;
                    if (isDrawLine)
                        Debug.DrawLine(middleNoralPoint, middleNoralPoint + planeNormal, Color.gray, 10000);
                }


                //偏移投影到 XY平面 偏移量
                Vector3 moveXY, moveXY2;
                Vector3 v1;
                Vector3 v2;
                Vector3 v3;
                float angleXY;
                {
                    Vector3 a = parent.position - contactPoints[0].point;
                    v1 = planeNormal * (Vector3.Dot(planeNormal, a)) + contactPoints[0].point;

                    Vector3 b = parent.position - contactMovePosition;
                    v2 = planeNormal * (Vector3.Dot(planeNormal, b)) + contactMovePosition;
                    if (isDrawLine)
                    {
                        Debug.DrawLine(parent.position, v1, Color.gray, 10000);
                        Debug.DrawLine(parentparent.position, v1, Color.gray, 10000);

                        Debug.DrawLine(parent.position, v2, Color.gray, 10000);
                        Debug.DrawLine(parentparent.position, v2, Color.gray, 10000);
                    }
                    if (pointSphere != null && pointSphere.childCount > 0)
                    {
                        if (pointSphere.childCount > 2)
                            pointSphere.GetChild(2).transform.position = v1;
                        if (pointSphere.childCount > 3)
                            pointSphere.GetChild(3).transform.position = v2;
                    }
                    if (isDrawLine)
                        Debug.DrawLine(v1, v2, Color.green, 10000);
                    moveXY = v2 - v1;

                    //parent.transform.position += moveXY;

                    //XY平面 旋转角度
                    //float angleXY = Vector3.Angle(parent.up, moveXY);
                    //parent.transform.Rotate(-Vector3.forward, angleXY, Space.Self);

                    //follow.position += moveXY;
                    // follow x = 1.533665y = 0.8145986z = 2.34811

                    //Debug.Log("follow x="+ follow.position.x+ "y="+ follow.position.y+"z="+ follow.position.z);
                    Vector3 c = parent.position - follow.transform.position;
                    v3 = planeNormal * (Vector3.Dot(planeNormal, c)) + follow.transform.position;
                    if (pointSphere != null && pointSphere.childCount > 0)
                    {
                        if (pointSphere.childCount > 4)
                            pointSphere.GetChild(4).transform.position = v3;
                    }
                    if (isDrawLine)
                    {
                        Debug.DrawLine(v1, v3, Color.green, 10000);
                        Debug.DrawLine(v2, v3, Color.green, 10000);
                    }

                    //xy平面碰撞偏移量
                    moveXY2 = v2 - v3;
                    if (Vector3.Dot(moveXY2, parent.up) <= 0.0f)
                    {
                        moveXY2 = v3 - v2;
                    }
                    //angleXY = Vector3.Angle(parent.up, moveXY2);
                    //parent.transform.Rotate(-Vector3.forward, -angleXY, Space.Self);
                }

                //余旋定理求角度
                float R1 = 0.0f;
                {
                    float oneDistance = Vector3.Magnitude(parentparent.position - (parent.position + moveXY2));
                    float twoDistance = Vector3.Magnitude(parentparentparent.position - parentparent.position);
                    float threeDistance = Vector3.Magnitude((parent.position + moveXY2) - parentparentparent.position);
                    float CosR1 = (Mathf.Pow(twoDistance, 2) + Mathf.Pow(threeDistance, 2) - Mathf.Pow(oneDistance, 2))
                                    / (2 * twoDistance * threeDistance);
                    R1 = Mathf.Acos(CosR1);
                    //Debug.Log("CosR1 =" + CosR1 + "R1" + R1);
                    Vector3 v4 = (parent.position + moveXY2) - parentparentparent.position;
                    Vector2 v5 = Vector2RotationMatrix(new Vector2(v4.x, v4.y), R1);
                    Vector3 v6 = new Vector3(v5.x, v5.y, v4.z).normalized * twoDistance;
                    if (pointSphere != null && pointSphere.childCount > 0)
                    {
                        if (pointSphere.childCount > 6)
                            pointSphere.GetChild(6).transform.position = parentparentparent.position + v6;
                    }
                    if (isDrawLine)
                    {
                        Debug.DrawLine(parentparentparent.position, parentparentparent.position + v6, Color.blue, 10000);
                    }
                    //ChangeFingerAngel(-R1 * Mathf.Rad2Deg,);
                    mFinger.ParentRotationZFixed(-R1 * Mathf.Rad2Deg, Hi5_Glove_Finger_bone_Type.EOne);
                    //parentparentparent.transform.localRotation = Quaternion.Euler(parentparentparent.transform.localEulerAngles.x,
                    //parentparentparent.transform.localEulerAngles.y, parentparentparent.transform.localEulerAngles.z - R1 * Mathf.Rad2Deg);

                }
                //Debug.Log("R1 * Mathf.Rad2Deg =" + R1 * Mathf.Rad2Deg);
                angleXY = Vector3.Angle(parent.up, moveXY2) + R1 * Mathf.Rad2Deg;
                if (Vector3.Dot(moveXY2, parent.up) >= 0.0f)
                {
                    ChangeFingerAngelFixed(-angleXY);
                    //parent.transform.Rotate(-Vector3.forward, angleXY, Space.Self);
                }
                else
                {
                    ChangeFingerAngelFixed(angleXY);
                    //parent.transform.Rotate(-Vector3.forward, -angleXY, Space.Self);
                }



                //Debug.Log("parent.transform.localEulerAngles.z =" + parent.transform.localEulerAngles.z);
                //if (parent.transform.localEulerAngles.z > 180.0f)
                //{
                //    parent.transform.localRotation = Quaternion.Euler(parent.transform.localEulerAngles.x,
                //                             parent.transform.localEulerAngles.y,
                //                             parent.transform.localEulerAngles.z - 180.0f);
                //}
            }
        }
        int countone = 0;
		private void CalculateOneRotation(ContactPoint[] contacts)
        {
            if (countone != 0)
                return;
            countone++;
            Transform parent = transform.parent.transform;
            Transform parentparent = transform.parent.parent.transform;
            Transform parentparentparent = transform.parent.parent.parent.transform;
            ContactPoint[] contactPoints = contacts;

            if (contactPoints != null && contactPoints.Length > 0)
            {
                float separation = contactPoints[0].separation;
                Vector3 contactPointNormal = contactPoints[0].normal;
                contactPointNormal.Normalize();
                Vector3 separationVector = (contactPointNormal) * separation;
                Vector3 contactMovePosition = contactPoints[0].point - separationVector;

                //测试点
                if (pointSphere != null && pointSphere.childCount > 0)
                {
                    if (pointSphere.childCount > 0)
                        pointSphere.GetChild(0).transform.position = contactPoints[0].point;
                    if (pointSphere.childCount > 5)
                    {
                        pointSphere.GetChild(5).transform.position = contactPoints[0].point;
                        pointSphere.GetChild(5).transform.parent = parent;
                        //follow.transform.position = contactPoints[0].point;
                        //follow.transform.parent = parent;
                    }
                }

                follow.transform.position = contactPoints[0].point;
                follow.transform.parent = parent;
                if (pointSphere != null && pointSphere.childCount > 0)
                {
                    if (pointSphere.childCount > 1)
                        pointSphere.GetChild(1).transform.position = contactMovePosition;
                }
                //求xz横截面
                Vector3 planeNormal;
                {
                    Vector3 x = parent.transform.position;
                    Vector3 y = childs[0].position - parent.transform.position;
                    Vector3 z = Vector3.Cross(x, y);

                    Vector3 middlePoint = new Vector3(parent.transform.position.x + (childs[1].position.x - parent.transform.position.x) / 2,
                        parent.transform.position.y + (childs[1].position.y - parent.transform.position.y) / 2,
                        parent.transform.position.z + (childs[1].position.z - parent.transform.position.z) / 2);
                    Vector3 middleNoralPoint = middlePoint + z;
                    Vector3 e1 = middleNoralPoint - parent.position;
                    Vector3 e2 = middleNoralPoint - childs[0].position;

                    if (pointSphere != null && pointSphere.childCount > 0)
                    {
                        //if (pointSphere.childCount > 2)
                        //    pointSphere.GetChild(2).transform.position = middleNoralPoint;
                    }

                    planeNormal = Vector3.Cross(e1, e2);
                    if (isDrawLine)
                    {
                        Debug.DrawLine(middlePoint, middleNoralPoint, Color.gray, 10000);
                        Debug.DrawLine(parent.position, childs[0].position, Color.gray, 10000);
                        Debug.DrawLine(parent.position, middleNoralPoint, Color.gray, 10000);
                        Debug.DrawLine(childs[0].position, middleNoralPoint, Color.gray, 10000);
                    }

                    planeNormal = planeNormal.normalized;
                    if (isDrawLine)
                        Debug.DrawLine(middleNoralPoint, middleNoralPoint + planeNormal, Color.gray, 10000);
                }


                //偏移投影到 XY平面 偏移量
                Vector3 moveXY, moveXY2;
                Vector3 v1;
                Vector3 v2;
                float angleXY;
                {
                    Vector3 a = parent.position - contactPoints[0].point;
                    v1 = planeNormal * (Vector3.Dot(planeNormal, a)) + contactPoints[0].point;

                    Vector3 b = parent.position - contactMovePosition;
                    v2 = planeNormal * (Vector3.Dot(planeNormal, b)) + contactMovePosition;
                    if (isDrawLine)
                    {
                        Debug.DrawLine(parent.position, v1, Color.gray, 10000);
                        //Debug.DrawLine(childs.position, v1, Color.gray, 10000);

                        Debug.DrawLine(parent.position, v2, Color.gray, 10000);
                        //Debug.DrawLine(parentparent.position, v2, Color.gray, 10000);
                    }
                    if (pointSphere != null && pointSphere.childCount > 0)
                    {
                        if (pointSphere.childCount > 2)
                            pointSphere.GetChild(2).transform.position = v1;
                    }

                    if (pointSphere != null && pointSphere.childCount > 0)
                    {
                        if (pointSphere.childCount > 3)
                            pointSphere.GetChild(3).transform.position = v2;
                    }
                    //if (pointSphere.Length > 2)
                    //    pointSphere[2].transform.position = v1;
                    //if (pointSphere.Length > 3)
                    //    pointSphere[3].transform.position = v2;
                    if (isDrawLine)
                        Debug.DrawLine(v1, v2, Color.green, 10000);
                    moveXY = v2 - v1;
                    //parent.transform.position += moveXY;


                    //Debug.Log("follow x="+ follow.position.x+ "y="+ follow.position.y+"z="+ follow.position.z);
                    Vector3 c = parent.position - follow.transform.position;
                    Vector3 v3 = planeNormal * (Vector3.Dot(planeNormal, c)) + follow.transform.position;
                    if (pointSphere != null && pointSphere.childCount > 0)
                    {
                        if (pointSphere.childCount > 4)
                            pointSphere.GetChild(4).transform.position = v3;
                    }
                    if (isDrawLine)
                    {
                        Debug.DrawLine(parent.transform.position, v3, Color.green, 10000);
                        Debug.DrawLine(parent.transform.position, v2, Color.green, 10000);
                        //Debug.DrawLine(parent.transform.position, v3, Color.green, 10000);
                        Debug.DrawLine(v3, v2, Color.green, 10000);
                    }

                    //xy平面碰撞偏移量
                    moveXY2 = v2 - v3;
                    if (Vector3.Dot(moveXY2, parent.up) <= 0.0f)
                    {
                        moveXY2 = v3 - v2;
                    }
                    angleXY = Vector3.Angle(parent.up, moveXY2);


                    Vector3 temp = childs[2].position;

                    float oneDistance = Vector3.Magnitude(childs[2].position - childs[1].position);
                    float twoDistance = Vector3.Magnitude(childs[1].position - childs[0].position);
                    if (Vector3.Dot(moveXY2, parent.up) >= 0.0f)
                    {
                        ChangeFingerRotationFixed(angleXY);
                        //parent.transform.Rotate(-Vector3.forward, angleXY, Space.Self);
                    }
                    else
                    {
                        ChangeFingerRotationFixed(-angleXY);
                        //parent.transform.Rotate(-Vector3.forward, -angleXY, Space.Self);
                    }


                    float threeDistance = Vector3.Magnitude(childs[2].position - childs[0].position);
                    float CosR1 = (Mathf.Pow(twoDistance, 2) + Mathf.Pow(threeDistance, 2) - Mathf.Pow(oneDistance, 2))
                                   / (2 * twoDistance * threeDistance);
                    float R1 = Mathf.Acos(CosR1);


                    childs[0].transform.localRotation = Quaternion.Euler(childs[0].localEulerAngles.x,
                                                    childs[0].localEulerAngles.y,
                                                    childs[0].localEulerAngles.z + R1 * Mathf.Rad2Deg);

                    //mFinger.ParentRotationZFixed(-R1 * Mathf.Rad2Deg, Hi5_Glove_Finger_bone_Type.ETwo);
                    ////Debug.Log("CosR1 =" + CosR1 + "R1" + R1);
                    return;
                }
            }
        }
        internal void Reset()
        {
            count = 0;
            isFixed = false;
            countTwo = 0;
            countThree = 0;
            countone = 0;
            //Debug.Log("Reset rotation");
            //Debug.Log("Reset rotation");
        }

        internal int count = 0;
		internal virtual bool CalculateRotation(ContactPoint[] contacts)
        {
            //if (count != 0)
            //    return false;
            //count++;
            if (mFingerBoneType == Hi5_Glove_Finger_bone_Type.EThree)
            {
                //Debug.Log("EThree");
				CalculateThreeRotation(contacts);
            }
            else if (mFingerBoneType == Hi5_Glove_Finger_bone_Type.ETwo)
            {
                //Debug.Log("ETwo");
				CalculateTwoRotation(contacts);
            }
            else if (mFingerBoneType == Hi5_Glove_Finger_bone_Type.EOne)
            {
                //Debug.Log("EOne");
                //caculateOneRotation(contacts);
            }
            return true ;
        }

        private Vector2 Vector2RotationMatrix(Vector2 v, float angle)
        {
            var x = v.x;
            var y = v.y;
            var sin = Mathf.Sin(Mathf.PI * angle / 180);
            var cos = Mathf.Cos(Mathf.PI * angle / 180);
            var newX = x * cos + y * sin;
            var newY = x * -sin + y * cos;
            return new Vector2((float)newX, (float)newY);
        }


        
		 override protected void OnCollisionEnter(Collision collision)
        {
            //base.OnCollisionEnter(collision);
           
        }

        float GetAngle(Transform leftTransform, Transform midTransform, Transform rightTransform)
        {
            Vector3 left = leftTransform.position;
            Vector3 middle = midTransform.position;
            Vector3 right = rightTransform.position; 
            Vector3 vLeft = left - middle;
            Vector3 vRight = right - middle;
            float angle = Vector3.Angle(vLeft, vRight);
            // float angle1 = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(vLeft, vRight) / (vLeft.magnitude * vRight.magnitude));
            return angle;
        }

        virtual protected void Update()
        {
			//if (Hi5_Layer_Set.IsResetLayer && isAttributeSet && !isSetLayer) {
			//	if (mFingerBoneType == Hi5_Glove_Finger_bone_Type.EFour) {
			//		gameObject.layer = LayerMask.NameToLayer ("Hi5OtherFingerTail");
			//	}
			//	else 
			//		gameObject.layer = LayerMask.NameToLayer ("Hi5OtherFingerOther");
			//	isSetLayer = true;
			//}
            if (contactPoints != null)
            {

            }

            //if (saveTime > 0.0f)
            //{
            //    saveTime -= Time.deltaTime;
            //    //Debug.Log("saveTime"+ saveTime);
            //}
            //if (saveTime < 0.0f)
            //    saveTime = 0.0f;

        }


        protected bool isFixed = false;
        virtual protected internal void ChangeFingerAngelFixed(float Angel)
        {
            //if (isFixed)
            //{
            //    if (mFingerBoneType == Hi5_Glove_Finger_bone_Type.EThree)
            //    {
            //    }
            //    else
            //        return;
            //}
            //isFixed = true;
            Transform parent = transform.parent;
            float currentZAngle = parent.localEulerAngles.z;
            float ZeulerAngle = currentZAngle + Angel;
            float valueAngle = 0.0f;
            if (currentZAngle > 180.0f)
                currentZAngle -= 360;
            if (Angel < 0)
            {
                valueAngle = (mZMinAngel > (currentZAngle + Angel)) ? mZMinAngel : (currentZAngle + Angel);
            }
            else
            {
                valueAngle = (mZMaxAngel < (currentZAngle + Angel)) ? mZMaxAngel : (currentZAngle + Angel);
            }
            //
           // parent.localRotation = Quaternion.Euler(parent.localEulerAngles.x, parent.localEulerAngles.y, ZeulerAngle);
           if(!float.IsNaN(valueAngle))
                parent.localRotation = Quaternion.Euler(parent.localEulerAngles.x, parent.localEulerAngles.y, valueAngle);
        }

        virtual protected internal void ChangeFingerAngel(float Angel)
        {
            Transform parent = transform.parent;
            float currentZAngle = parent.localEulerAngles.z;
            float ZeulerAngle = currentZAngle + Angel;
            float valueAngle = 0.0f;
            if (currentZAngle > 180.0f)
                currentZAngle -= 360;
            if (Angel < 0)
            {
                valueAngle = (mZMinAngel > (currentZAngle + Angel)) ? mZMinAngel : (currentZAngle + Angel);
            }
            else
            {
                valueAngle = (mZMaxAngel < (currentZAngle + Angel)) ? mZMaxAngel : (currentZAngle + Angel);
            }
            //parent.localRotation = Quaternion.Euler(parent.localEulerAngles.x, parent.localEulerAngles.y, ZeulerAngle);
            parent.localRotation = Quaternion.Euler(parent.localEulerAngles.x, parent.localEulerAngles.y, valueAngle);
        }

        virtual protected internal void ChangeFingerRotationFixed(float Angel)
        {
            //if (isFixed)
            //{
            //    if (mFingerBoneType == Hi5_Glove_Finger_bone_Type.EThree)
            //    {
            //    }
            //    else
            //        return;
            //}
            //isFixed = true;
            Transform parent = transform.parent;
            //float preAngel = parent.localEulerAngles.z;

            parent.transform.Rotate(-Vector3.forward, Angel, Space.Self);
            float currentAngel = parent.localEulerAngles.z;
            if (currentAngel > 180.0f)
                currentAngel -= 360;
            float valueAngle = 0.0f;
            if (currentAngel < 0)
            {
                valueAngle = (mZMinAngel > currentAngel) ? mZMinAngel : (currentAngel);
            }
            else
            {
                valueAngle = (mZMaxAngel < currentAngel) ? mZMaxAngel : currentAngel;
            }
            parent.transform.localRotation = Quaternion.Euler(parent.localEulerAngles.x, parent.localEulerAngles.y, valueAngle);
        }

        private void RecordPosition(float deltaTime)
        {
            if (mQueuePositionRecord.Count > (Hi5_Interaction_Const.ObjectPinchRecordPositionCount - 1))
            {
                mQueuePositionRecord.Dequeue();
            }
            Hi5_Position_Record record = new Hi5_Position_Record(transform.position, prePositionRecord, deltaTime);
            mQueuePositionRecord.Enqueue(record);
            prePositionRecord = transform.position;
        }
         
    }
}

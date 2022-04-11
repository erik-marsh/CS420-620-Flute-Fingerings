using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hi5_Interaction_Core
{
    public class Hi5_Hand_Collider_Visible_Thumb_Finger : Hi5_Hand_Collider_Visible_Finger
    {
        internal float mYMaxAngel = 0.0f;
        internal float mYMinAngel = 0.0f;
       
        public override void setFinger(Hi5_Hand_Visible_Hand hand,
                                Hi5_Hand_Visible_Finger finger,
                              Hi5_Glove_Finger_bone_Type finger_bone_Type,
                               float zMaxAngel,
                               float zMinAngel)
        {
            mHand = hand;
            mFinger = finger;
            mFingerBoneType = finger_bone_Type;
            mZMinAngel = zMinAngel;
            mZMaxAngel = zMaxAngel;
			isAttributeSet = true;
        }

        public void SetYAngle(float YMax, float YMin)
        {
            mYMaxAngel = YMax;
            mYMinAngel = YMin;
        }



        override protected void OnCollisionStay(Collision collision)
        {
            //Objectscripe scripe = collision.collider.gameObject.GetComponent<Objectscripe>();
            //if (scripe != null)
            //{
            //    scripe.isStop = true;
            //}

            ContactPoint[] contactPoints = collision.contacts;
            Hi5_Hand_CollisionData data = new Hi5_Hand_CollisionData();
            data.collider_finger = this;
            data.constactPoint = contactPoints;
            //caculateRotation(contactPoints);
            if(Hi5_Interaction_Const.IsUseVisibleHand)
                mFinger.AddDicCollision(mFingerBoneType, data);
        }

		void CalculateThreeRotation(ContactPoint[] contacts)
        {
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
                {
                    Vector3 planeNormal;
                    Vector3 x = parent.forward;
                    Vector3 y = parentparent.position - parent.position;
                    Vector3 z = Vector3.Cross(x, y);
                    Vector3 middlePoint = new Vector3(parent.position.x + (parentparent.position.x - parentparent.position.x) / 2,
                        parent.position.y + (parentparent.position.y - parent.position.y) / 2,
                        parent.position.z + (parentparent.position.z - parent.position.z) / 2);
                    Vector3 middleNoralPoint = middlePoint + z;

                    if (pointSphere != null && pointSphere.childCount == 8)
                    {
                        if (pointSphere.childCount > 2)
                            pointSphere.GetChild(2).transform.position = middlePoint;
                    }

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
                            Debug.DrawLine(parentparent.transform.position, v3, Color.green, 10000);
                            Debug.DrawLine(parentparent.transform.position, v2, Color.green, 10000);
                        }

                        //xy平面碰撞偏移量
                        angleXY = Vector3.Angle(v3 - parentparent.transform.position, v2 - parentparent.transform.position);
                        moveXY2 = v2 - v3;
                        if (Vector3.Dot(moveXY2, parent.up) <= 0.0f)
                        {
                            ChangeFingerRotationFixed(-angleXY);
                        }
                        else
                            ChangeFingerRotationFixed(angleXY);
                    }
                }
                //求YX横截面
                {
                    Vector3 planeNormal;
                    Vector3 x = parent.up;
                    Vector3 y = parentparent.position - parent.position;
                    Vector3 z = Vector3.Cross(x, y);
                    Vector3 middlePoint = new Vector3(parent.position.x + (parentparent.position.x - parentparent.position.x) / 2,
                        parent.position.y + (parentparent.position.y - parent.position.y) / 2,
                        parent.position.z + (parentparent.position.z - parent.position.z) / 2);
                    Vector3 middleNoralPoint = middlePoint + z;

                    if (pointSphere != null && pointSphere.childCount == 8)
                    {
                        if (pointSphere.childCount > 6)
                            pointSphere.GetChild(6).transform.position = middlePoint;
                    }

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
                            if (pointSphere.childCount > 7)
                                pointSphere.GetChild(7).transform.position = v1;
                        }

                        if (pointSphere != null && pointSphere.childCount > 0)
                        {
                            if (pointSphere.childCount > 5)
                                pointSphere.GetChild(5).transform.position = v2;
                        }
                        //if (pointSphere.Length > 2)
                        //    pointSphere[2].transform.position = v1;
                        //if (pointSphere.Length > 3)
                        //    pointSphere[3].transform.position = v2;
                        if (isDrawLine)
                            Debug.DrawLine(v1, v2, Color.red, 10000);
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
                            Debug.DrawLine(parentparent.transform.position, v3, Color.red, 10000);
                            Debug.DrawLine(parentparent.transform.position, v2, Color.red, 10000);
                        }

                        //xy平面碰撞偏移量
                        angleXY = Vector3.Angle(v3 - parentparent.transform.position, v2 - parentparent.transform.position);
                        moveXY2 = v2 - v3;
                        if (Vector3.Dot(moveXY2, parent.forward) <= 0.0f)
                        {
                            ChangeFingerYRotation(-angleXY);
                        }
                        else
                            ChangeFingerYRotation(angleXY);
                    }
                }
            }
        }

		private void CalculateTwoRotation(ContactPoint[] contacts)
        {
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
                {
                    Vector3 planeNormal;
                    Vector3 x = parent.forward;
                    Vector3 y = parentparent.position - parent.position;
                    Vector3 z = Vector3.Cross(x, y);
                    Vector3 middlePoint = new Vector3(parent.position.x + (parentparent.position.x - parentparent.position.x) / 2,
                        parent.position.y + (parentparent.position.y - parent.position.y) / 2,
                        parent.position.z + (parentparent.position.z - parent.position.z) / 2);
                    Vector3 middleNoralPoint = middlePoint + z;

                    if (pointSphere != null && pointSphere.childCount == 8)
                    {
                        if (pointSphere.childCount > 2)
                            pointSphere.GetChild(2).transform.position = middlePoint;
                    }

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
                            Debug.DrawLine(parentparent.transform.position, v3, Color.green, 10000);
                            Debug.DrawLine(parentparent.transform.position, v2, Color.green, 10000);
                        }

                        //xy平面碰撞偏移量
                        angleXY = Vector3.Angle(v3 - parentparent.transform.position, v2 - parentparent.transform.position);
                        moveXY2 = v2 - v3;
                        if (Vector3.Dot(moveXY2, parent.up) <= 0.0f)
                        {
                            ChangeFingerRotationFixed(-angleXY);
                        }
                        else
                            ChangeFingerRotationFixed(angleXY);
                    }
                }
                //求YX横截面
                {
                    Vector3 planeNormal;
                    Vector3 x = parent.up;
                    Vector3 y = parentparent.position - parent.position;
                    Vector3 z = Vector3.Cross(x, y);
                    Vector3 middlePoint = new Vector3(parent.position.x + (parentparent.position.x - parentparent.position.x) / 2,
                        parent.position.y + (parentparent.position.y - parent.position.y) / 2,
                        parent.position.z + (parentparent.position.z - parent.position.z) / 2);
                    Vector3 middleNoralPoint = middlePoint + z;

                    if (pointSphere != null && pointSphere.childCount == 8)
                    {
                        if (pointSphere.childCount > 6)
                            pointSphere.GetChild(6).transform.position = middlePoint;
                    }

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
                            if (pointSphere.childCount > 7)
                                pointSphere.GetChild(7).transform.position = v1;
                        }

                        if (pointSphere != null && pointSphere.childCount > 0)
                        {
                            if (pointSphere.childCount > 5)
                                pointSphere.GetChild(5).transform.position = v2;
                        }
                        //if (pointSphere.Length > 2)
                        //    pointSphere[2].transform.position = v1;
                        //if (pointSphere.Length > 3)
                        //    pointSphere[3].transform.position = v2;
                        if (isDrawLine)
                            Debug.DrawLine(v1, v2, Color.red, 10000);
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
                            Debug.DrawLine(parentparent.transform.position, v3, Color.red, 10000);
                            Debug.DrawLine(parentparent.transform.position, v2, Color.red, 10000);
                        }

                        //xy平面碰撞偏移量
                        angleXY = Vector3.Angle(v3 - parentparent.transform.position, v2 - parentparent.transform.position);
                        moveXY2 = v2 - v3;
                        if (Vector3.Dot(moveXY2, parent.forward) <= 0.0f)
                        {
                            ChangeFingerYRotation(-angleXY);
                        }
                        else
                            ChangeFingerYRotation(angleXY);
                    }
                }
            }
        }

		internal override bool CalculateRotation(ContactPoint[] contacts)
        {

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
            return true;
        }

        //ContactPoint[] contactPoints = contacts;
        //if (contactPoints != null && contactPoints.Length > 0)
        //{
        //    Transform parentparent = transform.parent.parent.transform;
        //    Transform parent = transform.parent.transform;

        //    //collision contact  Normal
        //    float separation = contactPoints[0].separation;
        //    Vector3 contactPointNormal = contactPoints[0].normal;
        //    contactPointNormal.Normalize();
        //    //Debug.Log("OnCollisionStay separation = " + separation);
        //    //separation = GetMaxseqaration (separation);
        //    //Debug.Log("OnCollisionStay separation = " + separation);
        //    //Debug.Log("caculateRotation contactPointNormal = " + contactPointNormal);
        //    //collision contact  separation point
        //    Vector3 separationVector = (contactPointNormal) * separation;
        //    Vector3 contactMovePosition = transform.position + separationVector;

        //    // vector of collision contact point  to parent point and vector of collision contact move point  to parent point angle
        //    Vector3 contactPosition = contactPoints[0].point;
        //    float angleOne = Vector3.Angle(contactMovePosition - transform.parent.parent.position,
        //        contactPosition - transform.parent.parent.position);
        //    //object1.transform.position = transform.position;
        //    //object2.transform.position = contactMovePosition;
        //    //            object3.transform.position = transform.position - separationVector;
        //    //test angleOne 
        //    //parentparent.Rotate(new Vector3(0.0f, 0.0f, -angleOne / 3), Space.Self);

        //    // plane (parant  point , parentparent point, parent forward) 
        //    Vector3 x = parent.up;
        //    Vector3 y = parentparent.position - parent.position;
        //    Vector3 z = Vector3.Cross(x, y);
        //    //z.Normalize();
        //    Vector3 middlePoint = new Vector3(parent.position.x + (parentparent.position.x - parent.position.x) / 2,
        //        parent.position.y + (parentparent.position.y - parent.position.y) / 2,
        //        parent.position.z + (parentparent.position.z - parent.position.z) / 2);
        //    Vector3 middleNoralPoint = middlePoint - z;
        //    if (isDrawLine)
        //    {
        //        Debug.DrawLine(parentparent.position, contactPosition, Color.blue, 10000);
        //        Debug.DrawLine(parentparent.position, contactMovePosition, Color.red, 10000);
        //        Debug.DrawLine(parent.position, contactPosition, Color.green, 10000);
        //        Debug.DrawLine(parent.position, contactMovePosition, Color.yellow, 10000);
        //        Debug.DrawLine(middlePoint, middleNoralPoint, Color.black, 10000);
        //        Debug.DrawLine(parent.position, parentparent.position, Color.black, 10000);
        //        Debug.DrawLine(parent.position, middleNoralPoint, Color.black, 10000);
        //        Debug.DrawLine(parentparent.position, middleNoralPoint, Color.black, 10000);
        //    }

        //    //plane normal
        //    Vector3 e1 = middleNoralPoint - parent.position;
        //    Vector3 e2 = middleNoralPoint - parentparent.position;
        //    Vector3 planeNormal = Vector3.Cross(e1, e2);
        //    planeNormal = planeNormal.normalized;
        //    if (isDrawLine)
        //        Debug.DrawLine(middleNoralPoint, middleNoralPoint + planeNormal, Color.gray, 10000);

        //    Vector3 v1 = contactMovePosition - parentparent.position - planeNormal * (Vector3.Dot(planeNormal, contactMovePosition - parentparent.position));
        //    Vector3 v2 = contactPosition - parentparent.position - planeNormal * (Vector3.Dot(planeNormal, contactPosition - parentparent.position));
        //    //Debug.Log("v1="+ v1.normalized);
        //    //Debug.Log("v2=" + v2.normalized);
        //    //parent Bone z rotattion angle
        //    float anglez = Vector3.Angle(v1, v2);
        //    ///Debug.Log("caculateRotation anglez = " + anglez);
        //    //Vector3.Cross(contactPointNormal, parentparent.forward);

        //    if (mFinger != null)
        //    {
        //        if (Vector3.Dot(contactPointNormal, parentparent.forward) >= 0.0f)
        //            mFinger.ParentRotationY(anglez, mFingerBoneType);
        //        else
        //            mFinger.ParentRotationY(-anglez, mFingerBoneType);
        //    }

        //    //parentparent.Rotate(new Vector3(0.0f, 0.0f, -anglez), Space.Self);
        //}
    

        protected bool isFixedY = false;
        virtual protected internal void ChangeFingerYRotation(float Angel)
        {
            if (isFixedY)
                return;
            isFixedY = true;
            Transform parent = transform.parent;
            Transform parentparent = transform.parent.parent;
            float zMaxTotalAngel = 0.0f;
            float zMinTotalAngel = 0.0f;
            float yMaxTotalAngel = 0.0f;
            float yMinTotalAngel = 0.0f;

            float zMaxAngel = 0.0f;
            float zMinAngel = 0.0f;
            float yMaxAngel = 0.0f;
            float yMinAngel = 0.0f;
            float anoterAngel = 0.0f;
            float yMax, yMin = 0.0f;
            if (mFingerBoneType == Hi5_Glove_Finger_bone_Type.EThree)
            {
                Hi5_Hand_Collider_Visible_Finger mParentFinger = mFinger.GetChildCollider(Hi5_Glove_Finger_bone_Type.ETwo);
                Hi5_Hand_Collider_Visible_Thumb_Finger mTumbFinger = mParentFinger as Hi5_Hand_Collider_Visible_Thumb_Finger;
                Hi5_Hand_Visible_Thumb_Finger finger = mFinger as Hi5_Hand_Visible_Thumb_Finger;
                if (finger != null)
                {
                    zMaxTotalAngel = finger.zMaxTotalAngel;
                    zMinTotalAngel = finger.zMinTotalAngel;
                    yMaxTotalAngel = finger.yMaxTotalAngel;
                    yMinTotalAngel = finger.yMinTotalAngel;
                }
                if (mTumbFinger != null)
                {
                    yMaxAngel = mTumbFinger.mYMaxAngel;
                    yMinAngel = mTumbFinger.mYMinAngel;
                    zMaxAngel = mTumbFinger.mZMaxAngel;
                    zMinAngel = mTumbFinger.mZMinAngel;
                }
                Transform anFinger = parentparent.parent.transform;//mFinger.GetChildCollider(Hi5_Glove_Finger_bone_Type.EOne);
                anoterAngel = anFinger.transform.localEulerAngles.y;

            }
            else if (mFingerBoneType == Hi5_Glove_Finger_bone_Type.ETwo)
            {
                //Hi5_Hand_Collider_Visible_Finger mParentFinger = mFinger.GetChildCollider(Hi5_Glove_Finger_bone_Type.EOne);
                //Hi5_Hand_Collider_Visible_Thumb_Finger mTumbFinger = mParentFinger as Hi5_Hand_Collider_Visible_Thumb_Finger;
                Hi5_Hand_Visible_Thumb_Finger finger = mFinger as Hi5_Hand_Visible_Thumb_Finger;
                if (finger != null)
                {
                    zMaxTotalAngel = finger.zMaxTotalAngel;
                    zMinTotalAngel = finger.zMinTotalAngel;
                    yMaxTotalAngel = finger.yMaxTotalAngel;
                    yMinTotalAngel = finger.yMinTotalAngel;
                }
                if (finger != null)
                {
                    yMaxAngel = finger.m_OneBoneYMaxAngle;
                    yMinAngel = finger.m_OneBoneYMinAngle;
                    zMaxAngel = finger.m_OneBoneMaxAngle;
                    zMinAngel = finger.m_OneBoneMinAngle;
                }
                Hi5_Hand_Collider_Visible_Finger anFinger = mFinger.GetChildCollider(Hi5_Glove_Finger_bone_Type.ETwo);
                anoterAngel = anFinger.transform.localEulerAngles.y;
            }
            float MaxAngle = yMaxAngel;
            if (MaxAngle > yMaxTotalAngel - anoterAngel)
                MaxAngle = yMaxTotalAngel - anoterAngel;
            float MinAngle = yMinAngel;
            if (MinAngle < yMinTotalAngel - anoterAngel)
                MinAngle = yMinTotalAngel - anoterAngel;

            parentparent.transform.Rotate(-Vector3.up, Angel, Space.Self);
            float currentAngel = parentparent.localEulerAngles.y;
            if (currentAngel > 180.0f)
                currentAngel -= 360;
            if (currentAngel > MaxAngle)
                currentAngel = MaxAngle;
            //parentparent.localRotation = Quaternion.Euler(parentparent.localEulerAngles.x,
            //                                            currentAngel,
            //                                            parentparent.localEulerAngles.z);
        }

        override protected internal void ChangeFingerRotationFixed(float Angel)
        {
            if (isFixed)
                return;
            isFixed = true;


            Transform parentparent = transform.parent.parent;
            Transform parent = transform.parent;

            float zMaxTotalAngel = 0.0f;
            float zMinTotalAngel = 0.0f;
            float yMaxTotalAngel = 0.0f;
            float yMinTotalAngel = 0.0f;

            float zMaxAngel = 0.0f;
            float zMinAngel = 0.0f;
            float yMaxAngel = 0.0f;
            float yMinAngel = 0.0f;
            float anoterAngel = 0.0f;
            float zMax, zMin = 0.0f;
            if (mFingerBoneType == Hi5_Glove_Finger_bone_Type.EThree)
            {
                Hi5_Hand_Collider_Visible_Finger mParentFinger = mFinger.GetChildCollider(Hi5_Glove_Finger_bone_Type.ETwo);
                Hi5_Hand_Collider_Visible_Thumb_Finger mTumbFinger = mParentFinger as Hi5_Hand_Collider_Visible_Thumb_Finger;
                Hi5_Hand_Visible_Thumb_Finger finger = mFinger as Hi5_Hand_Visible_Thumb_Finger;
                if (finger != null)
                {
                    zMaxTotalAngel = finger.zMaxTotalAngel;
                    zMinTotalAngel = finger.zMinTotalAngel;
                    yMaxTotalAngel = finger.yMaxTotalAngel;
                    yMinTotalAngel = finger.yMinTotalAngel;
                }
                if (mTumbFinger != null)
                {
                    yMaxAngel = mTumbFinger.mYMaxAngel;
                    yMinAngel = mTumbFinger.mYMinAngel;
                    zMaxAngel = mTumbFinger.mZMaxAngel;
                    zMinAngel = mTumbFinger.mZMinAngel;
                }
                //Hi5_Hand_Collider_Visible_Finger anFinger = mFinger.GetChildCollider(Hi5_Glove_Finger_bone_Type.EOne);
                anoterAngel = parentparent.parent.transform.localEulerAngles.z;
            }
            else if (mFingerBoneType == Hi5_Glove_Finger_bone_Type.ETwo)
            {
                // Hi5_Hand_Collider_Visible_Finger mParentFinger = mFinger.GetChildCollider(Hi5_Glove_Finger_bone_Type.EOne);
                // Hi5_Hand_Collider_Visible_Thumb_Finger mTumbFinger = mParentFinger as Hi5_Hand_Collider_Visible_Thumb_Finger;
                Hi5_Hand_Visible_Thumb_Finger finger = mFinger as Hi5_Hand_Visible_Thumb_Finger;
                if (finger != null)
                {
                    zMaxTotalAngel = finger.zMaxTotalAngel;
                    zMinTotalAngel = finger.zMinTotalAngel;
                    yMaxTotalAngel = finger.yMaxTotalAngel;
                    yMinTotalAngel = finger.yMinTotalAngel;
                }
                if (finger != null)
                {
                    yMaxAngel = finger.m_OneBoneYMaxAngle;
                    yMinAngel = finger.m_OneBoneYMinAngle;
                    zMaxAngel = finger.m_OneBoneMaxAngle;
                    zMinAngel = finger.m_OneBoneMinAngle;
                }
                Hi5_Hand_Collider_Visible_Finger anFinger = mFinger.GetChildCollider(Hi5_Glove_Finger_bone_Type.ETwo);
                anoterAngel = anFinger.transform.localEulerAngles.z;
            }
            float MaxAngle = zMaxAngel;
            if (MaxAngle > zMaxTotalAngel - anoterAngel)
                MaxAngle = zMaxTotalAngel - anoterAngel;
            float MinAngle = zMinAngel;
            if (MinAngle < zMinTotalAngel - anoterAngel)
                MinAngle = zMinTotalAngel - anoterAngel;


            parentparent.transform.Rotate(-Vector3.forward, Angel, Space.Self);

            float currentAngel = parentparent.localEulerAngles.z;
            if (currentAngel > 180.0f)
                currentAngel -= 360;
            if (currentAngel > MaxAngle)
                currentAngel = MaxAngle;
            //parentparent.localRotation = Quaternion.Euler(parentparent.localEulerAngles.x,
            //                                            parentparent.localEulerAngles.y,
            //                                            currentAngel);
        }


        //override protected internal bool ChangeFingerAngel(float Angel, out float parentAngle)
        //{
        //    //Debug.Log("Y angle =" + Angel);

        //        bool isParentChangAngel = false;
        //        Transform parent = transform.parent;
        //        float currentYAngle = parent.localRotation.eulerAngles.y;
        //    if (currentYAngle > 180.0f)
        //        currentYAngle -= 360;
        //   // Debug.Log("currentYAngle =" + currentYAngle);


        //    float valueAngle = 0.0f;
        //        if (Angel < 0)
        //        {
        //        //valueAngle = currentYAngle + Angel;
        //        //isParentChangAngel = false;
        //        //parentAngle = 0.0f;
        //        valueAngle = (mZMinAngel > (currentYAngle + Angel)) ? mZMinAngel : (currentYAngle + Angel);
        //        //Debug.Log("valueAngleMin =" + valueAngle);
        //        if (valueAngle == mZMinAngel)
        //        {
        //            parentAngle = currentYAngle + Angel - mZMinAngel;
        //            //Debug.Log("parentAngleMin =" + parentAngle);
        //            isParentChangAngel = true;
        //        }
        //        else
        //        {
        //            parentAngle = 0.0f;
        //            //Debug.Log("parentAngleMin =" + parentAngle);
        //            isParentChangAngel = false;
        //        }
        //    }
        //        else
        //        {
        //        //valueAngle = currentYAngle + Angel;
        //        //isParentChangAngel = false;
        //        //parentAngle = 0.0f;

        //        valueAngle = (mZMaxAngel < (currentYAngle + Angel)) ? mZMaxAngel : (currentYAngle + Angel);
        //        //Debug.Log("valueAngle Max =" + valueAngle);
        //        if (valueAngle == mZMaxAngel)
        //        {
        //            parentAngle = currentYAngle + Angel - mZMaxAngel;
        //            //Debug.Log("parentAngleMax =" + parentAngle);
        //            isParentChangAngel = true;
        //        }
        //        else
        //        {
        //            parentAngle = 0.0f;
        //            //Debug.Log("parentAngleMax =" + parentAngle);
        //            isParentChangAngel = false;
        //        }
        //    }
        //        //Quaternion temp = Quaternion.Euler(0.0f, 0.0f, valueAngle);
        //        //if (mFingerBoneType == Hi5_Glove_Finger_bone_Type.)
        //        //parent.Rotate(new Vector3(0.0f, 0.0f, valueAngle - currentZAngle), Space.Self);
        //        parent.transform.localRotation = Quaternion.Euler(0.0f, valueAngle, 0.0f);
        //        //isRotationZ = false;
        //        return isParentChangAngel;
        //    //}
        //    //else
        //    //{
        //    //    parentAngle = Angel;
        //    //    return true;
        //    //}
        //}
        // override  protected internal bool ChangeFingerAngel(float Angel, out float parentAngle)
        //{

        //    //if (isRotationZ)
        //    {
        //        //Debug.Log("ChangeFingerAngel type = "+ mFingerBoneType);
        //        bool isParentChangAngel = false;
        //        Transform parent = transform.parent;
        //        float currentYAngle = parent.localRotation.y;
        //        float zMinAngel = mZMinAngel * Mathf.Deg2Rad / 2.0f;
        //        float zMaxAngel = mZMaxAngel * Mathf.Deg2Rad / 2.0f;
        //        //Debug.Log("currentZAngle  = " + mFingerBoneType+ "currentZAngle =" + currentZAngle);
        //        float valueAngle = 0.0f;
        //        if (Angel < 0)
        //        {
        //            valueAngle = (zMinAngel > (currentYAngle + Angel)) ? zMinAngel : (currentYAngle + Angel);
        //            if (valueAngle == zMinAngel)
        //            {
        //                parentAngle = currentYAngle + Angel - zMinAngel;
        //                isParentChangAngel = true;
        //            }
        //            else
        //            {
        //                parentAngle = 0.0f;
        //                isParentChangAngel = false;
        //            }
        //        }
        //        else
        //        {
        //            valueAngle = (zMaxAngel < (currentYAngle + Angel)) ? zMaxAngel : (currentYAngle + Angel);
        //            if (valueAngle == zMaxAngel)
        //            {
        //                parentAngle = currentYAngle + Angel - zMaxAngel;
        //                isParentChangAngel = true;
        //            }
        //            else
        //            {
        //                parentAngle = 0.0f;
        //                isParentChangAngel = false;
        //            }
        //        }
        //        //if(mFingerBoneType == Hi5_Glove_Finger_bone_Type.)
        //        parent.Rotate(new Vector3(0.0f, valueAngle - currentYAngle, 0.0f), Space.Self);
        //        //isRotationZ = false;
        //        return isParentChangAngel;
        //    }
        //}

        


    }
}

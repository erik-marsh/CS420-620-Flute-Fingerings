using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public class Hi5_Hand_Visible_Thumb_Finger : Hi5_Hand_Visible_Finger
    {
        bool isCaulate = false;
        private Hi5_Record mRecord = new Hi5_Record();
        public float zMaxTotalAngel  = 0.0f;
        public float zMinTotalAngel = 0.0f;
        public float yMaxTotalAngel = 0.0f;
        public float yMinTotalAngel = 0.0f;

        public float m_OneBoneYMaxAngle;
        public float m_TwoBoneYMaxAngle;
        public float m_OneBoneYMinAngle;
        public float m_TwoBoneYMinAngle;
        internal override void AddCoiilder()
        {
            coliderFingers.Clear();
            Hi5_Hand_Collider_Visible_Thumb_Finger[] tempCOllider = gameObject.GetComponentsInChildren<Hi5_Hand_Collider_Visible_Thumb_Finger>();
            for (int i = 0; i < tempCOllider.Length; i++)
            {
                coliderFingers.Add(tempCOllider[i]);
            }
        }
       
        protected override void AddBoneCollider(Transform bone, int index)
        {
            if (m_finger_type == Hi5_Glove_Interaction_Finger_Type.EThumb && index == 1)
                return;
            Transform temp = null;
            if (index != 4)
            {
                //Debug.Log("index="+ index);
                temp = bone.GetChild(1);
            }
            else
                temp = bone.GetChild(0);
            if (temp != null)
            {
                if (temp.GetComponent<Hi5_Hand_Collider_Visible_Finger>() != null)
                {
                    mBoneCollider.Add((Hi5_Glove_Finger_bone_Type)index, temp.GetComponent<Hi5_Hand_Collider_Visible_Finger>());
                    if ((Hi5_Glove_Finger_bone_Type)index == Hi5_Glove_Finger_bone_Type.EOne)
                    {
                        temp.GetComponent<Hi5_Hand_Collider_Visible_Finger>().setFinger(mHand, this, (Hi5_Glove_Finger_bone_Type)index, m_OneBoneMaxAngle, m_OneBoneMinAngle);
                        temp.GetComponent<Hi5_Hand_Collider_Visible_Thumb_Finger>().SetYAngle(m_OneBoneYMaxAngle, m_OneBoneYMinAngle);

                    }
                    else if ((Hi5_Glove_Finger_bone_Type)index == Hi5_Glove_Finger_bone_Type.ETwo)
                    {
                        temp.GetComponent<Hi5_Hand_Collider_Visible_Finger>().setFinger(mHand, this, (Hi5_Glove_Finger_bone_Type)index, m_TwoBoneMaxAngle, m_TwoBoneMinAngle);
                        temp.GetComponent<Hi5_Hand_Collider_Visible_Thumb_Finger>().SetYAngle(m_TwoBoneYMaxAngle, m_TwoBoneYMinAngle);
                    }
                    else if ((Hi5_Glove_Finger_bone_Type)index == Hi5_Glove_Finger_bone_Type.EThree)
                    {
                        temp.GetComponent<Hi5_Hand_Collider_Visible_Finger>().setFinger(mHand, this, (Hi5_Glove_Finger_bone_Type)index, m_ThreeBoneMaxAngle, m_ThreeBoneMinAngle);
                    }

                    else if ((Hi5_Glove_Finger_bone_Type)index == Hi5_Glove_Finger_bone_Type.EFour)
                    {
                        temp.GetComponent<Hi5_Hand_Collider_Visible_Finger>().setFinger(mHand, this, (Hi5_Glove_Finger_bone_Type)index, 0.0f, 0.0f);
                    }

                }
            }
        }

        override protected void FixedUpdate()
        {
            if (m_dicCollision.Count == 0)
                return;
            if (isCaulate)
                return;
            isCaulate = true;
            int index = 0;
            Hi5_Glove_Finger_bone_Type minKey = Hi5_Glove_Finger_bone_Type.EThree;
            foreach (Hi5_Glove_Finger_bone_Type key in m_dicCollision.Keys)
            {
                if (index == 0)
                    minKey = key;
                else
                {
                    if (minKey < key)
                        minKey = key;
                }
                index++;
            }

            if (m_dicCollision.ContainsKey(minKey))
            {
				bool isCalculate = m_dicCollision[minKey].collider_finger.CalculateRotation(m_dicCollision[minKey].constactPoint);
            }
        }


        internal protected override void ParentRotationZ(float angle, Hi5_Glove_Finger_bone_Type boneType)
        {
            //mChildNodes[boneType].transform.localRotation.eulerAngles.z;
        }

        internal protected override void ParentRotationY(float angle, Hi5_Glove_Finger_bone_Type boneType)
        {
            //if (m_finger_type != Hi5_Glove_Interaction_Finger_Type.EThumb)
            {
                //if (((int)boneType - 1) > 0)
                //{
                //    Hi5_Glove_Finger_bone_Type parentBone = (Hi5_Glove_Finger_bone_Type)((int)boneType - 1);
                //    //Debug.Log("ParentRotationZ type = " + parentBone);
                //    if (mBoneCollider.ContainsKey(parentBone))
                //    {
                //        float parentAngle;
                //        Quaternion angleQuatation = new Quaternion(0.0f, angle,0.0f ,1.0f);
                //        bool isParentRotationY = mBoneCollider[parentBone].ChangeFingerAngel(angleQuatation.y, out parentAngle);
                //        if (isParentRotationY)
                //            ParentRotationY(parentAngle, (Hi5_Glove_Finger_bone_Type)((int)boneType - 1));
                //        //ChildRotaitionZ (angle,boneType);
                //    }
                //}
                //else
                //{

                //}
            }
        }

        //判断大拇指运动轨迹是否向手掌
        internal bool IsMoveTowardHand()
        {
            Hi5_Position_Record[] records = mRecord.GetRecord().ToArray();
            if (records.Length == Hi5_Interaction_Const.ObjectPinchRecordPositionCount)
            {
                Hi5_Position_Record pre = records[Hi5_Interaction_Const.ObjectPinchRecordPositionCount - 3];
                Hi5_Position_Record back = records[Hi5_Interaction_Const.ObjectPinchRecordPositionCount - 1];
                Vector3 temp = back.position - pre.position;
                //Hi5_Hand_Collider_Visible_Finger collider = GetChildCollider(Hi5_Glove_Finger_bone_Type.EFour);
                //Vector3 planeNormal = (-collider.transform.parent.forward).normalized;
                //Vector3 temp1 = planeNormal * (Vector3.Dot(planeNormal, temp));
                Vector3 temp2 = mHand.palm.transform.position - transform.position;

                float dotValue = Vector3.Dot(temp, temp2);
                if(dotValue>0.0f)
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                return true;
            }
        }

         protected void Update()
        {
           
            mRecord.RecordPosition(Time.deltaTime, transform);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    //public enum Hi5_Glove_Interaction_Finger_Type
    //{
    //    EThumb = 0,
    //    EIndex,
    //    EMiddle,
    //    ERing,
    //    EPinky
    //}

    public enum Hi5_Glove_Finger_bone_Type
    {
        EOne = 1,
        ETwo,
        EThree,
        EFour,
        EMax
    }
    public class Hi5_Hand_Visible_Finger : MonoBehaviour
    {
        public Hi5_Glove_Interaction_Finger_Type m_finger_type = Hi5_Glove_Interaction_Finger_Type.EThumb;
        #region Angle
        public float m_ThreeBoneMaxAngle;
        public float m_TwoBoneMaxAngle;
        public float m_OneBoneMaxAngle;
        public float m_ThreeBoneMinAngle;
        public float m_TwoBoneMinAngle;
        public float m_OneBoneMinAngle;
        internal Hi5_Hand_Visible_Hand mHand;
        #endregion
        #region nodes Bone_Collider
        protected Dictionary<Hi5_Glove_Finger_bone_Type, Hi5_Hand_Collider_Visible_Finger> mBoneCollider = new Dictionary<Hi5_Glove_Finger_bone_Type, Hi5_Hand_Collider_Visible_Finger>();
        protected Dictionary<Hi5_Glove_Finger_bone_Type, Transform> mChildNodes = new Dictionary<Hi5_Glove_Finger_bone_Type, Transform>();
        internal protected Dictionary<Hi5_Glove_Finger_bone_Type, bool> mChildRotationLock = new Dictionary<Hi5_Glove_Finger_bone_Type, bool>();
        //internal bool isFollow = true;

        internal List<Hi5_Hand_Collider_Visible_Finger> coliderFingers = new List<Hi5_Hand_Collider_Visible_Finger>();
        private void Awake()
        {
            AddCoiilder();
        }

        internal void OpenCollider(bool isOpen)
        {
            foreach (Hi5_Hand_Collider_Visible_Finger item in coliderFingers)
            {
                if (isOpen)
                    item.gameObject.SetActive(true);
                else
                    item.gameObject.SetActive(false);
            }
        }
        internal virtual void AddCoiilder()
        {
            coliderFingers.Clear();
            Hi5_Hand_Collider_Visible_Finger[] tempCOllider = gameObject.GetComponentsInChildren<Hi5_Hand_Collider_Visible_Finger>();
            for (int i = 0; i < tempCOllider.Length; i++)
            {
                coliderFingers.Add(tempCOllider[i]);
            }
        }
        internal void AddChildNode(List<Transform> childs, Hi5_Hand_Visible_Hand hand)
        {
            mHand = hand;
            mChildNodes.Clear();
            int index = 1;
            for (int i = 0; i < childs.Count; i++)
            {
                mChildNodes.Add((Hi5_Glove_Finger_bone_Type)index, childs[i]);
                mChildRotationLock.Add((Hi5_Glove_Finger_bone_Type)index, false);
                AddBoneCollider(childs[i], index);
                index++;
            }
        }


        internal Transform GetNode(Hi5_Glove_Finger_bone_Type param)
        {
           return mChildNodes[param];
        }

        protected virtual void AddBoneCollider(Transform bone, int index)
        {
            if (m_finger_type == Hi5_Glove_Interaction_Finger_Type.EThumb && index == 1)
                return;
            Transform temp = null;
            if (index != 4)
            {
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
                        temp.GetComponent<Hi5_Hand_Collider_Visible_Finger>().setFinger(mHand, this, (Hi5_Glove_Finger_bone_Type)index, m_OneBoneMaxAngle, m_OneBoneMinAngle);
                    else if ((Hi5_Glove_Finger_bone_Type)index == Hi5_Glove_Finger_bone_Type.ETwo)
                        temp.GetComponent<Hi5_Hand_Collider_Visible_Finger>().setFinger(mHand, this, (Hi5_Glove_Finger_bone_Type)index, m_TwoBoneMaxAngle, m_TwoBoneMinAngle);
                    else if ((Hi5_Glove_Finger_bone_Type)index == Hi5_Glove_Finger_bone_Type.EThree)
                        temp.GetComponent<Hi5_Hand_Collider_Visible_Finger>().setFinger(mHand, this, (Hi5_Glove_Finger_bone_Type)index, m_ThreeBoneMaxAngle, m_ThreeBoneMinAngle);
                    else if ((Hi5_Glove_Finger_bone_Type)index == Hi5_Glove_Finger_bone_Type.EFour)
                        temp.GetComponent<Hi5_Hand_Collider_Visible_Finger>().setFinger(mHand, this, (Hi5_Glove_Finger_bone_Type)index, 0.0f, 0.0f);
                }
            }
        }
        #endregion
        protected Dictionary<Hi5_Glove_Finger_bone_Type, Hi5_Hand_CollisionData> m_dicCollision = new Dictionary<Hi5_Glove_Finger_bone_Type, Hi5_Hand_CollisionData>();
        public void AddDicCollision(Hi5_Glove_Finger_bone_Type key, Hi5_Hand_CollisionData data)
        {
            if (m_dicCollision.ContainsKey(key))
            {
                m_dicCollision[key] = data;
            }
            else
                m_dicCollision.Add(key, data);
        }

        #region unity system
        private void Update()
        {
            //float angle = mChildNodes[Hi5_Glove_Finger_bone_Type.EThree].transform.localRotation.eulerAngles.z;
            //Debug.Log("angle"+ angle +"     "+ angle);
        }

        int count = 0;
        virtual protected void FixedUpdate()
        {

            Hi5_Glove_Finger_bone_Type minKey = Hi5_Glove_Finger_bone_Type.EThree;
            if (m_dicCollision.Count == 0)
                return ;
            int index = 0;
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

				if (!isCalculate)
                {
                    if (minKey == Hi5_Glove_Finger_bone_Type.EThree)
                    {
                        if (m_dicCollision.ContainsKey(Hi5_Glove_Finger_bone_Type.ETwo))
                        {
							isCalculate = m_dicCollision[Hi5_Glove_Finger_bone_Type.ETwo].collider_finger.CalculateRotation(m_dicCollision[Hi5_Glove_Finger_bone_Type.ETwo].constactPoint);
							if (!isCalculate)
                            {
                                if (m_dicCollision.ContainsKey(Hi5_Glove_Finger_bone_Type.EOne))
                                {
									isCalculate = m_dicCollision[Hi5_Glove_Finger_bone_Type.EOne].collider_finger.CalculateRotation(m_dicCollision[Hi5_Glove_Finger_bone_Type.EOne].constactPoint);

                                }
                            }
                           
                        }
                        else
                        {
                            if (m_dicCollision.ContainsKey(Hi5_Glove_Finger_bone_Type.EOne))
                            {
								isCalculate = m_dicCollision[Hi5_Glove_Finger_bone_Type.EOne].collider_finger.CalculateRotation(m_dicCollision[Hi5_Glove_Finger_bone_Type.EOne].constactPoint);
                            }

                        }
                    }
                    else if (minKey == Hi5_Glove_Finger_bone_Type.ETwo)
                    {
                        if (m_dicCollision.ContainsKey(Hi5_Glove_Finger_bone_Type.ETwo))
                        {
							isCalculate = m_dicCollision[Hi5_Glove_Finger_bone_Type.ETwo].collider_finger.CalculateRotation(m_dicCollision[Hi5_Glove_Finger_bone_Type.ETwo].constactPoint);
							if (!isCalculate)
                            {
                                if (m_dicCollision.ContainsKey(Hi5_Glove_Finger_bone_Type.EOne))
                                {
									isCalculate = m_dicCollision[Hi5_Glove_Finger_bone_Type.EOne].collider_finger.CalculateRotation(m_dicCollision[Hi5_Glove_Finger_bone_Type.EOne].constactPoint);
									
                                }
                               
                            }
                           
                        }
                    }
                }
            }
            m_dicCollision.Clear();
        }
        /*private void FixedUpdate()
        {

            Hi5_Glove_Finger_bone_Type minKey = Hi5_Glove_Finger_bone_Type.EThree;
            if (m_dicCollision.Count == 0)
                return;
            //if (count != 0)
            //    return;
            //count++;
            int index = 0;
            foreach (Hi5_Glove_Finger_bone_Type key in m_dicCollision.Keys)
            {
                if (index == 0)
                    minKey = key;
                else
                {
                    if (minKey > key)
                        minKey = key;
                }
                if (key == Hi5_Glove_Finger_bone_Type.EOne)
                    Debug.Log("1");
                else if (key == Hi5_Glove_Finger_bone_Type.ETwo)
                    Debug.Log("2");
                else if (key == Hi5_Glove_Finger_bone_Type.EThree)
                    Debug.Log("3");
                index++;
            }

            if (m_dicCollision.ContainsKey(minKey))
            {
                bool isCaculate = m_dicCollision[minKey].collider_finger.caculateRotation(m_dicCollision[minKey].constactPoint);
                if (!isCaculate)
                {
                    if (minKey == Hi5_Glove_Finger_bone_Type.EOne)
                    {
                        if (m_dicCollision.ContainsKey(Hi5_Glove_Finger_bone_Type.ETwo))
                        {
                            isCaculate = m_dicCollision[Hi5_Glove_Finger_bone_Type.ETwo].collider_finger.caculateRotation(m_dicCollision[Hi5_Glove_Finger_bone_Type.ETwo].constactPoint);
                            if (!isCaculate)
                            {
                                if (m_dicCollision.ContainsKey(Hi5_Glove_Finger_bone_Type.EThree))
                                {
                                    isCaculate = m_dicCollision[Hi5_Glove_Finger_bone_Type.EThree].collider_finger.caculateRotation(m_dicCollision[Hi5_Glove_Finger_bone_Type.EThree].constactPoint);
                                    if (isCaculate)
                                    {
                                        Debug.Log("Three");
                                    }
                                }
                                //else
                                //{
                                //    Debug.Log("Two");
                                //}
                            }
                            else
                            {
                                Debug.Log("Two");
                            }
                        }
                        else
                        {
                            if (m_dicCollision.ContainsKey(Hi5_Glove_Finger_bone_Type.EThree))
                            {
                                isCaculate = m_dicCollision[Hi5_Glove_Finger_bone_Type.EThree].collider_finger.caculateRotation(m_dicCollision[Hi5_Glove_Finger_bone_Type.EThree].constactPoint);
                                if (isCaculate)
                                {
                                    Debug.Log("Three");
                                }
                            }

                        }
                    }
                    else if (minKey == Hi5_Glove_Finger_bone_Type.ETwo)
                    {
                        if (m_dicCollision.ContainsKey(Hi5_Glove_Finger_bone_Type.ETwo))
                        {
                            isCaculate = m_dicCollision[Hi5_Glove_Finger_bone_Type.ETwo].collider_finger.caculateRotation(m_dicCollision[Hi5_Glove_Finger_bone_Type.ETwo].constactPoint);
                            if (!isCaculate)
                            {
                                if (m_dicCollision.ContainsKey(Hi5_Glove_Finger_bone_Type.EThree))
                                {
                                    isCaculate = m_dicCollision[Hi5_Glove_Finger_bone_Type.EThree].collider_finger.caculateRotation(m_dicCollision[Hi5_Glove_Finger_bone_Type.EThree].constactPoint);
                                    if (isCaculate)
                                    {
                                        Debug.Log("Three");
                                    }
                                }
                                else
                                {
                                    Debug.Log("Two");
                                }
                            }
                            else
                            {
                                Debug.Log("Two");
                            }
                        }
                    }
                }
            }
            m_dicCollision.Clear();
        }*/
        #endregion
        protected internal virtual void ParentRotationY(float angle, Hi5_Glove_Finger_bone_Type boneType)
        {
        }
        //protected internal virtual void ParentRotationZ(float angle, Hi5_Glove_Finger_bone_Type boneType)
        //{
        //    //if (m_finger_type != Hi5_Glove_Interaction_Finger_Type.EThumb)
        //    {
        //        if (((int)boneType - 1) > 0)
        //        {
        //            Hi5_Glove_Finger_bone_Type parentBone = (Hi5_Glove_Finger_bone_Type)((int)boneType - 1);
        //            //Debug.Log("ParentRotationZ type = " + parentBone);
        //            if (mBoneCollider.ContainsKey(parentBone))
        //            {
        //                float parentAngle;
        //                //Quaternion angleQuatation = Quaternion.Euler(0.0f, 0.0f, angle);
        //                Quaternion angleQuatation = new Quaternion(0.0f, 0.0f, angle, 1.0f);
        //                bool isParentRotationZ = mBoneCollider[parentBone].ChangeFingerAngel(angleQuatation.z, out parentAngle);
        //                if (isParentRotationZ)
        //                    ParentRotationZ(parentAngle, (Hi5_Glove_Finger_bone_Type)((int)boneType - 1));
        //                //ChildRotaitionZ (angle,boneType);
        //            }
        //        }
        //        else
        //        {

        //        }
        //    }
        //}


        protected internal virtual void ParentRotationZFixed(float angle, Hi5_Glove_Finger_bone_Type boneType)
        {
            Hi5_Glove_Finger_bone_Type parentBone = (Hi5_Glove_Finger_bone_Type)((int)boneType);
            if (mBoneCollider.ContainsKey(parentBone))
            {
                mBoneCollider[parentBone].ChangeFingerAngelFixed(angle);
            }
        }

        protected internal virtual void ParentRotationZ(float angle, Hi5_Glove_Finger_bone_Type boneType)
        {
            Hi5_Glove_Finger_bone_Type parentBone = (Hi5_Glove_Finger_bone_Type)((int)boneType);
            if (mBoneCollider.ContainsKey(parentBone))
            {
                mBoneCollider[parentBone].ChangeFingerAngel(angle);
            }
        }
        internal protected virtual void ChildRotaitionY(float angle, Hi5_Glove_Finger_bone_Type boneType)
        {

        }

    
        internal protected void TailFingerCollider(Collision collision)
        {

        }

        internal Transform GetBrother(Hi5_Glove_Finger_bone_Type boneType)
        {
            if (((int)boneType + 1) < (int)Hi5_Glove_Finger_bone_Type.EMax)
            {
                Hi5_Glove_Finger_bone_Type parentBone = (Hi5_Glove_Finger_bone_Type)((int)boneType + 1);
                if (mBoneCollider.ContainsKey(parentBone))
                {
                    return mBoneCollider[parentBone].transform.parent;
                }
                else
                {
                    //Debug.Log("m_finger_type =" + (int)m_finger_type);
                }
            }
            return null;
        }

        internal List<Transform> GetChildNodes(Hi5_Glove_Finger_bone_Type boneType)
        {
            List<Transform> backValue = new List<Transform>();
            while (((int)boneType + 1) < (int)Hi5_Glove_Finger_bone_Type.EMax)
            {
                Hi5_Glove_Finger_bone_Type parentBone = (Hi5_Glove_Finger_bone_Type)((int)boneType + 1);
                if (mBoneCollider.ContainsKey(parentBone))
                {
                    backValue.Add(mBoneCollider[parentBone].transform.parent);
                }
                boneType = boneType + 1;
            }
            return backValue;
        }
        internal Hi5_Hand_Collider_Visible_Finger GetChildCollider(Hi5_Glove_Finger_bone_Type boneType)
        {
            if (mBoneCollider.ContainsKey(boneType))
            {
                return mBoneCollider[boneType];
            }
            return null;
        }


    
    internal void ApplyFingerRotation(List<Transform> bones)
        {
            if (Hi5_Interaction_Const.IsUseVisibleHand)
            {
                if (isLockFingerRotation())
                    return;
            }

            for (int i = 0; i < bones.Count; i++)
            {
                Hi5_Glove_Finger_bone_Type boneType = (Hi5_Glove_Finger_bone_Type)(i + 1);
                if (m_finger_type == Hi5_Glove_Interaction_Finger_Type.EThumb)
                {
                    mChildNodes[boneType].localRotation = bones[i].localRotation;
                }
                else
                {
                    Hi5_Glove_Finger_bone_Type boneTypeTemp = boneType - 1;
                    if (boneTypeTemp > 0)
                    {
                        mChildNodes[boneTypeTemp].localRotation = bones[i].localRotation;
                    }
                }
            }
        }

        internal bool isLockFingerRotation()
        {
            //ruige redo 添加参数Id
            if (mHand.GetGloveHand().IsTriggerObjectById(m_finger_type))
            {
                return true;
            }
            else
            {
                foreach (KeyValuePair<Hi5_Glove_Finger_bone_Type, Hi5_Hand_Collider_Visible_Finger> item in mBoneCollider)
                {
                    item.Value.Reset();
                }
                return false;
            }
        }

        internal void RealeaseFingerFollow()
        {
           
            //Debug.Log("lock release");

            //if (mBoneCollider.ContainsKey(Hi5_Glove_Finger_bone_Type.EOne))
            //    mBoneCollider[Hi5_Glove_Finger_bone_Type.EOne].saveTime = Hi5_Interaction_Const.FingerCollisionSaveTime;
            //if (mBoneCollider.ContainsKey(Hi5_Glove_Finger_bone_Type.ETwo))
            //    mBoneCollider[Hi5_Glove_Finger_bone_Type.ETwo].saveTime = Hi5_Interaction_Const.FingerCollisionSaveTime;
            //if (mBoneCollider.ContainsKey(Hi5_Glove_Finger_bone_Type.EThree))
            //    mBoneCollider[Hi5_Glove_Finger_bone_Type.EThree].saveTime = Hi5_Interaction_Const.FingerCollisionSaveTime;
            //if (mBoneCollider.ContainsKey(Hi5_Glove_Finger_bone_Type.EFour))
            //    mBoneCollider[Hi5_Glove_Finger_bone_Type.EFour].saveTime = Hi5_Interaction_Const.FingerCollisionSaveTime;
            //if (mChildRotationLock.ContainsKey(Hi5_Glove_Finger_bone_Type.EOne))
            //{
            //    if (mBoneCollider.ContainsKey(Hi5_Glove_Finger_bone_Type.EOne))
            //        mBoneCollider[Hi5_Glove_Finger_bone_Type.EOne].saveTime = Hi5_Interaction_Const.FingerCollisionSaveTime;
            //    mChildRotationLock[Hi5_Glove_Finger_bone_Type.EOne] = false;
            //}
            //else if (mChildRotationLock.ContainsKey(Hi5_Glove_Finger_bone_Type.ETwo))
            //{
            //    mBoneCollider[Hi5_Glove_Finger_bone_Type.ETwo].saveTime = Hi5_Interaction_Const.FingerCollisionSaveTime;
            //    mChildRotationLock[Hi5_Glove_Finger_bone_Type.ETwo] = false;
            //}
            //else if (mChildRotationLock.ContainsKey(Hi5_Glove_Finger_bone_Type.EThree))
            //{
            //    mBoneCollider[Hi5_Glove_Finger_bone_Type.EThree].saveTime = Hi5_Interaction_Const.FingerCollisionSaveTime;
            //    mChildRotationLock[Hi5_Glove_Finger_bone_Type.EThree] = false;
            //}
            //else if (mChildRotationLock.ContainsKey(Hi5_Glove_Finger_bone_Type.EFour))
            //{
            //    mBoneCollider[Hi5_Glove_Finger_bone_Type.EFour].saveTime = Hi5_Interaction_Const.FingerCollisionSaveTime;
            //    mChildRotationLock[Hi5_Glove_Finger_bone_Type.EFour] = false;
            //}

        }
    }
}

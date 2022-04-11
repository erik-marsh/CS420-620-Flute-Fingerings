using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public class Hi5_Glove_Collider_Finger : Hi5_Glove_Interaction_Collider
    {
        public int mBoneIndex = 0;
        internal Hi5_Glove_Interaction_Finger_Type mFingerType;
        internal protected Hi5_Glove_Interaction_Finger mFinger;
        //统计Trigger次数
        internal Dictionary<int, int> m_TriggerCount = new Dictionary<int, int>();
        private void Awake()
        {
            base.Awake();
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.004f, transform.localPosition.z);
        }

        internal bool IsIndexColliderThumb()
        {
            if (mFingerType == Hi5_Glove_Interaction_Finger_Type.EThumb)
            {
                bool isTrigger = false;
                foreach (Collider item in m_Triggers)
                {
                    if (item.gameObject.GetComponent<Hi5_Glove_Collider_Tail_Finger>() != null
                        && item.gameObject.transform.parent.parent.parent.parent.parent.GetComponent<Hi5_Glove_Interaction_Finger>() != null
                        && item.gameObject.transform.parent.parent.parent.parent.parent.GetComponent<Hi5_Glove_Interaction_Finger>().mHand.m_IsLeftHand == mFinger.mHand.m_IsLeftHand     
                        && item.gameObject.transform.parent.parent.parent.parent.parent.GetComponent<Hi5_Glove_Interaction_Finger>().m_finger_type == Hi5_Glove_Interaction_Finger_Type.EIndex)
                    {
                        isTrigger = true;
                    }
                }
                return isTrigger;
            }
            else
                return false;
        }
        override protected void OnEnable()
        {
            base.OnEnable();
            m_TriggerCount.Clear();
        }
        override protected void OnTriggerEnter(Collider other)
        {
            if (mBoneIndex == 4 && mFinger != null)
            {
				if (other.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null || other.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
                {
					
					int objectId = -1000;
					if(other.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
						objectId = other.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
					else if(other.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
						objectId = other.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
                    mFinger.TailFingerCollider(objectId);
                }
            }
            //Debug.Log("OnTriggerEnter" + other.name);
            base.OnTriggerEnter(other);
			if (other.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null || other.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
            {
				int objectId = -1000;
				if(other.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
					objectId = other.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
				else if(other.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
					objectId = other.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
			
                if (m_TriggerCount.ContainsKey(objectId))
                {
                    m_TriggerCount[objectId]++;
                }
                else
                {
                    m_TriggerCount.Add(objectId, 1);
                }
            }

            // 

        }

        override protected void OnTriggerStay(Collider other)
        {

        }

        override protected void OnTriggerExit(Collider other)
        {
           
            //Debug.Log("OnTriggerExit" + other.name);
            base.OnTriggerExit(other);
			if (other.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null && other.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
            {
				int objectId = -1000;
				if(other.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
					objectId = other.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
				else if(other.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
					objectId = other.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
			
                if (m_TriggerCount.ContainsKey(objectId))
                {
                    if (m_TriggerCount[objectId] > 1)
                        m_TriggerCount[objectId]--;
                    else
                        m_TriggerCount.Remove(objectId);
                }
            }

        
           
        }


        public bool IsTriggerObjectById(int ObjectId)
        {
            bool isTigger = IsTriggerObject(ObjectId);
            return isTigger;

            /*
            foreach (Collider item in colliders)
            {
                if (item.gameObject.GetComponent<Hi5_Glove_Interraction_Item>() != null)
                {
                    string name = item.gameObject.name;
                    Debug.Log("TriggerObject   " + name + "ObjectId" + ObjectId);
                    //if (item.gameObject.GetComponent<Hi5_Glove_Interraction_Item>().idObject == ObjectId)
                    //{
                        isTigger = true;
                        break;
                    //}
                }
                else
                {
                    //string name = item.gameObject.name;
                    Debug.Log("TriggerObjectFalse   "+name);
                }
              
            }
            return isTigger;*/
        }
        internal bool IsTriggerObject(int objectId)
        {
            if (m_TriggerCount.ContainsKey(objectId))
                return true;
            else
                return false;
        }


		bool isAttributeSet = false;

        internal protected void SetAttribute(int boneIndex,
                                            Hi5_Glove_Interaction_Finger_Type fingerType,
                                            Hi5_Glove_Interaction_Finger finger)
        {
           mBoneIndex = boneIndex;
           mFingerType = fingerType;
            mFinger = finger;
			isAttributeSet = true;
        }

		bool isSetLayer = false;
		void Update()
		{
			//if (Hi5_Layer_Set.IsResetLayer && isAttributeSet && !isSetLayer) {
			//	if (mBoneIndex == 4) {
			//		gameObject.layer = LayerMask.NameToLayer ("Hi5OtherFingerTail");
			//	}
			//	else 
			//		gameObject.layer = LayerMask.NameToLayer ("Hi5OtherFingerOther");
			//	isSetLayer = true;
			//}
		}
    }
}

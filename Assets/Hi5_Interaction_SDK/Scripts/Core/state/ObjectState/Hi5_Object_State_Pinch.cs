using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public enum Hi5_Object_Pinch_Type
    {
        ENone = 0,
        ELeft = 1,
        ERight = 2,
        EDouble
    }
    public class Hi5_Object_State_Pinch : Hi5_Object_State_Base
    {
        Hi5_Glove_Interaction_Hand m_left_hand = null;
        Hi5_Glove_Interaction_Hand m_right_hand = null;
        internal bool isTestRelease = false;
        internal Hi5_Object_Pinch_Type mPinchType = Hi5_Object_Pinch_Type.ENone;
        // Queue<Hi5_Position_Record> mQueuePositionRecord = new Queue<Hi5_Position_Record>();
		Quaternion localQua;
        override public void Start()
        {
			localQua = ObjectItem.transform.localRotation;
           // mPinchType = Hi5_Object_Pinch_Type.ELeft;
			ObjectItem.CleanRecord();
            if (mObjectItem.gameObject.GetComponent<Rigidbody>() != null)
            {
                mObjectItem.gameObject.GetComponent<Rigidbody>().Sleep();

#if UNITY_2017_2_OR_NEWER

#else
                   mObjectItem.transform.gameObject.GetComponent<Rigidbody>().WakeUp();
#endif
            }

            //mObjectItem.ChangeColor (Color.cyan);
            //if (Hi5_Interaction_Const.TestModifyConstraints)
            ObjectItem.SetIsKinematic(true);
        }


        internal void SetPinchHand(Hi5_Object_Pinch_Type handType, Hi5_Glove_Interaction_Hand hand)
        {
            if (mPinchType == Hi5_Object_Pinch_Type.ENone)
                mPinchType = handType;
            else if (mPinchType == Hi5_Object_Pinch_Type.EDouble)
            {

            }
            else if (mPinchType == handType)
            {

            }
            else
            {
                mPinchType = Hi5_Object_Pinch_Type.EDouble;
            }
            if (handType == Hi5_Object_Pinch_Type.ELeft)
                m_left_hand = hand;
            else if (handType == Hi5_Object_Pinch_Type.ERight)
                m_right_hand = hand;
        }

		internal bool CancelPinchHand(Hi5_Object_Pinch_Type handType,out bool OtherIsRelease)
        {
			if (mPinchType == Hi5_Object_Pinch_Type.EDouble)
			{
				if (handType == Hi5_Object_Pinch_Type.ELeft) 
				{
					mPinchType = Hi5_Object_Pinch_Type.ERight;
					if (m_right_hand != null) 
					{
						List<int> handList;
						bool IsOtherHandTrigger = false;
						if (m_right_hand.mHandCollider.IsPinch (out handList)) {
							if (handList != null) {
								foreach (int item in handList) {
									if (item == ObjectItem.idObject) {
										IsOtherHandTrigger = true;
										break;
									}
								}
							}	
						}
						if (IsOtherHandTrigger) {
							ObjectItem.transform.parent = m_right_hand.mVisibleHand.palm.transform;
							m_left_hand = null;
							OtherIsRelease = false;
							return false;
						} else {
							ObjectItem.transform.parent = Hi5_Interaction_Object_Manager.GetObjectManager ().transform;
							m_left_hand = null;
							OtherIsRelease = true;
							m_right_hand = null;
							mPinchType = Hi5_Object_Pinch_Type.ENone;
							return true;
						}
					} else {
						ObjectItem.transform.parent = Hi5_Interaction_Object_Manager.GetObjectManager ().transform;
						m_left_hand = null;
						OtherIsRelease = true;
						m_right_hand = null;
						mPinchType = Hi5_Object_Pinch_Type.ENone;
						return true;
					}
				}
                else if (handType == Hi5_Object_Pinch_Type.ERight) {
					mPinchType = Hi5_Object_Pinch_Type.ELeft;
					if (m_left_hand != null) {
						List<int> handList;
						bool IsOtherHandTrigger = false;
						if (m_left_hand.mHandCollider.IsPinch (out handList)) {
							if (handList != null) {
								foreach (int item in handList) {
									if (item == ObjectItem.idObject) {
										IsOtherHandTrigger = true;
										break;
									}
								}
							}	
						}
						if (IsOtherHandTrigger) {
							ObjectItem.transform.parent = m_left_hand.mVisibleHand.palm.transform;
							m_right_hand = null;
							OtherIsRelease = false;
							return false;
						} else {
							ObjectItem.transform.parent = Hi5_Interaction_Object_Manager.GetObjectManager ().transform;
							m_right_hand = null;
							OtherIsRelease = true;
							m_left_hand = null;
							mPinchType = Hi5_Object_Pinch_Type.ENone;
							return true;
						}

					}
                    else
                    {
						ObjectItem.transform.parent = Hi5_Interaction_Object_Manager.GetObjectManager ().transform;
						m_right_hand = null;
						OtherIsRelease = true;
						m_left_hand = null;
						mPinchType = Hi5_Object_Pinch_Type.ENone;
						return true;
					}
				}
                else
                {
                    ObjectItem.transform.parent = Hi5_Interaction_Object_Manager.GetObjectManager().transform;
                    m_right_hand = null;
                    OtherIsRelease = true;
                    m_left_hand = null;
                    mPinchType = Hi5_Object_Pinch_Type.ENone;
                    return true;
                }
            }
            else if (mPinchType == handType)
            {
				mPinchType = Hi5_Object_Pinch_Type.ENone;
                if (handType == Hi5_Object_Pinch_Type.ELeft)
                {
                    ObjectItem.transform.parent = Hi5_Interaction_Object_Manager.GetObjectManager().transform;
                    OtherIsRelease = false;
                    m_left_hand = null;
                }
                else if (handType == Hi5_Object_Pinch_Type.ERight)
                {
                    ObjectItem.transform.parent = Hi5_Interaction_Object_Manager.GetObjectManager().transform;
                    OtherIsRelease = false;
                    m_right_hand = null;

                }
                else
                {
                    ObjectItem.transform.parent = Hi5_Interaction_Object_Manager.GetObjectManager().transform;
                    OtherIsRelease = false;
                }
				return true;
			}
			else
			{
				ObjectItem.transform.parent = Hi5_Interaction_Object_Manager.GetObjectManager ().transform;
				m_right_hand = null;
				OtherIsRelease = true;
				m_left_hand = null;
				mPinchType = Hi5_Object_Pinch_Type.ENone;
				return true;
			}
           
        }

        override public void Update(float deltaTime)
        {
            //if (Hi5_Interaction_Const.TestPlaneMoveUsePhysic)
            {
				//ObjectItem.transform.localRotation = localQua;
                //if(Hi5_Interaction_Const.TestModifyConstraints)
                    ObjectItem.SetIsKinematic(true);
				//ObjectItem.SetAllLock ();
            }
        }

        override public void End()
        {
            //Debug.Log("pinch end");
            if (ObjectItem != null)
            {
               // if (Hi5_Interaction_Const.TestModifyConstraints)
                    ObjectItem.SetIsKinematic(false);
                //mObjectItem.ChangeColor (mObjectItem.orgColor);
                //ObjectItem.CacullateThrowMove();
                //mQueuePositionRecord.Clear();
            }
        }

        override public void FixedUpdate(float deltaTime)
        {
            
        }
    }
}

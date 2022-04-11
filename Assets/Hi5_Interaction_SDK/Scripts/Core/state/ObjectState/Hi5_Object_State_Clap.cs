using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public class Hi5_Object_State_Clap : Hi5_Object_State_Base
    {
        public enum Hi5_Object_Clap_Type
        {
            ENone = 0,
            ELeft = 1,
            ERight = 2,
            EDouble
        }
        internal Hi5_Glove_Interaction_Finger_Type fingerTypeOne = Hi5_Glove_Interaction_Finger_Type.ENone;
		internal Hi5_Glove_Interaction_Finger_Type fingerTypeTwo = Hi5_Glove_Interaction_Finger_Type.ENone;
        internal Hi5_Object_Clap_Type handType = Hi5_Object_Clap_Type.ENone;
        private float cd = Hi5_Interaction_Const.Clapcd;
		internal Hi5_Glove_Interaction_Hand hand = null;
        public override void Start()
        {
            //Debug.Log("Hi5_Object_State_Clap start");
            cd = Hi5_Interaction_Const.Clapcd;
            //ObjectItem.ChangeColor (Color.green);

            //if (Hi5_Interaction_Const.TestModifyConstraints)
            {
                ObjectItem.SetIsKinematic(true);
                ObjectItem.SetUseGravity(true);
            }
        }

        override public void FixedUpdate(float deltaTime)
        {

        }

        public override void Update(float deltaTime)
        {
            //if (!Hi5_Interaction_Const.TestModifyConstraints)
            //{
            //    if (ObjectItem.mObjectType == EObject_Type.ECommon)
            //    {
            //        ObjectItem.SetIsKinematic(true);
            //        if (!Hi5_Interaction_Const.TestModifyConstraints)
            //            ObjectItem.SetIsLockYPosition(true);
            //        ObjectItem.SetUseGravity(true);
            //    }
            //    else
            //    {
            //        ObjectItem.SetIsKinematic(true);
            //        if (!Hi5_Interaction_Const.TestModifyConstraints)
            //            ObjectItem.SetIsLockYPosition(true);
            //        ObjectItem.SetUseGravity(true);
            //    }
            //}

            cd -= deltaTime;
            if (IsRealese() && cd<0.0f)
            {
				ObjectItem.mstatemanager.ChangeState(E_Object_State.EStatic);
                {
                    Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance(ObjectItem.idObject,
                        ObjectItem.mObjectType,
                       handType == Hi5_Object_Clap_Type.ELeft ? EHandType.EHandLeft : EHandType.EHandRight,
                        EEventObjectType.EStatic);
					Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
                }

                {
                    Hi5_Glove_Interaction_Hand_Event_Data data = Hi5_Glove_Interaction_Hand_Event_Data.Instance(ObjectItem.idObject,
                        handType == Hi5_Object_Clap_Type.ELeft ? EHandType.EHandLeft : EHandType.EHandRight,
                        EEventHandType.ERelease);
					Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageHandEvent, (object)data, null);
                }
            }
        }
        private bool IsRealese()
        {
			List<Collider>  triggers =  ObjectItem.GetTriggers();
            if(triggers != null  && triggers.Count>0)
            {
                foreach (Collider item in triggers)
                {
                    if (item.gameObject.GetComponent<Hi5_Glove_Collider_Finger>() != null
                        && item.gameObject.GetComponent<Hi5_Glove_Collider_Finger>().mFinger != null
                        && item.gameObject.GetComponent<Hi5_Glove_Collider_Finger>().mFinger.mHand != null)
                    {
                        if (handType == Hi5_Object_Clap_Type.ELeft)
                        {
                            if (item.gameObject.GetComponent<Hi5_Glove_Collider_Finger>().mFinger.mHand.m_IsLeftHand)
                            {
                                return false;
                            }
                        }
                        else if (handType == Hi5_Object_Clap_Type.ERight)
                        {
                            if (!item.gameObject.GetComponent<Hi5_Glove_Collider_Finger>().mFinger.mHand.m_IsLeftHand)
                            {
                                return false;
                            }
                        }
                        
                    }
                    else if (item.gameObject.GetComponent<Hi5_Glove_Collider_Palm>() != null
                        && item.gameObject.GetComponent<Hi5_Glove_Collider_Palm>().mHand != null)
                    {
                        if (handType == Hi5_Object_Clap_Type.ELeft)
                        {
                            if (item.gameObject.GetComponent<Hi5_Glove_Collider_Palm>().mHand.m_IsLeftHand)
                            {
                                return false;
                            }
                        }
                        else if (handType == Hi5_Object_Clap_Type.ERight)
                        {
                            if (!item.gameObject.GetComponent<Hi5_Glove_Collider_Palm>().mHand.m_IsLeftHand)
                            { 
                                return false;
                            }
                        }
                        
                    }
                }
            }
            return true;
        }

       
        public override void End()
        {
			fingerTypeOne = Hi5_Glove_Interaction_Finger_Type.ENone;
			fingerTypeTwo =  Hi5_Glove_Interaction_Finger_Type.ENone;
            handType = Hi5_Object_Clap_Type.ENone;

			//if (Hi5_Interaction_Const.TestChangeState) 
			{
				if(hand != null)
					hand.mState.ChangeState(E_Hand_State.ERelease);
				
			}
			hand = null;
//			ObjectItem.SetIsKinematic (false);
//			ObjectItem.SetUseGravity (true);
//			ObjectItem.SetIsLockYPosition (false);
            //ObjectItem.ChangeColor(ObjectItem.orgColor);
        }
    }
}

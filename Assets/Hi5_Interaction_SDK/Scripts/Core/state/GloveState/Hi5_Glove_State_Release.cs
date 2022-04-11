using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Hi5_Interaction_Interface;
namespace Hi5_Interaction_Core
{
    public class Hi5_Glove_State_Release : Hi5_Glove_State_Base
    {

        // Use this for initialization
        override public void Start()
        {

        }

        override public void FixedUpdate(float deltaTime)
        {

        }
        // Update is called once per frame
        override public void Update(float deltaTime)
        {
			if (Hand == null || Hand.mState == null || Hand.mState.mJudgeMent == null || mDecision == null)
                return;


			if (mDecision.IsPinch ())
			{
				return;
			}



            {
				if (mDecision.IsFlyPinch ())
				{
					return;
				}


                // Debug.Log("start flypinch");
                //空中抓取
//                List<int> pinchs4;
//                //isPinch = Hand.mState.mJudgeMent.IsPinch(out pinchs4, out ObjectId);
//                //if(!isPinch)
//                isPinch = Hand.mState.mJudgeMent.IsFlyIngPinch(out pinchs4, out ObjectId);
//                if (isPinch)
//                {
//                    Hi5_Glove_Interaction_Item item = Hi5_Interaction_Object_Manager.GetObjectManager().GetItemById(ObjectId);
//					float dis = Mathf.Abs(item.PlaneY - item.transform.position.y);
//					if (item != null && item.state == E_Object_State.EMove
//					   && (item.mstatemanager.GetMoveState ().mMoveType == Hi5ObjectMoveType.EThrowMove || item.mstatemanager.GetMoveState ().mMoveType == Hi5ObjectMoveType.EFree )
//						&& !item.mstatemanager.GetMoveState ().IsProtectionFly () && dis>(0.1f*item.GetHeight()))
//					{
//						if (item.mObjectType == EObject_Type.ECommon)
//						{
//							Hi5_Interaction_Message.GetInstance ().DispenseMessage (Hi5_MessageKey.messageFlyPinchObject, pinchs4, Hand, ObjectId);
//							Hi5_Glove_State_Base baseState = mState.GetBaseState (E_Hand_State.EPinch);
//							if (baseState != null) {
//								(baseState as Hi5_Glove_State_Pinch).isPinchCollider = true;
//								(baseState as Hi5_Glove_State_Pinch).objectId = ObjectId;
//							}
//							//Hi5_Glove_Interraction_Item item = Hi5_Interaction_Object_Manager.GetObjectManager().GetItemById(ObjectId);
//							/*if (item != null && item.state == E_Object_State.EMove
//                            && (item.mstatemanager.GetMoveState().mMoveType == Hi5ObjectMoveType.EThrowMove || item.mstatemanager.GetMoveState().mMoveType == Hi5ObjectMoveType.EFree)
//                         !item.mstatemanager.GetMoveState().IsProtectionFly())*/
//							{
//								// Vector3 boxSize = item.GetComponent<BoxCollider>().size;
//								//bool ContactIsSelf = false;
//								//float distance = Hi5_Interaction_Const.GetDistance(Hand.mPalm.transform, item, out ContactIsSelf);
//								//if (ContactIsSelf)
//								{
//									//Debug.Log("distance" + distance);
//									//Vector3 offset = new Vector3(Hand.mPalm.transform.position.x - item.transform.position.x,
//									//                            Hand.mPalm.transform.position.y - item.transform.position.y,
//									//                            Hand.mPalm.transform.position.z - item.transform.position.z).normalized;
//									// Debug.Log("offset x=" + offset.x + "offset y=" + offset.y + "offset z=" + offset.z);
//									//offset = offset * distance;
//									//Debug.Log("offset x="+ offset.x+ "offset y=" + offset.y + "offset z=" + offset.z);
//									item.transform.position = Hand.GetThumbAndMiddlePoin ();
//									//item.ChangeColor(Color.gray);
//								}
//							}
//
//							item.CleanRecord();
//							mState.ChangeState (E_Hand_State.EPinch);
//							Hi5_Glove_Interaction_Hand handTemp = Hand;
//							{
//								Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance (item.idObject,
//									                                                                   item.mObjectType,
//																										handTemp.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
//									                                                                   EEventObjectType.EPinch);
//								Hi5InteractionManager.Instance.GetMessage ().DispenseMessage (Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
//							}
//
//							{
//								Hi5_Glove_Interaction_Hand_Event_Data data = Hi5_Glove_Interaction_Hand_Event_Data.Instance (item.idObject,
//																									 handTemp.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
//									                                                                 EEventHandType.EPinch);
//								Hi5InteractionManager.Instance.GetMessage ().DispenseMessage (Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageHandEvent, (object)data, null);
//							}
//
//							//Hi5_Glove_Interaction_Object_Event_Data data = new Hi5_Glove_Interaction_Object_Event_Data();
//							//if (Hand.m_IsLeftHand)
//							//    data.mHandType = EHandType.EHandLeft;
//							//else
//							//    data.mHandType = EHandType.EHandRight;
//							//data.mObjectType = item.mObjectType;
//							//data.mEventType = EEventType.EPinch;
//							//data.mObjectId = item.idObject;
//							//Hi5InteractionManger.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
//						
//							//return;
//						}
//					}
//                   
//                }
            }

            //pinch2
            {
				
//                List<int> pinchs2;
//                List<Hi5_Glove_Interaction_Finger> fingers = null;
//                bool isPinch2 = Hand.mState.mJudgeMent.IsPinch2(out pinchs2, out fingers, out ObjectId);
//
//                Hi5_Glove_Interaction_Item item = Hi5_Interaction_Object_Manager.GetObjectManager().GetItemById(ObjectId);
////				if (isPinch2)
////				{
////					Debug.Log ("isPinch2 true id"+ObjectId);
////					if (item != null) 
////					{
////						Debug.Log ("isPinch2 state  "+item.state);
////					}
////				}
//
//                if (item != null 
//                    && item.mObjectType == EObject_Type.ECommon
//                    && item.mstatemanager != null
//                    && item.mstatemanager.GetMoveState() != null
//					&& (item.state == E_Object_State.EStatic || item.state == E_Object_State.EPinch || item.state == E_Object_State.EFlyLift || item.state == E_Object_State.EClap
//						|| (item.state == E_Object_State.EMove && item.mstatemanager.GetMoveState().mMoveType == Hi5ObjectMoveType.EPlaneMove)
//						|| (item.state == E_Object_State.EMove && item.mstatemanager.GetMoveState().mMoveType == Hi5ObjectMoveType.EFree)))
//                	{ 
//                        if (isPinch2 && pinchs2 != null && pinchs2.Count > 0 && fingers != null)
//                        {
//                            foreach (Hi5_Glove_Interaction_Finger tempItem in fingers)
//                            {
//                                tempItem.NotifyEnterPinch2State();
//                            }
//                            Hi5_Interaction_Message.GetInstance().DispenseMessage(Hi5_MessageKey.messagePinchObject2, pinchs2, Hand, ObjectId);
//                            Hi5_Glove_State_Base baseState = mState.GetBaseState(E_Hand_State.EPinch2);
//                            if (baseState != null)
//                            {
//                                (baseState as Hi5_Glove_State_Pinch2).objectId = ObjectId;
//                            }
//
//							item.CleanRecord();
//							Hi5_Glove_Interaction_Hand handTemp = Hand;
//							mState.ChangeState(E_Hand_State.EPinch2);
//	                        {
//	                            Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance(item.idObject,
//	                                item.mObjectType,
//									handTemp.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
//	                                EEventObjectType.EPinch);
//								Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
//	                        }
//
//	                        {
//	                            Hi5_Glove_Interaction_Hand_Event_Data data = Hi5_Glove_Interaction_Hand_Event_Data.Instance(item.idObject,
//									handTemp.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
//	                                EEventHandType.EPinch);
//								Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageHandEvent, (object)data, null);
//	                        }
//	                        //Hi5_Glove_Interaction_Object_Event_Data data = new Hi5_Glove_Interaction_Object_Event_Data();
//	                        //if (Hand.m_IsLeftHand)
//	                        //    data.mHandType = EHandType.EHandLeft;
//	                        //else
//	                        //    data.mHandType = EHandType.EHandRight;
//	                        //data.mObjectType = item.mObjectType;
//	                        //data.mEventType = EEventType.EPinch;
//	                        //data.mObjectId = item.idObject;
//	                        //Hi5InteractionManger.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
//							//item.GetComponent<Hi5_Interface_Object>().ChangeColor(Color.green);
//
//	                         return;
//                       }
//                  }
				if (mDecision.IsPinch2 ())
				{
					return;
				}
            }

            //clap
            {
				
                if (mState.State == E_Hand_State.ERelease)
                {
					if (mDecision.IsClap ())
					{
						return;
					}


//                    int objectId = 0;
//                    Hi5_Glove_Interaction_Finger_Type fingTypeOne;
//					Hi5_Glove_Interaction_Finger_Type fingTypeTwo;
//					bool isClap = Hand.mState.mJudgeMent.IsClap(out objectId, out fingTypeOne,out fingTypeTwo);
//                    Hi5_Glove_Interaction_Item item = Hi5_Interaction_Object_Manager.GetObjectManager().GetItemById(objectId);
//					if (item != null && (item.state == E_Object_State.EStatic || item.state == E_Object_State.EPock))
//                    {
//                        Hi5_Object_State_Clap clapState = item.mstatemanager.GetState(E_Object_State.EClap) as Hi5_Object_State_Clap;
//						clapState.fingerTypeOne = fingTypeOne;
//						clapState.fingerTypeTwo = fingTypeTwo;
//						clapState.hand = Hand;
//                        if (Hand.m_IsLeftHand)
//                            clapState.handType = Hi5_Object_State_Clap.Hi5_Object_Clap_Type.ELeft;
//                        else
//                            clapState.handType = Hi5_Object_State_Clap.Hi5_Object_Clap_Type.ERight;
//						Hi5_Glove_Interaction_Hand handTemp = Hand;
//                        item.mstatemanager.ChangeState(E_Object_State.EClap);
////						if (Hi5_Interaction_Const.TestChangeState)
////							mState.ChangeState(E_Hand_State.EClap);
//                        {
//                            Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance(item.idObject,
//                                item.mObjectType,
//								handTemp.m_IsLeftHand? EHandType.EHandLeft: EHandType.EHandRight,
//                                EEventObjectType.EClap);
//							Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
//                        }
//
//                        {
//                            Hi5_Glove_Interaction_Hand_Event_Data data = Hi5_Glove_Interaction_Hand_Event_Data.Instance(item.idObject,
//								handTemp.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
//                                EEventHandType.EClap);
//							Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageHandEvent, (object)data, null);
//                        }
//
//						return;
//                    }
                }
            }

            //lift
            {
				
                //if (Hi5_Interaction_Const.TestFlyMoveNoUsedGravity)
                {
                    if (mState.State == E_Hand_State.ERelease)
                    {
						if (mDecision.IsLift ())
						{
                            
							return;
						}
                    }
                }
            }
        }

      

        public override void End()
        {

        }
    }
}

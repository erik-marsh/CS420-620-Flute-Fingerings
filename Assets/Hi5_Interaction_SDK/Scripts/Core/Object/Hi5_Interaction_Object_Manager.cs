using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public class Hi5_Interaction_Object_Manager : MonoBehaviour
    {
        public static Hi5_Interaction_Object_Manager GetObjectManager()
        {
            return mManager;
        }
        static Hi5_Interaction_Object_Manager mManager = null;
        //public Transform tempMove;
        Dictionary<int, Hi5_Glove_Interaction_Item> mObjectDic = new Dictionary<int, Hi5_Glove_Interaction_Item>();
        #region unity system
        private void Awake()
        {
            transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
            mManager = this;
            AddObjecs();
        }

        private void OnDisable()
        {
            Hi5_Interaction_Message.GetInstance().UnRegisterMessage(PinchObject, Hi5_MessageKey.messagePinchObject);
            Hi5_Interaction_Message.GetInstance().UnRegisterMessage(FlyPinchObject, Hi5_MessageKey.messageFlyPinchObject);
            Hi5_Interaction_Message.GetInstance().UnRegisterMessage(UnPinchObject, Hi5_MessageKey.messageUnPinchObject);
            Hi5_Interaction_Message.GetInstance().UnRegisterMessage(GetObjectById, Hi5_MessageKey.messageGetObjecById);
            Hi5_Interaction_Message.GetInstance().UnRegisterMessage(Pinch2Object, Hi5_MessageKey.messagePinchObject2);
            Hi5_Interaction_Message.GetInstance().UnRegisterMessage(UnPinch2Object, Hi5_MessageKey.messageUnPinchObject2);
            Hi5_Interaction_Message.GetInstance().UnRegisterMessage(LiftObject, Hi5_MessageKey.messageLiftObject);
            mObjectDic.Clear();
        }

        private void OnEnable()
        {
            Hi5_Interaction_Message.GetInstance().RegisterMessage(PinchObject, Hi5_MessageKey.messagePinchObject);
            Hi5_Interaction_Message.GetInstance().RegisterMessage(FlyPinchObject, Hi5_MessageKey.messageFlyPinchObject);
            Hi5_Interaction_Message.GetInstance().RegisterMessage(GetObjectById, Hi5_MessageKey.messageGetObjecById);
            Hi5_Interaction_Message.GetInstance().RegisterMessage(UnPinchObject, Hi5_MessageKey.messageUnPinchObject);
            Hi5_Interaction_Message.GetInstance().RegisterMessage(Pinch2Object, Hi5_MessageKey.messagePinchObject2);
            Hi5_Interaction_Message.GetInstance().RegisterMessage(UnPinch2Object, Hi5_MessageKey.messageUnPinchObject2);
            Hi5_Interaction_Message.GetInstance().RegisterMessage(LiftObject, Hi5_MessageKey.messageLiftObject);
            //mObjectDic.Clear();
        }
        #endregion
        internal Hi5_Glove_Interaction_Item GetItemById(int id)
        {
            if (mObjectDic.ContainsKey(id))
            {
                return mObjectDic[id];
            }
            else
                return null;
        }

        internal Dictionary<int, Hi5_Glove_Interaction_Item> GetItems()
        {
            return mObjectDic;
        }

        private void AddObjecs()
        {
            mObjectDic.Clear();
            Hi5_Glove_Interaction_Item[] objects = transform.GetComponentsInChildren<Hi5_Glove_Interaction_Item>();
            foreach (Hi5_Glove_Interaction_Item item in objects)
            {
                if (!mObjectDic.ContainsKey(item.idObject))
                    mObjectDic.Add(item.idObject, item);
            }
        }

        public void AddObject(Hi5_Glove_Interaction_Item item, int key)
        {
            if (!mObjectDic.ContainsKey(key))
            {
                mObjectDic.Add(key, item);
            }
        }

        public void RemoveObject(int key)
        {
            if (mObjectDic.ContainsKey(key))
                mObjectDic.Remove(key);
        }
        void UnPinchObject(string messageKey, object param1, object param2, object param3, object param4)
		{
			
			if (messageKey.CompareTo(Hi5_MessageKey.messageUnPinchObject) == 0)
			{
				//Debug.Log ("UnPinchObject");
				int objectId = (int)param1;
				Hi5_Glove_Interaction_Hand hand = param2 as Hi5_Glove_Interaction_Hand;
				if ( mObjectDic.ContainsKey(objectId))
				{
					Hi5_Glove_Interaction_Item pinchObject = mObjectDic[objectId];
                    if (pinchObject != null && pinchObject.mObjectType == EObject_Type.ECommon)
                    {
                        Hi5_Object_State_Base state = pinchObject.mstatemanager.GetState(E_Object_State.EPinch);
                        bool isRelease = false;
						bool OtherHandRelease = false;
                        if (state != null && state is Hi5_Object_State_Pinch)
                        {
                            Hi5_Object_State_Pinch pinchState = state as Hi5_Object_State_Pinch;
                            if (pinchState != null && !pinchState.isTestRelease)
                            {
                                if (hand.m_IsLeftHand)
									isRelease = pinchState.CancelPinchHand(Hi5_Object_Pinch_Type.ELeft,out OtherHandRelease);
                                else
									isRelease = pinchState.CancelPinchHand(Hi5_Object_Pinch_Type.ERight,out OtherHandRelease);
                            }
                        }
						if(OtherHandRelease)
						{
							Hi5_Interaction_Message.GetInstance().DispenseMessage(Hi5_MessageKey.messagePinchOtherHandRelease, hand, objectId);
						}

                        if (isRelease)
                        {
							if (!pinchObject.isTouchPlane)
                            {
								//Debug.Log ("!pinchObject.isTouchPlane");
                               
								pinchObject.ChangeState(E_Object_State.EMove);
                                pinchObject.CalculateThrowMove(hand.mPalm.transform, hand);
                                pinchObject.CleanRecord();
//								if (Hi5_Interaction_Const.TestChangeState)
//									hand.mState.ChangeState(E_Hand_State.ERelease);
                                {
                                    Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance(pinchObject.idObject,
                                        pinchObject.mObjectType,
                                       hand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
                                        EEventObjectType.EMove);
									Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
                                }

                                {
                                    Hi5_Glove_Interaction_Hand_Event_Data data = Hi5_Glove_Interaction_Hand_Event_Data.Instance(pinchObject.idObject,
                                        hand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
                                        EEventHandType.EThrow);
									Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageHandEvent, (object)data, null);
                                }
                            }
                            else
                            {
								Debug.Log ("pinchObject.isTouchPlane");
								//if (Hi5_Interaction_Const.TestChangeState)
								hand.mState.ChangeState(E_Hand_State.ERelease);
								pinchObject.ChangeState(E_Object_State.EStatic);
								Hi5_Object_State_Static staticState = pinchObject.mstatemanager.GetState(E_Object_State.EStatic) as Hi5_Object_State_Static;
								staticState.ResetPreTransform();
                                {
                                    Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance(pinchObject.idObject,
                                        pinchObject.mObjectType,
                                       hand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
                                        EEventObjectType.EStatic);
									Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
                                }

                                {
                                    Hi5_Glove_Interaction_Hand_Event_Data data = Hi5_Glove_Interaction_Hand_Event_Data.Instance(pinchObject.idObject,
                                        hand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
                                        EEventHandType.ERelease);
									Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageHandEvent, (object)data, null);
                                }                               
                            }
                        }
                    }
                }
			}
		}

        void UnPinch2Object(string messageKey, object param1, object param2, object param3, object param4)
        {
            if (messageKey.CompareTo(Hi5_MessageKey.messageUnPinchObject2) == 0)
            {
				//Debug.Log ("UnPinch2Object");
                int objectId = (int)param1;
                //ruige 加入双手判断
                Hi5_Glove_Interaction_Hand hand = param2 as Hi5_Glove_Interaction_Hand;
                if (mObjectDic.ContainsKey(objectId))
                {
                    Hi5_Glove_Interaction_Item pinchObject = mObjectDic[objectId];
                    if (pinchObject != null &&  pinchObject.mObjectType == EObject_Type.ECommon)
                    {
                        Hi5_Object_State_Base state = pinchObject.mstatemanager.GetState(E_Object_State.EPinch);
                        bool isRelease = true;
						bool OtherHandRelease = false;
                        if(state != null && state is Hi5_Object_State_Pinch)
                        {
                            Hi5_Object_State_Pinch pinchState = state as Hi5_Object_State_Pinch;
                            if (pinchState != null)
                            {
                                if(hand.m_IsLeftHand)
									isRelease = pinchState.CancelPinchHand(Hi5_Object_Pinch_Type.ELeft,out OtherHandRelease);
                                else
									isRelease = pinchState.CancelPinchHand(Hi5_Object_Pinch_Type.ERight,out OtherHandRelease);
                            }
                        }


						if(OtherHandRelease)
						{
							Hi5_Interaction_Message.GetInstance().DispenseMessage(Hi5_MessageKey.messagePinchOtherHandRelease, hand, objectId);
						}

                        if (isRelease)
                        {
                            if (!pinchObject.isTouchPlane)
                            {
								//Debug.Log ("!pinchObject.isTouchPlane");
                                
								pinchObject.ChangeState(E_Object_State.EMove);
                                pinchObject.CalculateThrowMove(hand.mPalm.transform, hand);
                                pinchObject.CleanRecord();
//								if (Hi5_Interaction_Const.TestChangeState)
//									hand.mState.ChangeState(E_Hand_State.ERelease);
                                {
                                    Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance(pinchObject.idObject,
                                        pinchObject.mObjectType,
                                       hand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
                                        EEventObjectType.EMove);
									Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
                                }

                                {
                                    Hi5_Glove_Interaction_Hand_Event_Data data = Hi5_Glove_Interaction_Hand_Event_Data.Instance(pinchObject.idObject,
                                        hand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
                                        EEventHandType.EThrow);
									Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageHandEvent, (object)data, null);
                                }
                            }
                            else
                            {
								//Debug.Log ("!pinchObject.isTouchPlane");
								pinchObject.ChangeState(E_Object_State.EStatic);
								Hi5_Object_State_Static staticState = pinchObject.mstatemanager.GetState(E_Object_State.EStatic) as Hi5_Object_State_Static;
								staticState.ResetPreTransform();
//								if (Hi5_Interaction_Const.TestChangeState)
//									hand.mState.ChangeState(E_Hand_State.ERelease);
                                {
                                    Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance(pinchObject.idObject,
                                        pinchObject.mObjectType,
                                       hand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
                                        EEventObjectType.EStatic);
									Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
                                }

                                {
                                    Hi5_Glove_Interaction_Hand_Event_Data data = Hi5_Glove_Interaction_Hand_Event_Data.Instance(pinchObject.idObject,
                                        hand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
                                        EEventHandType.ERelease);
									Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageHandEvent, (object)data, null);
                                }


                                //Hi5_Glove_Interaction_Object_Event_Data data = new Hi5_Glove_Interaction_Object_Event_Data();
                                //if (hand.m_IsLeftHand)
                                //    data.mHandType = EHandType.EHandLeft;
                                //else
                                //    data.mHandType = EHandType.EHandRight;
                                //data.mObjectType = pinchObject.mObjectType;
                                //data.mEventType = EEventType.EStatic;
                                //data.mObjectId = pinchObject.idObject;
                                //Hi5InteractionManger.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
                               
                            }
                        }
                            
                    }
                    //pinchObject.transform.parent = null;
                   
                }
            }
        }

        void PinchObject(string messageKey, object param1, object param2, object param3, object param4)
        {
            if (messageKey.CompareTo(Hi5_MessageKey.messagePinchObject) == 0)
            {
                List<int> objectIds = param1 as List<int>;
                Hi5_Glove_Interaction_Hand hand = param2 as Hi5_Glove_Interaction_Hand;
                int objectId = (int)param3;
                if (mObjectDic.ContainsKey(objectId))
                {
                    Hi5_Glove_Interaction_Item pinchObject = mObjectDic[objectId];
                    hand.AddPinchObject(pinchObject.transform, hand.mVisibleHand.palm);
                    pinchObject.SetIsKinematic(true);
                    
                    pinchObject.ChangeState(E_Object_State.EPinch);
                   
                    Hi5_Object_State_Base state = pinchObject.mstatemanager.GetState(E_Object_State.EPinch);
                    if (state != null && (state is Hi5_Object_State_Pinch))
                    {
                        Hi5_Object_State_Pinch pinchState = state as Hi5_Object_State_Pinch;
                        if(hand.m_IsLeftHand)
                            pinchState.SetPinchHand(Hi5_Object_Pinch_Type.ELeft, hand);
                        else
                            pinchState.SetPinchHand(Hi5_Object_Pinch_Type.ERight, hand);
                    }
                }
                //if (objectIds != null && objectIds.Count > 0 && mObjectDic.ContainsKey(objectIds[0]))
                //{
                //    Hi5_Glove_Interraction_Item pinchObject = mObjectDic[objectIds[0]];
                //    hand.AddPinchObject(pinchObject.transform);
                //    pinchObject.SetIsKinematic(true);
                //    pinchObject.ChangeState(E_Object_State.EPinch);
                //}
            }
        }

        void FlyPinchObject(string messageKey, object param1, object param2, object param3, object param4)
        {
            if (messageKey.CompareTo(Hi5_MessageKey.messageFlyPinchObject) == 0)
            {
                List<int> objectIds = param1 as List<int>;
                Hi5_Glove_Interaction_Hand hand = param2 as Hi5_Glove_Interaction_Hand;
                int objectId = (int)param3;
                
                if (mObjectDic.ContainsKey(objectId))
                {
                   // Debug.Log("FlyPinchObject");
                    Hi5_Glove_Interaction_Item pinchObject = mObjectDic[objectId];
                    if (pinchObject != null && pinchObject.mObjectType == EObject_Type.ECommon)
                    {
                        hand.AddPinchObject(pinchObject.transform, hand.mVisibleHand.palm);
                        pinchObject.SetIsKinematic(true);

                        //pinchObject.transform.position = hand.mPalm.transform.position;
                        pinchObject.ChangeState(E_Object_State.EPinch);
                        Hi5_Object_State_Base state = pinchObject.mstatemanager.GetState(E_Object_State.EPinch);
                        if (state != null && (state is Hi5_Object_State_Pinch))
                        {
                            Hi5_Object_State_Pinch pinchState = state as Hi5_Object_State_Pinch;
                            //pinchState.isTestRelease = true;
                            if (hand.m_IsLeftHand)
                                pinchState.SetPinchHand(Hi5_Object_Pinch_Type.ELeft, hand);
                            else
                                pinchState.SetPinchHand(Hi5_Object_Pinch_Type.ERight, hand);
                        }
                    }
                  
                }
                //if (objectIds != null && objectIds.Count > 0 && mObjectDic.ContainsKey(objectIds[0]))
                //{
                //    Hi5_Glove_Interraction_Item pinchObject = mObjectDic[objectIds[0]];
                //    hand.AddPinchObject(pinchObject.transform);
                //    pinchObject.SetIsKinematic(true);
                //    pinchObject.ChangeState(E_Object_State.EPinch);
                //}
            }
        }


        void Pinch2Object(string messageKey, object param1, object param2, object param3,object param4)
        {
            if (messageKey.CompareTo(Hi5_MessageKey.messagePinchObject2) == 0)
            {
                List<int> objectIds = param1 as List<int>;
                Hi5_Glove_Interaction_Hand hand = param2 as Hi5_Glove_Interaction_Hand;
                int objectId = (int)param3;
                if (mObjectDic.ContainsKey(objectId))
                {
                    //ruige 加入判断
                    Hi5_Glove_Interaction_Item pinchObject = mObjectDic[objectId];
                    if (pinchObject != null && pinchObject.mObjectType == EObject_Type.ECommon)
                    {
                        hand.AddPinch2Object(pinchObject.transform, hand.mVisibleHand.palm);

                        pinchObject.SetIsKinematic(true);
                        pinchObject.ChangeState(E_Object_State.EPinch);
                        Hi5_Object_State_Base state = pinchObject.mstatemanager.GetState(E_Object_State.EPinch);
                        if (state != null && (state is Hi5_Object_State_Pinch))
                        {
                            Hi5_Object_State_Pinch pinchState = state as Hi5_Object_State_Pinch;
                            if (hand.m_IsLeftHand)
                                pinchState.SetPinchHand(Hi5_Object_Pinch_Type.ELeft, hand);
                            else
                                pinchState.SetPinchHand(Hi5_Object_Pinch_Type.ERight, hand);
                        }
                    }
                   
                }
            }
        }

        void LiftObject(string messageKey, object param1, object param2, object param3, object param4)
        {
            //Debug.Log("LiftObject");
            // Hi5_MessageKey.messageLiftObject, Hand, ObjectId
            if (messageKey.CompareTo(Hi5_MessageKey.messageLiftObject) == 0)
            {
                Hi5_Glove_Interaction_Hand hand = param1 as Hi5_Glove_Interaction_Hand;
                int id = (int)param2;
                if (mObjectDic.ContainsKey(id))
                {
                    Hi5_Glove_Interaction_Item pinchObject = mObjectDic[id];
                    if (pinchObject != null && pinchObject.mObjectType == EObject_Type.ECommon)
                    {
                        Hi5_Object_State_Base state = pinchObject.mstatemanager.GetState(E_Object_State.EFlyLift);
                        if (state is Hi5_Object_State_Fly_Lift)
                        {
                            (state as Hi5_Object_State_Fly_Lift).hand = hand;
                        }
                        pinchObject.mstatemanager.ChangeState(E_Object_State.EFlyLift);
                    }
                       
                }
            }
        }
        void GetObjectById(string messageKey, object param1, object param2, object param3, object param4)
        {
            if (messageKey.CompareTo(Hi5_MessageKey.messageGetObjecById) == 0)
            {
                int id = (int)param1;
                if (mObjectDic.ContainsKey(id))
                {
                    List<Hi5_Glove_Interaction_Item> transformTemp = param2 as List<Hi5_Glove_Interaction_Item>;
                    transformTemp.Add(mObjectDic[id]);
                    param2 = transformTemp as object;
                }
            }
        }


    }
}

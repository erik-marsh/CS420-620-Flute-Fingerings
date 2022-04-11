using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hi5_Interaction_Core;
using HI5.VRCalibration;
using HI5;
namespace Hi5_Interaction_Interface
{
    public class Hi5_Interface_Hand : MonoBehaviour
    {
        protected Hi5_Hand_Visible_Hand mHand = null;
		bool isRegister = false;
        protected Hi5_Hand_Visible_Hand Hand
        {
            get
            {
                if (mHand == null)
                    mHand = gameObject.GetComponent<Hi5_Hand_Visible_Hand>();
                return mHand;
            }
        }

		/**
     	* Get hand state.
    	**/
		public E_Interface_Hand_State HandState
        {
            get
            {
                if (mHand == null)
                    mHand = gameObject.GetComponent<Hi5_Hand_Visible_Hand>();
                if (mHand == null)
                {
					return E_Interface_Hand_State.ERelease;
                }
                else
                {
					E_Interface_Hand_State state = E_Interface_Hand_State.ERelease;
                    if (mHand.mGlove_Hand == null || mHand.GetHand() == null || mHand.GetHand().mState == null)
						return E_Interface_Hand_State.ERelease;
                    else
                    {
						switch(mHand.GetHand().mState.State)
						{
							case E_Hand_State.EPinch:
							case E_Hand_State.EPinch2:
								state = E_Interface_Hand_State.EPinch;
								break;
							case E_Hand_State.ELift:
								state = E_Interface_Hand_State.ELift;
							break;
							case E_Hand_State.ERelease:
								state = E_Interface_Hand_State.ERelease;
							break;
						}
						return state;
                    }
                }
            }
        }

		/**
     	* Get hand state and hand pinch or lift interacted object.
    	**/
		public E_Interface_Hand_State GetHandState(out int interactionObjectId)
		{
			interactionObjectId = -1000;
			if (mHand == null)
				mHand = gameObject.GetComponent<Hi5_Hand_Visible_Hand>();
			if (mHand == null) {
				return E_Interface_Hand_State.ERelease;
			}
			else
			{
				if (mHand.mGlove_Hand == null || mHand.GetHand () == null || mHand.GetHand ().mState == null)
					return E_Interface_Hand_State.ERelease;
				else
				{
					E_Interface_Hand_State stateValve = E_Interface_Hand_State.ERelease; 
					switch(mHand.GetHand().mState.State)
					{
					case E_Hand_State.EPinch:
						interactionObjectId = mHand.PinchObjectId ();
						break;
					case E_Hand_State.EPinch2:
						interactionObjectId = mHand.Pinch2ObjectId ();
						break;
					case E_Hand_State.ELift:
						interactionObjectId = mHand.LiftObjectId ();
						break;
					case E_Hand_State.ERelease:
						break;
					}
					return stateValve;
				}
			}
		}
   
        protected void OnEnable()
        {
            if (mHand == null)
                mHand = gameObject.GetComponent<Hi5_Hand_Visible_Hand>();
			if (Hi5InteractionManager.Instance != null)
            {
				Hi5InteractionManager.Instance.GetMessage().RegisterMessage(MessageFun, Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageHandEvent);
				isRegister = true; ;
            }
                
        }

		/**
     	* Get hand gesture recognition status.
    	**/
        public Hi5_Glove_Gesture_Recognition_State GetRecognitionState()
        {
            return mHand.GetHand().GetRecognitionState();
        }

        protected void Update()
        {

			if (Hi5InteractionManager.Instance != null && !isRegister)
            {
				Hi5InteractionManager.Instance.GetMessage().RegisterMessage(MessageFun, Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageHandEvent);
				isRegister = true; ;
            }
        }

        protected void OnDisable()
        {
			if(Hi5InteractionManager.Instance != null)
				Hi5InteractionManager.Instance.GetMessage().UnRegisterMessage(MessageFun, Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageHandEvent);
			isRegister = false;
        }

		/**
		 * Get hand event.
		 * */
        void MessageFun(string messageKey, object param1, object param2)
        {
            if (messageKey.CompareTo(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageHandEvent) == 0)
            {
                Hi5_Glove_Interaction_Hand_Event_Data data = param1 as Hi5_Glove_Interaction_Hand_Event_Data;
                bool isRun = false;
                if (data.mHandType == EHandType.EHandLeft && mHand.m_IsLeftHand)
                {
                    isRun = true;
                }
                else if (data.mHandType == EHandType.EHandRight && !mHand.m_IsLeftHand)
                {
                    isRun = true;
                }
                if (!isRun)
                    return;
                switch (data.mEventType)
                {
					case EEventHandType.EClap:
						if (data.mHandType == EHandType.EHandLeft && mHand.m_IsLeftHand) {
							HI5_Manager.EnableLeftVibration (200);
						} else if (data.mHandType == EHandType.EHandRight && !mHand.m_IsLeftHand) {
							HI5_Manager.EnableRightVibration (200);
						}
                        break;
					case EEventHandType.EPoke:
                        break;
					case EEventHandType.EPinch:
					
                        break;
                    case EEventHandType.EThrow:
                        break;
                    case EEventHandType.ELift:
                        break;
                    case EEventHandType.ERelease:
                        break;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hi5_Interaction_Core
{
    public class Hi5_Glove_State_Pinch : Hi5_Glove_State_Base
    {
       internal  bool isPinchCollider = false;
        internal int objectId = -1000;
        Dictionary<Hi5_Glove_Interaction_Finger_Type, Hi5_Glove_Interaction_Finger> fingerColliders = new Dictionary<Hi5_Glove_Interaction_Finger_Type, Hi5_Glove_Interaction_Finger>();
        public override void Start()
        {
            
        }
        override public void FixedUpdate(float deltaTime)
        {

        }

        void  AngelPinchRelease()
        {
            Dictionary<Hi5_Glove_Interaction_Finger_Type, Hi5_Glove_Interaction_Finger> fingers = Hand.mFingers;
            bool isRelease = false;
            int releaseCount = 0;
            if (fingers.ContainsKey(Hi5_Glove_Interaction_Finger_Type.EThumb))
            {
                if (fingers[Hi5_Glove_Interaction_Finger_Type.EThumb].IsPinchRelease)
                {
                    releaseCount++;
                }
            }

            if (fingers.ContainsKey(Hi5_Glove_Interaction_Finger_Type.EMiddle))
            {
                if (fingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].IsPinchRelease)
                {
                    releaseCount++;
                }
            }
            if (fingers.ContainsKey(Hi5_Glove_Interaction_Finger_Type.EIndex))
            {
                if (fingers[Hi5_Glove_Interaction_Finger_Type.EIndex].IsPinchRelease)
                {
                    releaseCount++;
                }
            }
            else if (fingers.ContainsKey(Hi5_Glove_Interaction_Finger_Type.ERing))
            {
                if (fingers[Hi5_Glove_Interaction_Finger_Type.ERing].IsPinchRelease)
                {
                    releaseCount++;
                }
            }
            if (releaseCount >= 2)
                isRelease = true;
            if (isRelease)
            {
				mState.ChangeState(E_Hand_State.ERelease);
                Hi5_Interaction_Message.GetInstance().DispenseMessage(Hi5_MessageKey.messageUnPinchObject, (object)Hand.mPinchObjectId, Hand);
                
                if (Hand != null)
                    Hand.mPinchObjectId = -1;
            }
        }

        public override void Update(float deltaTime)
        {
            if (isPinchCollider)
            {
                AngelPinchRelease();
            }
            else
            {

            }
        }

        public override void End()
        {
            fingerColliders.Clear();
        }

        internal  void AddFingerCollider(Hi5_Glove_Interaction_Finger finger)
        {
            if(!fingerColliders.ContainsKey(finger.m_finger_type))
                fingerColliders.Add(finger.m_finger_type, finger);
            
        }
    }
}

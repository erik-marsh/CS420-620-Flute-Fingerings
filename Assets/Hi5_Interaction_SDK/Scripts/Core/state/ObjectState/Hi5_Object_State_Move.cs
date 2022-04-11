using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public class Hi5_Object_State_Move : Hi5_Object_State_Base
    {
        Hi5_Object_Move mMoveObject = null;
        internal Hi5_Object_Move Move
        {
            get { return mMoveObject; }
        }

        override internal protected void Init(Hi5_Glove_Interaction_Item itemObject, Hi5_Object_State_Manager state)
        {
            mObjectItem = itemObject;
            mState = state;
            mMoveObject = new Hi5_Object_Move(itemObject, state);
            mMoveObject.SetAttribute(mObjectItem.AirFrictionRate, mObjectItem.PlaneFrictionRate);
        }

        override public void Start()
        {
            mMoveObject.Start();
        }
        
        override public void Update(float deltaTime)
        {
            //List<int> pinchs;

          
            //bool isPinch = IsPinch(out pinchs);
            //if (isPinch)
            //{
            //    Hi5_Interaction_Message.GetInstance().DispenseMessage(Hi5_MessageKey.messagePinchObject, pinchs, Hand);
            //    mState.ChangeState(Hi5_Glove_Interaction_State.E_Hand_State.EPinch);
            //}

            if (mMoveObject != null)
            {
                mMoveObject.Update(deltaTime);
            }

            //if (mCollision != null)
            //{
            //    mMoveObject.SetPlaneMove(mCollision);
            //    mCollision = null;
            //}
        }

        override public void End()
        {
            mMoveObject.StopMove();
			//ObjectItem.ChangeColor(ObjectItem.orgColor);
        }

        override public void FixedUpdate(float deltaTime)
        {
            if (mMoveObject != null)
            {
                mMoveObject.FixUpdate(deltaTime);
            }
        }
        Collision mCollision = null;
        internal void SetPlaneMove(Collision collision)
        {
            //mCollision = collision;
            mMoveObject.SetPlaneMove(collision);
        }
    }
}

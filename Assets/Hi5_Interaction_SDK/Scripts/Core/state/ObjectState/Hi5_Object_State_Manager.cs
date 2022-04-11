using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    
    public class Hi5_Object_State_Manager
    {
        Dictionary<E_Object_State, Hi5_Object_State_Base> mDicState = new Dictionary<E_Object_State, Hi5_Object_State_Base>();
        Hi5_Object_State_Base mCurrentState = null;
        Hi5_Glove_Interaction_Item mItem;

        
        private E_Object_State mState = E_Object_State.EStatic;
        /// <summary>
        /// get hand gesture state
        /// </summary>
        public E_Object_State State
        {
            get { return mState; }
            set { mState = value; }
        }

        protected Hi5_Object_State_Manager()
        {
            State = E_Object_State.EStatic;
            mDicState.Clear();
        }

        protected void init(Hi5_Glove_Interaction_Item objectItem)
        {

            mItem = objectItem;
            Hi5_Object_State_Pinch pinchState = new Hi5_Object_State_Pinch();
            pinchState.Init(objectItem, this);
            Hi5_Object_State_Static releaseState = new Hi5_Object_State_Static();
            releaseState.Init(objectItem, this);
            Hi5_Object_State_Move moveState = new Hi5_Object_State_Move();
            moveState.Init(objectItem, this);
            Hi5_Object_State_Clap  clapState = new Hi5_Object_State_Clap();
            clapState.Init(objectItem, this);
			Hi5_Object_State_Poke pokeState = new Hi5_Object_State_Poke();
			pokeState.Init(objectItem,this);
            //if (Hi5_Interaction_Const.TestFlyMoveNoUsedGravity)
            {
                Hi5_Object_State_Fly_Lift flyLift = new Hi5_Object_State_Fly_Lift();
                flyLift.Init(objectItem, this);
                mDicState.Add(E_Object_State.EFlyLift, flyLift);
            }
            mDicState.Add(E_Object_State.EPinch, pinchState);
            mDicState.Add(E_Object_State.EStatic, releaseState);
            mDicState.Add(E_Object_State.EMove, moveState);
            mDicState.Add(E_Object_State.EClap, clapState);
			mDicState.Add(E_Object_State.EPoke, pokeState);
            mCurrentState = releaseState;
           
        }

        internal void Update(float deltaTime)
        {
            if (mCurrentState != null)
                mCurrentState.Update(deltaTime);
        }

        internal void FixedUpdate(float deltaTime)
        {
            if (mCurrentState != null)
                mCurrentState.FixedUpdate(deltaTime);
        }

        internal static Hi5_Object_State_Manager CreateState(Hi5_Glove_Interaction_Item objectItem)
        {
            Hi5_Object_State_Manager state = new Hi5_Object_State_Manager();
            state.init(objectItem);
            return state;
        }

        internal protected void ChangeState(E_Object_State state,bool iForce = false)
        {
            if (State == state && !iForce)
            {
                return;
            }

            State = state;
			if ( mItem != null)
				mItem.state = State;
            mCurrentState.End();
            if (mDicState.ContainsKey(state))
            {
                mDicState[state].Start();
				//Debug.Log ("object state"+state);
                mCurrentState = mDicState[state];
            }
        }

        internal protected void StopThrowMove(bool isTouchObject =false)
        {
            if (State == E_Object_State.EMove)
            {
                if (mDicState[E_Object_State.EMove] is Hi5_Object_State_Move)
                {
                    if ((mDicState[E_Object_State.EMove] as Hi5_Object_State_Move).Move != null)
                    {
                        if((mDicState[E_Object_State.EMove] as Hi5_Object_State_Move).Move.mMoveType == Hi5ObjectMoveType.EThrowMove ||
                            (mDicState[E_Object_State.EMove] as Hi5_Object_State_Move).Move.mMoveType == Hi5ObjectMoveType.EFree)
                        {
                            ////Debug.Log("StopThrowMove");
                            ChangeState(E_Object_State.EStatic,true);
                            Hi5_Object_State_Base temp = GetState(E_Object_State.EStatic);
                            if (isTouchObject)
                                (temp as Hi5_Object_State_Static).isTouchObject = true;
                        }
                    }
                }
            }
			if (State == E_Object_State.EFlyLift) {
                //Debug.Log("StopThrowMove");
                ChangeState (E_Object_State.EStatic, true);
			}
        }


		internal void CalculateThrowMove(Queue<Hi5_Position_Record> records, Transform handPalm,Hi5_Glove_Interaction_Hand hand)
        {
            if (mDicState[E_Object_State.EMove] is Hi5_Object_State_Move)
            {
                if ((mDicState[E_Object_State.EMove] as Hi5_Object_State_Move).Move != null)
					(mDicState[E_Object_State.EMove] as Hi5_Object_State_Move).Move.CalculateThrowMove(records, handPalm,hand);
            }
        }

        internal void SetPlaneMove(Collision collision)
        {
            if (mDicState[E_Object_State.EMove] is Hi5_Object_State_Move)
            {
                if ((mDicState[E_Object_State.EMove] as Hi5_Object_State_Move).Move != null)
                {
                    (mDicState[E_Object_State.EMove] as Hi5_Object_State_Move).SetPlaneMove(collision);
                }
            }
        }
        internal Hi5_Object_Move GetMoveState()
        {
            if(mDicState.ContainsKey(E_Object_State.EMove))
                return (mDicState[E_Object_State.EMove] as Hi5_Object_State_Move).Move;
            else
                return null;
        }

        internal Hi5_Object_State_Base GetState(E_Object_State state)
        {
            return mDicState[state];
        }
    }
}

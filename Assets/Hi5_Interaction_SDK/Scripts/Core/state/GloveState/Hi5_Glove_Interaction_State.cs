using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public class Hi5_Glove_Interaction_State
    {
        /// <summary>
        ///  hand gesture state
        /// </summary>
        /// 
        internal Hi5_Object_JudgeMent mJudgeMent = null;
        Dictionary<E_Hand_State, Hi5_Glove_State_Base> mDicState = new Dictionary<E_Hand_State, Hi5_Glove_State_Base>();
        Hi5_Glove_State_Base mCurrentState = null;
		Hi5_Glove_Decision mDecision = null;

        private E_Hand_State mState = E_Hand_State.ERelease;
        /// <summary>
        /// get hand gesture state
        /// </summary>
        public E_Hand_State State
        {
            get { return mState; }
            set
			{
				mState = value;
				if (mJudgeMent != null && mJudgeMent.Hand != null)
					mJudgeMent.Hand.mCurrentState = mState;
			}
        }
        internal protected Hi5_Glove_State_Base GetBaseState(E_Hand_State stateEnum)
        {
            if (mDicState.ContainsKey(stateEnum))
                return mDicState[stateEnum];
            else
                return null;
        }
        protected Hi5_Glove_Interaction_State()
        {
            State = E_Hand_State.ERelease;
            mDicState.Clear();
        }

        internal Hi5_Glove_State_Pinch2 GetPinch2()
        {
            if (mDicState.ContainsKey(E_Hand_State.EPinch2))
            {
                Hi5_Glove_State_Pinch2 temp = mDicState[E_Hand_State.EPinch2] as Hi5_Glove_State_Pinch2;
                return temp;
            }
            else
                return null;
        }

        internal Hi5_Glove_State_Pinch GetPinch()
        {
            if (mDicState.ContainsKey(E_Hand_State.EPinch))
            {
                Hi5_Glove_State_Pinch temp = mDicState[E_Hand_State.EPinch] as Hi5_Glove_State_Pinch;
                return temp;
            }
            else
                return null;
        }

        internal Hi5_Glove_State_Lift GetLift()
        {
            if (mDicState.ContainsKey(E_Hand_State.ELift))
            {
                Hi5_Glove_State_Lift temp = mDicState[E_Hand_State.ELift] as Hi5_Glove_State_Lift;
                return temp;
            }
            else
                return null;
        }

        protected void init(Hi5_Glove_Interaction_Hand hand)
        {
			mJudgeMent = new Hi5_Object_JudgeMent();
            mDecision = new Hi5_Glove_Decision (mJudgeMent,hand,this);
            Hi5_Glove_State_Pinch pinchState = new Hi5_Glove_State_Pinch();
           
			pinchState.Init(hand,this, mDecision);
            Hi5_Glove_State_Release releaseState = new Hi5_Glove_State_Release();
			releaseState.Init(hand,this, mDecision);
            Hi5_Glove_State_Pinch2 pinch2 = new Hi5_Glove_State_Pinch2();
			pinch2.Init(hand, this, mDecision);
			Hi5_Glove_State_Clap clap = new Hi5_Glove_State_Clap();
			clap.Init(hand, this, mDecision);
            
            Hi5_Glove_State_Lift Lift = new Hi5_Glove_State_Lift();
			Lift.Init(hand, this, mDecision);
			mDicState.Add(E_Hand_State.EClap, clap);
			mDicState.Add(E_Hand_State.ELift, Lift);
            mDicState.Add(E_Hand_State.EPinch,pinchState);
            mDicState.Add(E_Hand_State.ERelease,releaseState);
            mDicState.Add(E_Hand_State.EPinch2, pinch2);
            mCurrentState = releaseState;
           
            mJudgeMent.mStateManager = this;
            mJudgeMent.Hand = hand;
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

        internal static Hi5_Glove_Interaction_State CreateState(Hi5_Glove_Interaction_Hand hand)
        {
            Hi5_Glove_Interaction_State state = new Hi5_Glove_Interaction_State();
            state.init(hand);
            return state;
        }

        internal protected void ChangeState(E_Hand_State state)
        {
            mCurrentState.End();
            State = state;
            if (mDicState.ContainsKey(state))
            {
                mDicState[state].Start();
                mCurrentState = mDicState[state];
            }
        }
			
        internal void AddPinchCollider(Hi5_Glove_Interaction_Finger finger)
        {
            if (mDicState.ContainsKey(E_Hand_State.EPinch))
            {
                (mDicState[E_Hand_State.EPinch] as Hi5_Glove_State_Pinch).AddFingerCollider(finger);
            }
        }

        internal void AddPinch2Collider(Hi5_Glove_Interaction_Finger finger)
        {
            if (mDicState.ContainsKey(E_Hand_State.EPinch2))
            {
                (mDicState[E_Hand_State.EPinch2] as Hi5_Glove_State_Pinch2).AddFingerCollider(finger);
            }
        }

		public void  GetGloveState<T>(E_Hand_State hand_type,out T backValue) where T : Hi5_Glove_State_Base
		{
			backValue = null;

			Hi5_Glove_State_Base item = GetBaseState(hand_type);
			if(item != null)
			{
				T  scrip = item as T;
				backValue = scrip;
			}

		}
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public abstract class Hi5_Glove_State_Base
    {
        internal protected Hi5_Glove_Interaction_Hand Hand
        {
            set { mHand = value; }
            get { return mHand; }
        }

        internal protected void Init(Hi5_Glove_Interaction_Hand hand, 
									Hi5_Glove_Interaction_State state,
									Hi5_Glove_Decision decision)
        {
            Hand = hand;
            mState = state;
			mDecision = decision;
        }

        private Hi5_Glove_Interaction_Hand mHand = null;
        internal protected Hi5_Glove_Interaction_State mState = null;
		internal protected Hi5_Glove_Decision mDecision = null;
        public abstract void Start();
        public abstract void Update(float deltaTime);

        public abstract void End();
        public abstract void FixedUpdate(float deltaTime);
        

    }
}

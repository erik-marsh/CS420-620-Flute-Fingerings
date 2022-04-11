using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public class Hi5_Glove_State_Lift : Hi5_Glove_State_Base
    {
        internal int objectId = -1000;
        public override void Start()
        {

        }

        public override void FixedUpdate(float deltaTime)
        {

        }

        public override void Update(float deltaTime)
        {
			if (Hand == null || Hand.mState == null || Hand.mState.mJudgeMent == null || mDecision == null)
				return;


			if (mDecision.IsPinch ())
			{
				return;
			}

			if (mDecision.IsPinch2 ())
			{
				return;
			}
        }

        public override void End()
        {
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
	public class Hi5_Glove_Gesture_Recognition
	{
		Hi5_Glove_Interaction_Hand mHand = null;
		Hi5_Glove_Gesture_Recognition_Record mRecord = null;
		internal Hi5_Glove_Gesture_Recognition (Hi5_Glove_Interaction_Hand hand)
		{
			mRecord = new Hi5_Glove_Gesture_Recognition_Record ();
			mHand = hand;
		}
        internal bool IsWantPinch = false;
		internal void Update(float detTime)
        {

            if (IsCloseThumbAndIndexCollider())
            {
                mHand.mVisibleHand.SetThumbAndIndexFingerCollider(false);
            }
            else
                mHand.mVisibleHand.SetThumbAndIndexFingerCollider(true);

            if (IsHandFist())
            {
                mRecord.RecordGesture(Hi5_Glove_Gesture_Recognition_State.EFist);
                mState = Hi5_Glove_Gesture_Recognition_State.EFist;
                mHand.mVisibleHand.ChangeColor(Color.red);
            }
            else if (IsHandIndexPoint())
            {
                mRecord.RecordGesture(Hi5_Glove_Gesture_Recognition_State.EIndexPoint);
                mState = Hi5_Glove_Gesture_Recognition_State.EIndexPoint;
                mHand.mVisibleHand.ChangeColor(Color.black);
            }

            else if (IsHandPlane())
            {
                mRecord.RecordGesture(Hi5_Glove_Gesture_Recognition_State.EHandPlane);
                mState = Hi5_Glove_Gesture_Recognition_State.EHandPlane;
                mHand.mVisibleHand.ChangeColor(Color.green);
            }
            else if (IsOk())
            {
                mRecord.RecordGesture(Hi5_Glove_Gesture_Recognition_State.EOk);
                mState = Hi5_Glove_Gesture_Recognition_State.EOk;
                mHand.mVisibleHand.ChangeColor(Color.yellow);
            }
        
            else
            {
                mRecord.RecordGesture(Hi5_Glove_Gesture_Recognition_State.ENone);
                mState = Hi5_Glove_Gesture_Recognition_State.ENone;
                mHand.mVisibleHand.ChangeColor(mHand.mVisibleHand.orgColor);
            }
            //if (Hi5_Interaction_Const.TestPinchOpenCollider)
            //{
            //    if (IsFlyPinch() || IsPinch2())
            //    {
            //        mHand.mVisibleHand.ChangeColor(Color.blue);
            //        IsWantPinch = true;
            //    }
            //    else
            //    {
            //        mHand.mVisibleHand.ChangeColor(mHand.mVisibleHand.orgColor);
            //        IsWantPinch = false;
            //    }
            //}
              
        }

        internal bool IsOk()
        {
           // return mHand.mFingers[Hi5_Glove_Interaction_Finger_Type.EThumb].IsTumbColliderIndex();
            if (mHand != null && mHand.mState != null && mHand.mState.mJudgeMent != null)
            {
                return mHand.mState.mJudgeMent.IsOK();
            }
            else
                return false;
        }

        internal bool IsCloseThumbAndIndexCollider()
        {
            if (mHand != null && mHand.mState != null && mHand.mState.mJudgeMent != null)
            {
                return mHand.mState.mJudgeMent.IsCloseThumbAndIndexCollider();
            }
            else
                return false;
        }

        internal bool IsFlyPinch()
		{
			if (mHand != null && mHand.mState != null && mHand.mState.mJudgeMent != null) {
				return mHand.mState.mJudgeMent.IsGestureFlyPinch ();
			} else
				return false;
		}

        internal bool IsPinch2()
        {
            if (mHand != null && mHand.mState != null && mHand.mState.mJudgeMent != null)
            {
                return mHand.mState.mJudgeMent.IsGesturePinch2();
            }
            else
                return false;
        }

		internal bool IsHandPlane()
		{
			if (mHand != null && mHand.mState != null && mHand.mState.mJudgeMent != null) {
				return mHand.mState.mJudgeMent.IsFingerPlane ();
			} else
				return false;
		}

		internal bool IsHandFist()
		{
			if (mHand != null && mHand.mState != null && mHand.mState.mJudgeMent != null) {
				return mHand.mState.mJudgeMent.IsHandFist ();
			} else
				return false;
		}

        internal bool IsHandIndexPoint()
        {
            if (mHand != null && mHand.mState != null && mHand.mState.mJudgeMent != null)
            {
                return mHand.mState.mJudgeMent.IsHandIndexPoint();
            }
            else
                return false;
        }
        
        internal bool IsRecordFlyPinch()
		{
			return mRecord.IsHaveGesture (Hi5_Glove_Gesture_Recognition_State.EOk);
		}

		internal void CleanRecord()
		{
			mRecord.RecordClean ();
		}

        internal Hi5_Glove_Gesture_Recognition_State mState = Hi5_Glove_Gesture_Recognition_State.ENone;
        internal Hi5_Glove_Gesture_Recognition_State GetRecognitionState()
        {
            return mState;
        }
    }
	public enum Hi5_Glove_Gesture_Recognition_State
	{
		ENone = 0,
		EOk,
		EFist,
        EIndexPoint,
		EHandPlane
	}

	public class Hi5_Glove_Gesture_Recognition_Record
	{
		internal Hi5_Glove_Gesture_Recognition_State mState = Hi5_Glove_Gesture_Recognition_State.ENone;
		Queue<Hi5_Glove_Gesture_Recognition_State> mQueuePositionRecord = new Queue<Hi5_Glove_Gesture_Recognition_State>();
		internal void RecordClean()
		{
			mQueuePositionRecord.Clear();
		}

		internal Queue<Hi5_Glove_Gesture_Recognition_State> GetRecord()
		{
			return mQueuePositionRecord;
		}

		internal void RecordGesture(Hi5_Glove_Gesture_Recognition_State state)
		{
			mState = state;
			if (mQueuePositionRecord.Count > (5 - 1))
			{
				mQueuePositionRecord.Dequeue();
			}
			mQueuePositionRecord.Enqueue(state);
		}

		internal bool IsHaveGesture(Hi5_Glove_Gesture_Recognition_State state)
		{
			foreach (Hi5_Glove_Gesture_Recognition_State item in mQueuePositionRecord) 
			{
				if (item == state)
					return true;
			}
			return false;
		}
	}
}
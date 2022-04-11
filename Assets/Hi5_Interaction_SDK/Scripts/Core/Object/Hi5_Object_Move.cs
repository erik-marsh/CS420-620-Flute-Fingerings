using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public enum Hi5ObjectMoveType
    {
        ENone = 0,
        EThrowMove = 1,
        EPlaneMove = 2,
        EFree = 3,
    }
    public class Hi5_Object_Move
    {
        #region move data
        public class ObjectMoveData
        {
            public Vector3 mDirection;
            public float cd = 0.0f;
            public float y;
            public float ySpeed;
        }
        #endregion

        protected internal Hi5_Glove_Interaction_Item mItem = null;
        protected internal ObjectMoveData mMoveData = null;
        internal float mAirFrictionRate = 0.0f;
        internal float mPlaneFrictionRate;
        Hi5_Object_State_Manager mState = null;

        Vector3 contactPointNormal;
        // bool isMove = false;
        internal Hi5ObjectMoveType mMoveType = Hi5ObjectMoveType.ENone;
    
        //飞行中暂停
        bool mIsFlyMovePause = false;
        //float mFlyMoveStartProtectionCd = Hi5_Interaction_Const.FingerPinchPauseProtectionTime;
        bool IsProtectFly = false;
        Transform protectedTransform;
        float mWaitFlyPauseTime = Hi5_Interaction_Const.FingerColliderPinchPauseTime;
        internal Hi5_Object_Move(Hi5_Glove_Interaction_Item objectItem, Hi5_Object_State_Manager state)
        {
            mState = state;
            mItem = objectItem;
           // objectItem.GetComponent<Rigidbody>
        }

        internal void Start()
        {
            //if (Hi5_Interaction_Const.TestModifyConstraints)
            //{
              

            //    //if(mMoveType == Hi5ObjectMoveType)
            //}
        }

        internal void SetMoveData(ObjectMoveData data)
        {
            mMoveData = data;
        }

        internal void StopMove()
        {
            //isMove = false;
            mMoveData = null;
            mMoveType = Hi5ObjectMoveType.ENone;
            //if (Hi5_Interaction_Const.TestPlaneMoveUsePhysic)
            {
//                mItem.SetIsKinematic(false);
//                mItem.SetIsLockYPosition(true);
            }
            //else
            //{
            //    mItem.SetIsKinematic(true);
            //    mItem.SetIsLockYPosition(false);
            //}
                
           // mItem.SetUseGravity(true);
            
            //mItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        }

        private void FreeFly(float deltaTime)
        {
            //if (Hi5_Interaction_Const.TestPlaneMoveUsePhysic)
            {
                //if (!Hi5_Interaction_Const.TestModifyConstraints)
                //{
                //    mItem.SetUseGravity(true);
                //    mItem.SetIsKinematic(false);
                //    mItem.CleanLock();
                //}
               

                //if (Hi5_Interaction_Const.TestPlaneStatic)
                {
					if (mItem.isTouchPlane) {
                      
                        mState.ChangeState (E_Object_State.EStatic);
						//if (Hi5_Interaction_Const.TestChangeState)
						{
							Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance(mItem.idObject, mItem.mObjectType, EHandType.EHandLeft, EEventObjectType.EStatic);
							Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
						}
					}
					if (mItem.mQueuePositionRecord != null) {
						Hi5_Position_Record[] records = mItem.mQueuePositionRecord.ToArray ();
						if (records != null && records.Length > 0) {
							if (records.Length - 2 > 0) {
								Vector3 offset = records [records.Length - 2].position - records [records.Length - 1].position;
								if (Mathf.Abs (offset.x) < 0.003f && Mathf.Abs (offset.y) < 0.003f&& Mathf.Abs (offset.z) < 0.003f) {
                                    //Debug.Log("FreeFly test");
									mState.ChangeState (E_Object_State.EStatic);
									//if (Hi5_Interaction_Const.TestChangeState)
									{
										Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance(mItem.idObject, mItem.mObjectType, EHandType.EHandLeft, EEventObjectType.EStatic);
										Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
									}
								}
							}
						}
					}
				}
            }
        }

        internal void Update(float deltaTime)
        {
            if (mMoveType == Hi5ObjectMoveType.EThrowMove)
            {
                ThrowMove(deltaTime);

                if (protectedTransform == null)
                {
                    if (IsProtectFly)
                        IsProtectFly = false;
                }
                else
                {
                    float distance = Vector3.Distance(protectedTransform.position, mItem.transform.position);
                    bool ContactIsSelf = true;
                    // float distance = Hi5_Interaction_Const.GetDistance(protecedTransform, mItem, out ContactIsSelf);
                    if (!ContactIsSelf)
                        IsProtectFly = false;
                    else
                    {
                        if (distance < Hi5_Interaction_Const.ThrowObjectProtectionDistance)
                        {
                            IsProtectFly = true;
                        }
                        else
                            IsProtectFly = false;
                    }
                }
            }
            
            PlaneMove(deltaTime);
            if (mMoveType == Hi5ObjectMoveType.EFree)
            {
                FreeFly(deltaTime);
                if (protectedTransform == null)
                {
                    if (IsProtectFly)
                        IsProtectFly = false;
                }
                else
                {
                    float distance = Vector3.Distance(protectedTransform.position, mItem.transform.position);
                    bool ContactIsSelf = true;
                    if (!ContactIsSelf)
                        IsProtectFly = false;
                    else
                    {
                        if (distance < Hi5_Interaction_Const.ThrowObjectProtectionDistance)
                        {
                            IsProtectFly = true;
                        }
                        else
                            IsProtectFly = false;
                    }
                }
            }
        }


        internal bool IsProtectionFly()
        {
            if (mMoveType == Hi5ObjectMoveType.EFree || mMoveType == Hi5ObjectMoveType.EThrowMove)
            {
                return IsProtectFly;
            }
            else
                return false;
        }

        internal void FixUpdate(float deltaTime)
        {
            
            
        }
        internal void SetFreeMove(Transform preted)
        {
            protectedTransform = preted;
            mMoveType = Hi5ObjectMoveType.EFree;
            IsProtectFly = true ;
            //if (Hi5_Interaction_Const.TestModifyConstraints)
            {
                //mItem.SetUseGravity(true);
                //mItem.SetIsKinematic(false);
                //mItem.CleanLock();

                if (mItem.GetComponent<Hi5_Object_Property>() != null
                       && mItem.GetComponent<Hi5_Object_Property>().ObjectProperty.AirMoveProperty != null)
                {
                    mItem.SetIsKinematic(false);
                    mItem.SetUseGravity(true);
                    mItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    mItem.GetComponent<Hi5_Object_Property>().SetRotation(mItem.GetComponent<Hi5_Object_Property>().ObjectProperty.AirMoveProperty.ConstraintsFreezeRotation,
                       mItem.GetComponent<Hi5_Object_Property>().ObjectProperty.AirMoveProperty.ConstraintsFreezeRotation,
                       mItem.GetComponent<Hi5_Object_Property>().ObjectProperty.AirMoveProperty.ConstraintsFreezeRotation);
                }
                else
                {
                    mItem.SetIsKinematic(false);
                    mItem.SetUseGravity(true);
                    mItem.CleanLock();
                }
            }
        }

		//internal void SetPlaneMove()
		//{
		//	if (mMoveType == Hi5ObjectMoveType.EThrowMove || mMoveType == Hi5ObjectMoveType.EFree)
		//		return;
		//	mMoveType = Hi5ObjectMoveType.EPlaneMove;
		//}

        internal void SetPlaneMove(Collision collision)
        {
			
            if (mMoveType == Hi5ObjectMoveType.EThrowMove || mMoveType == Hi5ObjectMoveType.EFree)
                return;
            //if (Hi5_Interaction_Const.TestPlaneMoveUsePhysic)
            {
                mMoveType = Hi5ObjectMoveType.EPlaneMove;

                if (mItem.GetComponent<Hi5_Object_Property>() != null
                     && mItem.GetComponent<Hi5_Object_Property>().ObjectProperty.PlaneMoveProperty != null)
                {
                    mItem.SetIsKinematic(false);
                    mItem.SetUseGravity(true);
                    mItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    mItem.GetComponent<Hi5_Object_Property>().SetRotation(mItem.GetComponent<Hi5_Object_Property>().ObjectProperty.PlaneMoveProperty.ConstraintsFreezeRotation,
                       mItem.GetComponent<Hi5_Object_Property>().ObjectProperty.PlaneMoveProperty.ConstraintsFreezeRotation,
                       mItem.GetComponent<Hi5_Object_Property>().ObjectProperty.PlaneMoveProperty.ConstraintsFreezeRotation);
                }
                else
                {
                    mItem.SetIsKinematic(false);
                    mItem.SetUseGravity(true);
                    mItem.CleanLock();
                }
                return;
            }

            Queue<Hi5_Position_Record> records = null;
            if (collision.gameObject.GetComponent<Hi5_Hand_Collider_Visible_Finger>() != null)
            {
                records = collision.gameObject.GetComponent<Hi5_Hand_Collider_Visible_Finger>().mQueuePositionRecord;
            }
            if (collision.gameObject.GetComponent<Hi5_Hand_Palm_Move>() != null)
            {
                records = collision.gameObject.GetComponent<Hi5_Hand_Palm_Move>().GetRecord();
            }
            if (records != null && records.Count > 0)
            {
                Vector3 distanceVector = Vector3.zero;
                int index = 0;
                int weightPointCount = 0;
                float timeCount = 0.0f;

                foreach (Hi5_Position_Record item in records)
                {
                    if (Hi5_Interaction_Const.RecordPositionWeight.Length > index)
                    {
                        int weight = Hi5_Interaction_Const.RecordPositionWeight[index];
                        weightPointCount += weight;
                        timeCount += item.mIntervalTime * weight;
                        distanceVector += item.mMoveVector * weight;
                    }
                    index++;
                }
                mMoveData = new ObjectMoveData();
                mMoveData.mDirection = distanceVector / timeCount;
                mMoveData.y = mMoveData.mDirection.y;
                mMoveData.ySpeed = mMoveData.mDirection.y;
                mMoveType = Hi5ObjectMoveType.EPlaneMove;
                
                contactPointNormal = collision.contacts[0].normal;
                contactPointNormal.y = 0.0f;
                {
                    //mItem.SetUseGravity(true);
                    //mItem.SetIsKinematic(false);
                    //mItem.CleanLock();

                    if (mItem.GetComponent<Hi5_Object_Property>() != null
                      && mItem.GetComponent<Hi5_Object_Property>().ObjectProperty.PlaneMoveProperty != null)
                    {
                        mItem.SetIsKinematic(false);
                        mItem.SetUseGravity(true);
                        mItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                        mItem.GetComponent<Hi5_Object_Property>().SetRotation(mItem.GetComponent<Hi5_Object_Property>().ObjectProperty.PlaneMoveProperty.ConstraintsFreezeRotation,
                           mItem.GetComponent<Hi5_Object_Property>().ObjectProperty.PlaneMoveProperty.ConstraintsFreezeRotation,
                           mItem.GetComponent<Hi5_Object_Property>().ObjectProperty.PlaneMoveProperty.ConstraintsFreezeRotation);
                    }
                    else
                    {
                        mItem.SetIsKinematic(false);
                        mItem.SetUseGravity(true);
                        mItem.CleanLock();
                    }
                }
            }
        }

        private void ThrowMove(float deltaTime)
        {
            if (mMoveType == Hi5ObjectMoveType.EThrowMove && mMoveData != null )
            {
				//if (Hi5_Interaction_Const.TestPhycis)
				{
					ObjectMoveData temp = mMoveData;
					{
						mMoveData.cd += deltaTime;
						float gravity = Physics.gravity.y*8.1f/9.8f;
						//float gravity = Physics.gravity.y;
						float y = 0;
						//y = (mMoveData.ySpeed /*- gravity*(9.8f-8.1f) * deltaTime*/)* (1.0f - mAirFrictionRate);
						y = (temp.ySpeed + gravity * deltaTime)* (1.0f - mAirFrictionRate);
						mMoveData.ySpeed = y;

						mMoveData.mDirection *= (1.0f - mAirFrictionRate);
						Vector3 move = new Vector3(mMoveData.mDirection.x, y, mMoveData.mDirection.z);
						if (!float.IsNaN(move.x) && !float.IsNaN(move.y) && !float.IsNaN(move.z))
						{
							if (deltaTime > Hi5_Interaction_Const.PRECISION)
								//mItem.transform.Translate(move * deltaTime, Space.World);
								mItem.GetComponent<Rigidbody>().velocity = move;
						}
					}
				}
            }
        }

        private void PlaneMove(float deltaTime)
        {
            //if (!mItem.isTouchPlane)
            //{
            //    //Debug.Log("SetPlaneMove false isTouchPlane");
            //    //mItem.GetComponent<Rigidbody>().useGravity = true;
            //    //mItem.GetComponent<Rigidbody>().isKinematic = false;
            //    mItem.SetUseGravity(true);
            //    mItem.SetIsKinematic(false);
            //    mState.ChangeState(E_Object_State.EStatic);
            //    return;
            //}
            if (mMoveType == Hi5ObjectMoveType.EPlaneMove)
            {
    //            if (!Hi5_Interaction_Const.TestModifyConstraints)
    //           {
				//	mItem.SetIsKinematic(false);
				//	mItem.SetUseGravity(true);
				//	mItem.CleanLock ();
				//} 
				//else
				//{
				//	mItem.SetIsKinematic(false);
				//	mItem.SetUseGravity(true);
				//	if (mItem.isTouchPlane)
				//	{
				//		mItem.SetIsLockYPosition(true);
				//	}
				//	else
				//	{
				//		mItem.SetIsLockYPosition(false);
				//	}
				//}
                
                if (mItem.mQueuePositionRecord != null)
                {
                    Hi5_Position_Record[] records = mItem.mQueuePositionRecord.ToArray();
                    if (records != null && records.Length > 0)
                    {
                        if (records.Length - 2 > 0)
                        {
                            Vector3 offset =  records[records.Length - 2].position - records[records.Length - 1].position;
                            if (Mathf.Abs(offset.x) < 0.001f && Mathf.Abs(offset.z) < 0.001f)
                            {
                               // Debug.Log("PlaneMove");
                                mState.ChangeState(E_Object_State.EStatic);
                            }
                        }
                    }
                }

                //if (Mathf.Abs(contactMove.x) < 0.003f && Mathf.Abs(contactMove.z) < 0.003f)
                //{
                //    mItem.SetIsLockYPosition(false);
                //    mState.ChangeState(E_Object_State.EStatic);
                //} 

                return;
            }
            return;
        }
      
        internal void SetAttribute(float AirFrictionRate,
                                   float PlaneFrictionRate)
        {
            mAirFrictionRate = AirFrictionRate;
            mPlaneFrictionRate = PlaneFrictionRate;
           // mMass = Mass;
        }

        internal void SetFlyPause()
        {
            //if (Hi5_Interaction_Const.TestFlyMoveNoUsedGravity)
            //{
            //    if (mMoveType == Hi5ObjectMoveType.EFree || mMoveType == Hi5ObjectMoveType.EThrowMove)
            //    {
            //        if (!mIsFlyMovePause && mFlyMoveStartProtectionCd < 0.0f)
            //        {
            //            mIsFlyMovePause = true;
            //            mItem.SetUseGravity(false);
            //            mItem.SetIsKinematic(true);
            //            mWaitFlyPauseTime = Hi5_Interaction_Const.FingerColliderPinchPauseTime;
            //        }
            //    }
                   
            //}
              
        }
		//GameObject Clone = null;
		internal void CalculateThrowMove(Queue<Hi5_Position_Record> records, Transform handPalm,Hi5_Glove_Interaction_Hand hand)
        {
			

            mIsFlyMovePause = false;
            int index = 0;
            int weightPointCount = 0;
            float timeCount = 0.0f;
            Vector3 distanceVector = Vector3.zero;

            foreach (Hi5_Position_Record item in records)
            {
                if (Hi5_Interaction_Const.RecordPositionWeight.Length > index)
                {
                    int weight = Hi5_Interaction_Const.RecordPositionWeight[index];
                    weightPointCount += weight;
                    timeCount += item.mIntervalTime * weight;
                    distanceVector += item.mMoveVector * weight;
                }
                index++;
            }
			if (index <= 1) {
//				mMoveData = new ObjectMoveData();
//				mMoveData.mDirection = new Vector3 (0.0f, 0.08598139f, 0.0f);
//				mMoveData.y = mMoveData.mDirection.y;
//				mMoveData.ySpeed = mMoveData.mDirection.y;
				//Debug.Log("index <= 1");
				Vector3 temp = hand.MoveAnchor.position - hand.mPalm.transform.position;
				temp.Normalize ();
				mMoveData = new ObjectMoveData();
				mMoveData.mDirection = temp*0.3998139f;
				mMoveData.y = mMoveData.mDirection.y;
				mMoveData.ySpeed = mMoveData.mDirection.y;

//				mMoveData = new ObjectMoveData();
//				mMoveData.mDirection = distanceVector / timeCount* Hi5_Interaction_Const.ThrowSpeed;
//				mMoveData.y = mMoveData.mDirection.y;
//				mMoveData.ySpeed = mMoveData.mDirection.y;

				Hi5_Interaction_Const.WriteItemMoveXml (records,mMoveData);
			}
			else 
			{
				//Debug.Log("index > 1");
				mMoveData = new ObjectMoveData();
				mMoveData.mDirection = distanceVector / timeCount* Hi5_Interaction_Const.ThrowSpeed;
				mMoveData.y = mMoveData.mDirection.y;
				mMoveData.ySpeed = mMoveData.mDirection.y;
				Hi5_Interaction_Const.WriteItemMoveXml (records,mMoveData);
			}

            //if (Hi5_Interaction_Const.TestPhycis) 
            //if (Hi5_Interaction_Const.TestModifyConstraints)
            {
				//mItem.SetIsKinematic (false);
				//mItem.SetUseGravity (true);
				//mItem.CleanLock ();

                if (mItem.GetComponent<Hi5_Object_Property>() != null
                   && mItem.GetComponent<Hi5_Object_Property>().ObjectProperty.AirMoveProperty != null)
                {
                    mItem.SetIsKinematic(false);
                    mItem.SetUseGravity(true);
                    mItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    mItem.GetComponent<Hi5_Object_Property>().SetRotation(mItem.GetComponent<Hi5_Object_Property>().ObjectProperty.AirMoveProperty.ConstraintsFreezeRotation,
                       mItem.GetComponent<Hi5_Object_Property>().ObjectProperty.AirMoveProperty.ConstraintsFreezeRotation,
                       mItem.GetComponent<Hi5_Object_Property>().ObjectProperty.AirMoveProperty.ConstraintsFreezeRotation);
                }
                else
                {
                    mItem.SetIsKinematic(false);
                    mItem.SetUseGravity(true);
                    mItem.CleanLock();
                }
            }

            mMoveType = Hi5ObjectMoveType.EThrowMove;
            protectedTransform = handPalm;
            IsProtectFly = true;
        }
    }
}


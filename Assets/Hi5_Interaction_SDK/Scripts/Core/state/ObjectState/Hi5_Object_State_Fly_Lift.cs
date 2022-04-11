using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public class Hi5_Object_State_Fly_Lift : Hi5_Object_State_Base
    {

        public enum ELift_Director
        {
            X,
            XContrary,
            Y,
            YContrary,
            Z,
            ZContrary
        }

        internal Hi5_Glove_Interaction_Hand hand = null;
        Hi5_Object_Move mMoveObject = null;
		Vector3 offsetDragSpeed = Vector3.zero;
        float closeTime = 0.2f;
		protected Hi5_Record mRecord = new Hi5_Record();
       // bool isOpenCollider = false;
        bool test2 = true;
        Transform parentTransform = null;
        internal Hi5_Object_Move Move
        {
            get { return mMoveObject; }
        }

        Vector3 parentPosition = Vector3.zero;
       // Vector3 offset = Vector3.zero;
 
        override internal protected void Init(Hi5_Glove_Interaction_Item itemObject, Hi5_Object_State_Manager state)
        {
            mObjectItem = itemObject;
            mState = state;
            mMoveObject = new Hi5_Object_Move(itemObject, state);
            mMoveObject.SetAttribute(mObjectItem.AirFrictionRate, mObjectItem.PlaneFrictionRate);
        }
        RigidbodyConstraints preConstraints;
        override public void Start()
        {
            //offset = Vector3.zero;

            mObjectItem.SetIsKinematic(true);
            mObjectItem.SetUseGravity(true);
            if (mObjectItem.gameObject.GetComponent<Rigidbody>() != null)
            {
                mObjectItem.gameObject.GetComponent<Rigidbody>().Sleep();
#if UNITY_2017_2_OR_NEWER
#else
                mObjectItem.gameObject.GetComponent<Rigidbody>().WakeUp();
#endif
            }

            mObjectItem.transform.parent = hand.mPalm.transform;
            parentTransform = hand.mPalm.transform;
            parentPosition = mObjectItem.transform.localPosition;
            mRecord.RecordClean ();
			offsetDragSpeed = Vector3.zero;
            preConstraints = mObjectItem.GetComponent<Rigidbody>().constraints;
            if (mObjectItem.GetComponent<BoxCollider>() != null)
            {
                Quaternion q = Quaternion.identity;
                ELift_Director director = Rotation(hand.mPalm.transform, mObjectItem.transform,out q);
                mObjectItem.transform.rotation = q;
                if (hand.mPalm.mChildCollider != null)
                {
                    //hand.mPalm.mChildCollider.gameObject.SetActive(true);
                    float HPalemTemp = -hand.mPalm.mChildCollider.localPosition.y;// hand.mPalm.mChildCollider.position.y - hand.mPalm.transform.position.y;
                    float HPalm = hand.mPalm.mChildCollider.GetComponent<BoxCollider>().size.y * hand.mPalm.mChildCollider.localScale.y;
                    Vector3 one = mObjectItem.GetComponent<BoxCollider>().ClosestPoint(hand.mPalm.mChildCollider.transform.position);
                    Vector3 two = hand.mPalm.mChildCollider.GetComponent<BoxCollider>().ClosestPoint(mObjectItem.transform.position);
                    Vector3 offset = two - one;
                    //Debug.Log("one = " + one);
                    //Debug.Log("two = " + two);
                    //Debug.Log("offset = "+ offset);
                    mObjectItem.transform.position += new Vector3(0.0f, offset.y, 0.0f);
                    switch (director)
                    {
                        case ELift_Director.X:
                        case ELift_Director.XContrary:
                            {
                                //mObjectItem.transform.position = hand.mPalm.mChildCollider.position;
                                float height = mObjectItem.GetComponent<BoxCollider>().size.x * mObjectItem.transform.localScale.x;
                                float offsetTemp = hand.mPalm.mChildCollider.position.y - (mObjectItem.transform.position.y + height / 2);
                                if (offsetTemp >= 0)
                                {
                                    //mObjectItem.transform.position = new Vector3(0.0f, mObjectItem.transform.position.y + height / 2, 0.0f);
                                    //Debug.Log("offsetTemp X");
                                    mObjectItem.transform.position = new Vector3(mObjectItem.transform.position.x,
                                       mObjectItem.transform.parent.transform.position.y + height / 2 - HPalm / 2+ HPalemTemp,
                                       mObjectItem.transform.position.z);
                                    //if (hand.mPalm.mChildCollider.GetComponent<BoxCollider>() != null)
                                    //    mObjectItem.transform.localPosition += new Vector3(0.0f, hand.mPalm.mChildCollider.GetComponent<BoxCollider>().size.y, 0.0f);
                                }
                                else
                                {
                                    //mObjectItem.transform.position = new Vector3(0.0f, mObjectItem.transform.position.y+ height / 2, 0.0f);
                                    //Debug.Log("offsetTemp X-");
                                    mObjectItem.transform.position = new Vector3(mObjectItem.transform.position.x,
                                       mObjectItem.transform.parent.transform.position.y + height / 2 - HPalm / 2+ HPalemTemp,
                                       mObjectItem.transform.position.z);
                                }
                            }
                            break;
                        case ELift_Director.Y:
                        case ELift_Director.YContrary:
                            {
                                // mObjectItem.transform.position = hand.mPalm.mChildCollider.position;
                                float height = mObjectItem.GetComponent<BoxCollider>().size.y * mObjectItem.transform.localScale.y;
                                float offsetTemp = hand.mPalm.mChildCollider.position.y - (mObjectItem.transform.position.y + height / 2);
                                if (offsetTemp >= 0)
                                {
                                  // mObjectItem.transform.position = new Vector3(0.0f, mObjectItem.transform.position.y + height / 2, 0.0f);
                                   // Debug.Log("offsetTemp Y");
                                    mObjectItem.transform.position = new Vector3(mObjectItem.transform.position.x,
                                       mObjectItem.transform.parent.transform.position.y + height / 2 - HPalm / 2+ HPalemTemp,
                                       mObjectItem.transform.position.z);
                                    //if (hand.mPalm.mChildCollider.GetComponent<BoxCollider>() != null)
                                    //    mObjectItem.transform.localPosition += new Vector3(0.0f, hand.mPalm.mChildCollider.GetComponent<BoxCollider>().size.y, 0.0f);
                                }
                                else
                                {
                                   // mObjectItem.transform.position = new Vector3(0.0f, mObjectItem.transform.position.y+ height / 2, 0.0f);
                                   // Debug.Log("offsetTemp Y-");
                                    mObjectItem.transform.position = new Vector3(mObjectItem.transform.position.x,
                                       mObjectItem.transform.parent.transform.position.y + height / 2 - HPalm / 2+ HPalemTemp,
                                       mObjectItem.transform.position.z);
                                }
                            }
                            break;
                        case ELift_Director.Z:
                        case ELift_Director.ZContrary:
                            {
                                 //mObjectItem.transform.position = hand.mPalm.mChildCollider.position;
                                float height = mObjectItem.GetComponent<BoxCollider>().size.z* mObjectItem.transform.localScale.z;
                                float offsetTemp = hand.mPalm.mChildCollider.position.y - (mObjectItem.transform.position.y + height / 2);
                                if (offsetTemp >= 0)
                                {
                                   //// Debug.Log("offsetTemp Z");
                                    mObjectItem.transform.position = new Vector3(mObjectItem.transform.position.x,
                                        mObjectItem.transform.parent.transform.position.y + height / 2 - HPalm / 2+ HPalemTemp,
                                        mObjectItem.transform.position.z);
                                    //if(hand.mPalm.mChildCollider.GetComponent<BoxCollider>() != null)
                                    //    mObjectItem.transform.localPosition += new Vector3(0.0f,hand.mPalm.mChildCollider.GetComponent<BoxCollider>().size.y,0.0f);
                                }
                                else
                                {
                                   // Debug.Log("height = "+ offsetTemp);

                                    //mObjectItem.transform.localPosition = new Vector3(0.0f, mObjectItem.transform.localPosition.y, 0.0f);
                                     mObjectItem.transform.position = new Vector3(mObjectItem.transform.position.x, 
                                         mObjectItem.transform.parent.transform.position.y + height/2- HPalm/2+ HPalemTemp, 
                                         mObjectItem.transform.position.z);
                                   // Debug.Log("offsetTemp Z-");
                                }
                            }
                            break;
                    }
                }
                

            }
            else if (mObjectItem.mLifeCollider != null)
            {
                //mObjectItem.mLifeCollider.gameObject.SetActive(true);
                Quaternion q = Quaternion.identity;
                ELift_Director director = Rotation(hand.mPalm.transform, mObjectItem.mLifeCollider.transform,out q);
                mObjectItem.transform.rotation = q;
                if (hand.mPalm.mChildCollider != null)
                {
                    //hand.mPalm.mChildCollider.gameObject.SetActive(true);
                    float HPalemTemp = -hand.mPalm.mChildCollider.localPosition.y;// hand.mPalm.mChildCollider.position.y - hand.mPalm.transform.position.y;
                    float HPalm = hand.mPalm.mChildCollider.GetComponent<BoxCollider>().size.y * hand.mPalm.mChildCollider.localScale.y;
                    Vector3 one = mObjectItem.mLifeCollider.GetComponent<BoxCollider>().ClosestPoint(hand.mPalm.mChildCollider.transform.position);
                    Vector3 two = hand.mPalm.mChildCollider.GetComponent<BoxCollider>().ClosestPoint(mObjectItem.mLifeCollider.transform.position);
                    Vector3 offset = two - one;
                    //Debug.Log("one = " + one);
                    //Debug.Log("two = " + two);
                    //Debug.Log("offset = "+ offset);
                    //mObjectItem.transform.position += new Vector3(0.0f, offset.y, 0.0f);
                    switch (director)
                    {
                        case ELift_Director.X:
                        case ELift_Director.XContrary:
                            {
                                //mObjectItem.transform.position = hand.mPalm.mChildCollider.position;
                                float height = mObjectItem.mLifeCollider.GetComponent<BoxCollider>().size.x * mObjectItem.mLifeCollider.transform.localScale.x;
                                float offsetTemp = hand.mPalm.mChildCollider.position.y - (mObjectItem.mLifeCollider.transform.position.y + height / 2);
                                if (offsetTemp >= 0)
                                {
                                    //mObjectItem.transform.position = new Vector3(0.0f, mObjectItem.transform.position.y + height / 2, 0.0f);
                                    //Debug.Log("offsetTemp X");
                                    mObjectItem.transform.position = new Vector3(mObjectItem.transform.position.x,
                                       mObjectItem.transform.parent.transform.position.y + height / 2 - HPalm / 2 + HPalemTemp,
                                       mObjectItem.transform.position.z);
                                    //if (hand.mPalm.mChildCollider.GetComponent<BoxCollider>() != null)
                                    //    mObjectItem.transform.localPosition += new Vector3(0.0f, hand.mPalm.mChildCollider.GetComponent<BoxCollider>().size.y, 0.0f);
                                }
                                else
                                {
                                    //mObjectItem.transform.position = new Vector3(0.0f, mObjectItem.transform.position.y+ height / 2, 0.0f);
                                   // Debug.Log("offsetTemp X-");
                                    mObjectItem.transform.position = new Vector3(mObjectItem.transform.position.x,
                                       mObjectItem.transform.parent.transform.position.y + height / 2 - HPalm / 2 + HPalemTemp,
                                       mObjectItem.transform.position.z);
                                }
                            }
                            break;
                        case ELift_Director.Y:
                        case ELift_Director.YContrary:
                            {
                                // mObjectItem.transform.position = hand.mPalm.mChildCollider.position;
                                float height = mObjectItem.mLifeCollider.GetComponent<BoxCollider>().size.y * mObjectItem.mLifeCollider.transform.localScale.y;
                                float offsetTemp = hand.mPalm.mChildCollider.position.y - (mObjectItem.mLifeCollider.transform.position.y + height / 2);
                                if (offsetTemp >= 0)
                                {
                                    // mObjectItem.transform.position = new Vector3(0.0f, mObjectItem.transform.position.y + height / 2, 0.0f);
                                    //Debug.Log("offsetTemp Y");
                                    mObjectItem.transform.position = new Vector3(mObjectItem.transform.position.x,
                                       mObjectItem.transform.parent.transform.position.y + height / 2 - HPalm / 2 + HPalemTemp,
                                       mObjectItem.transform.position.z);
                                    //if (hand.mPalm.mChildCollider.GetComponent<BoxCollider>() != null)
                                    //    mObjectItem.transform.localPosition += new Vector3(0.0f, hand.mPalm.mChildCollider.GetComponent<BoxCollider>().size.y, 0.0f);
                                }
                                else
                                {
                                    // mObjectItem.transform.position = new Vector3(0.0f, mObjectItem.transform.position.y+ height / 2, 0.0f);
                                    //Debug.Log("offsetTemp Y-");
                                    mObjectItem.transform.position = new Vector3(mObjectItem.transform.position.x,
                                       mObjectItem.transform.parent.transform.position.y + height / 2 - HPalm / 2 + HPalemTemp,
                                       mObjectItem.transform.position.z);
                                }
                            }
                            break;
                        case ELift_Director.Z:
                        case ELift_Director.ZContrary:
                            {
                                //mObjectItem.transform.position = hand.mPalm.mChildCollider.position;
                                float height = mObjectItem.mLifeCollider.GetComponent<BoxCollider>().size.z * mObjectItem.mLifeCollider.transform.localScale.z;
                                float offsetTemp = hand.mPalm.mChildCollider.position.y - (mObjectItem.mLifeCollider.transform.position.y + height / 2);
                                if (offsetTemp >= 0)
                                {
                                   // Debug.Log("offsetTemp Z");
                                    mObjectItem.transform.position = new Vector3(mObjectItem.transform.position.x,
                                        mObjectItem.transform.parent.transform.position.y + height / 2 - HPalm / 2 + HPalemTemp,
                                        mObjectItem.transform.position.z);
                                    //if(hand.mPalm.mChildCollider.GetComponent<BoxCollider>() != null)
                                    //    mObjectItem.transform.localPosition += new Vector3(0.0f,hand.mPalm.mChildCollider.GetComponent<BoxCollider>().size.y,0.0f);
                                }
                                else
                                {
                                  

                                    //mObjectItem.transform.localPosition = new Vector3(0.0f, mObjectItem.transform.localPosition.y, 0.0f);
                                    mObjectItem.transform.position = new Vector3(mObjectItem.transform.position.x,
                                        mObjectItem.transform.parent.transform.position.y + height / 2 - HPalm / 2 + HPalemTemp,
                                        mObjectItem.transform.position.z);
                                    //Debug.Log("offsetTemp Z-");
                                }
                            }
                            break;
                    }
                    //mObjectItem.mLifeCollider.gameObject.SetActive(false);
                }

            }
            // mObjectItem.SetIsLockRotation(true);
            hand.mPalm.OpenPhyCollider(true);
        }
        override public void Update(float deltaTime)
        {
			FollowpalmMove (deltaTime);
            mRecord.RecordPosition(Time.deltaTime, mObjectItem.transform);
            if (mObjectItem.GetComponent<BoxCollider>() != null)
            {

              //  mObjectItem.transform.localPosition = offset;


            }
            //if (test2)
            //{
            //Vector3 tempParentPosition = parentTransform.position;
            //mObjectItem.transform.localPosition = parentPosition;
            //parentPosition = parentTransform.position;
            //}
            
            bool IsRelease = false;
			float distance = Vector3.Distance (hand.mPalm.transform.position, mObjectItem.transform.position);
			if (distance > Hi5_Interaction_Const.liftChangeMoveDistance)
            {
			    IsRelease = true;
		    }
            //
			float angle = Vector3.Angle(Hi5_Interaction_Object_Manager.GetObjectManager().transform.up, -hand.mPalm.transform.up);
			if (!IsRelease &&  angle > 40.0f && angle > 0.0f) {
				IsRelease = true;
			}
			if (!IsRelease && !mObjectItem.IsLiftTrigger()) {
                    //Debug.Log("lis release");
                    //if(!Hi5_Interaction_Const.TestLiftRelease)
                    //    IsRelease = true;
					
			}
			if(IsRelease)
			{
                mObjectItem.GetComponent<Rigidbody>().constraints = preConstraints;
                hand.mPalm.OpenPhyCollider(false);
                Transform temp = hand.mPalm.transform;
					ObjectItem.transform.parent = Hi5_Interaction_Object_Manager.GetObjectManager ().transform;
					if (mObjectItem.mObjectType == EObject_Type.ECommon)
					{
						Hi5_Glove_Interaction_Hand handTemp = hand;
						mObjectItem.ChangeState (E_Object_State.EMove);
						//if(mObjectItem.mstatemanager != null && mObjectItem.mstatemanager.GetMoveState() != null)
						mObjectItem.mstatemanager.GetMoveState ().SetFreeMove (temp);
						{
							Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance (ObjectItem.idObject,
								ObjectItem.mObjectType,
								handTemp.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
								EEventObjectType.EMove);
							Hi5InteractionManager.Instance.GetMessage ().DispenseMessage (Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
						}

						{
							Hi5_Glove_Interaction_Hand_Event_Data data = Hi5_Glove_Interaction_Hand_Event_Data.Instance (ObjectItem.idObject,
								handTemp.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
								EEventHandType.ERelease);
							Hi5InteractionManager.Instance.GetMessage ().DispenseMessage (Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageHandEvent, (object)data, null);
						}
					}
				}
			//}
        }


		private void FollowpalmMove(float deltime)
		{
            if (hand != null && hand.mVisibleHand != null && hand.mVisibleHand.palmMove != null)
            {
                Queue<Hi5_Position_Record> records = mRecord.GetRecord();

                //Queue<Hi5_Position_Record>  records =  hand.mVisibleHand.palmMove.GetRecord();
                if (records != null)
                {
                    Hi5_Position_Record[] recordArray = records.ToArray();
                }
            }
		}

        override public void End()
        {
			hand.mState.ChangeState(E_Hand_State.ERelease);
			

            hand = null;
			mRecord.RecordClean ();
        }

        public override void FixedUpdate(float deltaTime)
        {

        }

        private ELift_Director Rotation(Transform other, Transform objectItem,out Quaternion q)
        {
            ELift_Director director = ELift_Director.Y;
            float angle1 = Vector3.Angle(other.transform.up, objectItem.transform.up);
            float angle11 = 180.0f - angle1;
            float angle2 = Vector3.Angle(other.transform.up, objectItem.transform.right);
            float angle22 = 180.0f - angle2;
            float angle3 = Vector3.Angle(other.transform.up, objectItem.transform.forward);
            float angle33 = 180.0f - angle3;
            List<float> angles = new List<float>();
            angles.Add(angle1);
            angles.Add(angle11);
            angles.Add(angle2);
            angles.Add(angle22);
            angles.Add(angle3);
            angles.Add(angle33);
            angles.Sort((x, y) => x.CompareTo(y));

            if (angle1 == angles[0])
            {
               // Debug.Log("Y +");
                Vector3 normal = other.transform.up;
                Vector3 up = objectItem.transform.up;
                Vector3 YRotationNomal = Vector3.Cross(up, normal).normalized;
                float angle = Vector3.Angle(up, normal);

                Quaternion r = new Quaternion(Mathf.Sin(angle / 2 * Mathf.Deg2Rad) * YRotationNomal.x,
                                             Mathf.Sin(angle / 2 * Mathf.Deg2Rad) * YRotationNomal.y,
                                             Mathf.Sin(angle / 2 * Mathf.Deg2Rad) * YRotationNomal.z,
                                            Mathf.Cos(angle / 2 * Mathf.Deg2Rad));
                q = r * objectItem.transform.rotation;
                director = ELift_Director.Y;
                // transform.Rotate(YRotationNomal, angle, Space.World);

            }
            else if (angle11 == angles[0])
            {
                //Debug.Log("Y -");
                Vector3 normal = -other.transform.up;
                Vector3 up = objectItem.transform.up;
                Vector3 YRotationNomal = Vector3.Cross(up, normal).normalized;
                float angle = Vector3.Angle(up, normal);
                //Debug.Log(angle);
                Quaternion r = new Quaternion(Mathf.Sin(angle / 2 * Mathf.Deg2Rad) * YRotationNomal.x,
                                             Mathf.Sin(angle / 2 * Mathf.Deg2Rad) * YRotationNomal.y,
                                             Mathf.Sin(angle / 2 * Mathf.Deg2Rad) * YRotationNomal.z,
                                            Mathf.Cos(angle / 2 * Mathf.Deg2Rad));
                q = r * objectItem.transform.rotation;
                director = ELift_Director.YContrary;
            }
            else if (angle2 == angles[0])
            {
                //Debug.Log("X +");
                Vector3 normal = other.transform.up;
                Vector3 up = objectItem.transform.right;
                Vector3 YRotationNomal = Vector3.Cross(up, normal).normalized;
                float angle = Vector3.Angle(up, normal);
                Quaternion r = new Quaternion(Mathf.Sin(angle / 2 * Mathf.Deg2Rad) * YRotationNomal.x,
                                              Mathf.Sin(angle / 2 * Mathf.Deg2Rad) * YRotationNomal.y,
                                              Mathf.Sin(angle / 2 * Mathf.Deg2Rad) * YRotationNomal.z,
                                             Mathf.Cos(angle / 2 * Mathf.Deg2Rad));
                q = r * objectItem.transform.rotation;
                director = ELift_Director.X;
            }
            else if (angle22 == angles[0])
            {
                //Debug.Log("X -");
                Vector3 normal = -other.transform.up;
                Vector3 up = objectItem.transform.right;
                Vector3 YRotationNomal = Vector3.Cross(up, normal).normalized;
                float angle = Vector3.Angle(up, normal);
                Quaternion r = new Quaternion(Mathf.Sin(angle / 2 * Mathf.Deg2Rad) * YRotationNomal.x,
                                              Mathf.Sin(angle / 2 * Mathf.Deg2Rad) * YRotationNomal.y,
                                              Mathf.Sin(angle / 2 * Mathf.Deg2Rad) * YRotationNomal.z,
                                             Mathf.Cos(angle / 2 * Mathf.Deg2Rad));
                q = r * objectItem.transform.rotation;
                director = ELift_Director.XContrary;
            }
            else if (angle3 == angles[0])
            {
                //Debug.Log("Z +");
                Vector3 normal = other.transform.up;
                Vector3 up = objectItem.transform.forward;
                Vector3 YRotationNomal = Vector3.Cross(up, normal).normalized;
                float angle = Vector3.Angle(up, normal);
                Quaternion r = new Quaternion(Mathf.Sin(angle / 2 * Mathf.Deg2Rad) * YRotationNomal.x,
                                              Mathf.Sin(angle / 2 * Mathf.Deg2Rad) * YRotationNomal.y,
                                              Mathf.Sin(angle / 2 * Mathf.Deg2Rad) * YRotationNomal.z,
                                             Mathf.Cos(angle / 2 * Mathf.Deg2Rad));
                q = r * objectItem.transform.rotation;
                director = ELift_Director.Z;
            }
            else if (angle33 == angles[0])
            {
                //Debug.Log("Z -");
                Vector3 normal = -other.transform.up;
                Vector3 up = objectItem.transform.forward;
                Vector3 YRotationNomal = Vector3.Cross(up, normal).normalized;
                float angle = Vector3.Angle(up, normal);
                Quaternion r = new Quaternion(Mathf.Sin(angle / 2 * Mathf.Deg2Rad) * YRotationNomal.x,
                                              Mathf.Sin(angle / 2 * Mathf.Deg2Rad) * YRotationNomal.y,
                                              Mathf.Sin(angle / 2 * Mathf.Deg2Rad) * YRotationNomal.z,
                                             Mathf.Cos(angle / 2 * Mathf.Deg2Rad));
                q = r * objectItem.transform.rotation;
                director = ELift_Director.ZContrary;
            }
            else
                q =  objectItem.transform.rotation;
            return director;
        }

    }
}

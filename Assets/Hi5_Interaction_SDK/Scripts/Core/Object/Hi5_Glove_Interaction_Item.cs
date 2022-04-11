//#define Test_Assembly
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Hi5_Interaction_Core
{
    public struct Hi5_Position_Record
    {
        public Vector3 mMoveVector;
        public float mIntervalTime;
        public Vector3 position;
        public Hi5_Position_Record(Vector3 point, Vector3 prePoint, float cd)
        {
            mMoveVector = point - prePoint;
            mIntervalTime = cd;
            position = point;
        }
    }

	public enum E_Hand_State
	{
		ERelease = -1,
		EPinch = 2,
		EPinch2 = 3,
		ELift = 4,
		EClap = 5,
	}

	public class Hi5_Glove_Interaction_Item : MonoBehaviour

	{
		public class ContactPointClass
		{
			public ContactPoint contactPoint; 	
		}

		internal float  PlaneY = 0.0f;
		internal bool IsPokeInLoop = false; 
		float IsPokeProtectionCd = Hi5_Interaction_Const.PokeProtectionCd;


        #region object Attribute
        public EObject_Type mObjectType = EObject_Type.ECommon;
        public string nameObject;
        public int idObject;
       // public PhysicMaterial planePhysicMaterial = null;
        #endregion

        #region object MoveAttribute
        internal float AirFrictionRate;
        //public float Gravity;
		internal float PlaneFrictionRate = Hi5_Interaction_Const.PlaneFrictionRateConst;

        //public float Mass;
        #endregion 

        internal Hi5_Object_State_Manager mstatemanager = null;
        internal Hi5_Glove_Interaction_Item_Trigger trigger = null;
		
		//internal List<Hi5_Glove_Interaction_Item_Trigger> triggers = new List<Hi5_Glove_Interaction_Item_Trigger> ();
		internal List<Hi5_Interaction_Item_Collider> itemColliders = new List<Hi5_Interaction_Item_Collider> ();
        internal Hi5_Object_Lift_Collider mLifeCollider = null;

        //internal float initY = -10.0f;
        internal bool isTouchPlane = false;
        internal  Queue<Hi5_Position_Record> mQueuePositionRecord = new Queue<Hi5_Position_Record>();
        Vector3 prePositionRecord;
        public E_Object_State state = E_Object_State.EStatic;
        public Hi5ObjectMoveType moveType = Hi5ObjectMoveType.ENone;
        public bool IsChangeColor = true;
        //internal Color orgColor;
        internal Vector3 scale;
        

        #region unity system
		
		protected void Awake()
        {
            //if (gameObject.GetComponent<Hi5_Object_Property>() != null)
            //{
            //    AirFrictionRate = gameObject.GetComponent<Hi5_Object_Property>().ObjectProperty.AirFrictionRate;
            //}
            mLifeCollider = GetComponentInChildren<Hi5_Object_Lift_Collider>();

            //triggers.Clear ();
            itemColliders.Clear();
				if(gameObject.GetComponent<Hi5_Interaction_Item_Collider>() != null)
				{
					itemColliders.Add(gameObject.GetComponent<Hi5_Interaction_Item_Collider>());
					gameObject.GetComponent<Hi5_Interaction_Item_Collider>().itemObject = this;
                    gameObject.GetComponent<Hi5_Interaction_Item_Collider>().SetOrgColor();

                }
				else
				{
					Hi5_Interaction_Item_Collider[] collliders =  gameObject.GetComponentsInChildren<Hi5_Interaction_Item_Collider>();
					foreach(Hi5_Interaction_Item_Collider item in collliders)
					{
						itemColliders.Add(item);
						item.itemObject = this;
					}
				}




            mstatemanager = Hi5_Object_State_Manager.CreateState(this);
            isTouchPlane = false;
            mQueuePositionRecord.Clear();
            prePositionRecord = transform.position;
            //Y = transform.position.y;
      
            if ( IsChangeColor)
            {
                if (itemColliders.Count > 0)
                {
                    
                    // Hi5_Interaction_Item_Collider[] collliders = gameObject.GetComponentsInChildren<Hi5_Interaction_Item_Collider>();
                    foreach (Hi5_Interaction_Item_Collider item in itemColliders)
                    {
                        if (item.GetComponent<MeshRenderer>() != null)
                        {
                            item.SetOrgColor();
                            //Debug.Log("adfasdfgsagfsagsag");
                            // item.orgColor = item.GetComponent<MeshRenderer>().material.color;
                        }

                    }
                }
            }
            
            scale = transform.localScale;

        }

        internal void CleanRecord()
		{
			mQueuePositionRecord.Clear();
		}
        internal void ResetCorlor()
        {
            //if (GetComponent<MeshRenderer>() != null && IsChangeColor)
            //    GetComponent<MeshRenderer>().material.color = orgColor;
            if (itemColliders.Count > 0 && IsChangeColor)
            {
               // Hi5_Interaction_Item_Collider[] collliders = gameObject.GetComponentsInChildren<Hi5_Interaction_Item_Collider>();
                foreach (Hi5_Interaction_Item_Collider item in itemColliders)
                {
                    if (item.GetComponent<MeshRenderer>() != null)
                    {
                        // Debug.Log("adfasdfgsagfsagsag");
                        if (IsChangeColor)
                            item.GetComponent<MeshRenderer>().material.color = item.orgColor;
                    }

                }
            }
        }
        internal void ChangeColor(Color color)
        {
            if (!IsChangeColor)
                return;
            
            if (GetComponent<MeshRenderer>() != null)
                GetComponent<MeshRenderer>().material.color = color;
            if (itemColliders.Count > 0)
            {
                Hi5_Interaction_Item_Collider[] collliders = gameObject.GetComponentsInChildren<Hi5_Interaction_Item_Collider>();
                foreach (Hi5_Interaction_Item_Collider item in collliders)
                {
                    if (item.GetComponent<MeshRenderer>() != null)
                    {
                        item.GetComponent<MeshRenderer>().material.color = color;
                    }

                }
            }
        }


		internal bool IsTouchPlane()
		{
				if(itemColliders.Count == 0)
					return true;
				bool temp = false;
				foreach(Hi5_Interaction_Item_Collider item in itemColliders)
				{
					if( item.trigger != null && item.trigger.IsTrigger)
					{
						temp = true;
						break;
					}
				}
				return temp;
		}
		internal void LateUpdate ()
		{
            
            IsPokeInLoop = false;
		}

		bool isSetLayer = false;
		internal void Update()
        {
     
            if (Hi5_Layer_Set.IsResetLayer && !isSetLayer) 
			{


            }

            if (IsPokeInLoop) 
			{
				
				IsPokeProtectionCd -= Time.deltaTime;
				if (IsPokeProtectionCd < 0.0f)
					IsPokeInLoop = false;
			}
			else
			{
				IsPokeProtectionCd = Hi5_Interaction_Const.PokeProtectionCd;
			}

            //transform.localScale = scale;
			if (mstatemanager != null) {
				state = mstatemanager.State;
			}
				
			if (mstatemanager != null && mstatemanager.GetMoveState() != null)
                moveType = mstatemanager.GetMoveState().mMoveType;

		
			    bool  isTemp = false;
				foreach(Hi5_Interaction_Item_Collider item in itemColliders)
            {
                        //Debug.Log("object touch plane----------");
                    if (item.trigger != null )
					{
                        //Debug.Log("object touch plane---------- A");
                        isTemp = item.trigger.IsTrigger;
                        if (isTemp)
                        {
                           // Debug.Log("object touch plane");
                            break;
                        }  
					}
				}
			    isTouchPlane = isTemp;
				   
            if (mstatemanager != null)
            {
                mstatemanager.Update(Time.deltaTime);
            }
        }



		//internal float Y = 0.0f; 
		internal void FixedUpdate()
        {
            RecordPosition(Time.deltaTime);
            if (mstatemanager != null)
            {
                mstatemanager.FixedUpdate(Time.deltaTime);
            }		
        }


        Vector3 originalPosition;
        Quaternion originalRotation;
		protected void OnEnable()
        {
            
            originalPosition = transform.position;
            originalRotation = transform.rotation;
            Hi5_Interaction_Message.GetInstance().RegisterMessage(Reset, Hi5_MessageKey.messageObjectReset);
        }

		protected void OnDisable()
        {
          
            Hi5_Interaction_Message.GetInstance().UnRegisterMessage(Reset, Hi5_MessageKey.messageObjectReset);
        }
        #endregion

        private void Reset(string messageKey, object param1, object param2, object param3, object param4)
        {
			if (mObjectType == EObject_Type.ECommon) 
			{
				if (messageKey.CompareTo(Hi5_MessageKey.messageObjectReset) == 0)
				{
					transform.parent = Hi5_Interaction_Object_Manager.GetObjectManager().transform;
					transform.position =  originalPosition;
					transform.rotation = originalRotation;
                    transform.gameObject.GetComponent<Rigidbody>().Sleep();
#if UNITY_2017_2_OR_NEWER

#else
                    transform.gameObject.GetComponent<Rigidbody>().WakeUp();
#endif
                    mstatemanager.ChangeState(E_Object_State.EMove);
                    mstatemanager.GetMoveState().mMoveType = Hi5ObjectMoveType.EFree;
                    //if (Hi5_Interaction_Const.TestChangeState)
                    {
						Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance(idObject, mObjectType, EHandType.EHandLeft, EEventObjectType.EStatic);
						Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
					}
					SetIsKinematic(false);
                    //if (!Hi5_Interaction_Const.TestModifyConstraints)
                    //    SetIsLockYPosition(true);
					SetUseGravity(true);
				}
			}
       }

        internal void ChangeState(E_Object_State state)
        {
            if (state == E_Object_State.EMove || state == E_Object_State.EFlyLift || state == E_Object_State.EPinch)
            {
                if (mObjectType == EObject_Type.EButton)
                    return;
            }
            mstatemanager.ChangeState(state);
        }

		internal void CalculateThrowMove(Transform handPalm,Hi5_Glove_Interaction_Hand hand)
        {
			mstatemanager.CalculateThrowMove(mQueuePositionRecord, handPalm,hand);
            
        }


        private void RecordPosition(float deltaTime)
        {
            if (mQueuePositionRecord.Count > (Hi5_Interaction_Const.ObjectPinchRecordPositionCount - 1))
            {
                mQueuePositionRecord.Dequeue();
            }
            Hi5_Position_Record record = new Hi5_Position_Record(transform.position, prePositionRecord, deltaTime);
            mQueuePositionRecord.Enqueue(record);
            prePositionRecord = transform.position;
        }

		internal ContactPointClass contactPointTemp = null;
		//bool isColliderStaticObjectGrasp = false;

		internal bool IsTouchStaticObject(out int ObjctID)
		{
            ObjctID = -1000;
            bool  isTriggerObjectId = false;
				foreach(Hi5_Interaction_Item_Collider item in itemColliders)
				{
					if (item.trigger != null && item.trigger.IsTriggerObject) 
					{
						Hi5_Glove_Interaction_Item itemTemp = Hi5_Interaction_Object_Manager.GetObjectManager ().GetItemById (item.trigger.mTriggerObjectId);
						if (itemTemp != null) 
						{
							if (itemTemp.state == E_Object_State.EStatic || itemTemp.state == E_Object_State.EFlyLift || itemTemp.state == E_Object_State.EMove) 
							{
                                 ObjctID = itemTemp.idObject;
                                isTriggerObjectId =  true;
								break;
							}
							
						} 
					}
				}
                return isTriggerObjectId;
		}
		
		//#if Test_Assembly
		internal void OnItemTriggerEnter(Collider collision)
		{
			//Debug.Log ("Hi5_Glove_Interaction_Item OnItemTriggerEnter ENTER");
			if (collision.gameObject.layer == Hi5_Interaction_Const.PlaneLayer()  )
			{
				if (mObjectType == EObject_Type.ECommon)
				{
					//Debug.Log ("Hi5_Glove_Interaction_Item OnItemTriggerEnter");
					mstatemanager.StopThrowMove(true);
					//if (Hi5_Interaction_Const.TestPlanePhycis) {


					//}
					//else
					//	transform.rotation = Quaternion.Euler(0.0f, transform.eulerAngles.y, 0.0f);
					{
						Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance(idObject, mObjectType, EHandType.EHandLeft, EEventObjectType.EStatic);
						Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
					}
				}
			}

			if (collision.gameObject.layer == Hi5_Interaction_Const.ObjectGrasp ())
			{
				if ((collision.gameObject.GetComponent<Hi5_Glove_Interaction_Item> () != null
				   && collision.gameObject.GetComponent<Hi5_Glove_Interaction_Item> ().state == E_Object_State.EStatic)
				   || (collision.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item> () != null
						&& collision.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item> ().state == E_Object_State.EStatic)) 
				{
					if (mObjectType == EObject_Type.ECommon)
					{
						//Debug.Log ("Hi5_Glove_Interaction_Item OnItemTriggerEnter");
						mstatemanager.StopThrowMove(true);
						//transform.rotation = Quaternion.Euler(0.0f, transform.eulerAngles.y, 0.0f);
						//if (Hi5_Interaction_Const.TestChangeState)
						{
							Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance(idObject, mObjectType, EHandType.EHandLeft, EEventObjectType.EStatic);
							Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
						}
					}
				}
			}
		}


        internal void SetIsKinematic(bool param)
        {
//			#if Test_Assembly
//			foreach(Hi5_Interaction_Item_Collider item in itemColliders)
//			{
//				item.GetComponent<Rigidbody>().isKinematic = param;
//			}
//			#else
				GetComponent<Rigidbody>().isKinematic = param;
			//#endif
           
        }

        internal void SetUseGravity(bool param)
        {
            //if (!Hi5_Interaction_Const.TestModifyConstraints)
            //{
              //  GetComponent<Rigidbody>().useGravity = param;

            //}
            
        }

        //internal void SetAllLock()
        //{
        //	GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        //}
        RigidbodyConstraints preRotationConstraints;
        internal void SetIsLockRotation(bool isLock)
        {
            if (isLock)
            {
                preRotationConstraints = GetComponent<Rigidbody>().constraints;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;// | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            }
            else
                GetComponent<Rigidbody>().constraints = preRotationConstraints;
        }
        RigidbodyConstraints prePositionConstraints;
        internal void SetIsLockPosition(bool isLock)
        {
            if (isLock)
            {
                prePositionConstraints = GetComponent<Rigidbody>().constraints;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;// | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            }
            else
                GetComponent<Rigidbody>().constraints = prePositionConstraints;
        }

        internal void SetIsLockYPosition(bool param)
        {
            //if (!Hi5_Interaction_Const.TestModifyConstraints)
            //{
            //    if (param)
            //    {
            //        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            //    }
            //    else
            //    {
            //        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            //    }
            //}

        }

        internal bool IsItemTouchPlane()
        {
            bool isTouchPlane = false;
            foreach (Hi5_Interaction_Item_Collider item in itemColliders)
            {
                if (item.trigger != null && item.trigger.IsTrigger)
                {
                    isTouchPlane = true;
                }
            }
             return  isTouchPlane;
        }


		internal void CleanLock()
		{
			GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
		}
        internal void ClockRotation()
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        } 

        internal void NotifyPokeEvent(Collision collision)
		{
			if (collision.gameObject.GetComponent<Hi5_Hand_Collider_Visible_Finger> () != null) {
				{
					Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance (idObject,
						                                               mObjectType,
						                                               collision.gameObject.GetComponent<Hi5_Hand_Collider_Visible_Finger> ().mFinger.mHand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
						                                               EEventObjectType.EPoke);
					Hi5InteractionManager.Instance.GetMessage ().DispenseMessage (Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
				}

				{
					Hi5_Glove_Interaction_Hand_Event_Data data = Hi5_Glove_Interaction_Hand_Event_Data.Instance (idObject,
						                                             collision.gameObject.GetComponent<Hi5_Hand_Collider_Visible_Finger> ().mFinger.mHand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
						                                             EEventHandType.EPoke);
					Hi5InteractionManager.Instance.GetMessage ().DispenseMessage (Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageHandEvent, (object)data, null);
				}
			}
			else if (collision.gameObject.GetComponent<Hi5_Hand_Collider_Visible_Thumb_Finger> () != null) {
				{
					Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance (idObject,
						mObjectType,
						collision.gameObject.GetComponent<Hi5_Hand_Collider_Visible_Thumb_Finger> ().mFinger.mHand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
						EEventObjectType.EPoke);
					Hi5InteractionManager.Instance.GetMessage ().DispenseMessage (Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
				}

				{
					Hi5_Glove_Interaction_Hand_Event_Data data = Hi5_Glove_Interaction_Hand_Event_Data.Instance (idObject,
						collision.gameObject.GetComponent<Hi5_Hand_Collider_Visible_Thumb_Finger> ().mFinger.mHand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
						EEventHandType.EPoke);
					Hi5InteractionManager.Instance.GetMessage ().DispenseMessage (Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageHandEvent, (object)data, null);
				}
			}
		}


		internal void NotifyPokeEvent(Collider other)
		{
			
			{
				Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance (idObject,
					mObjectType,
					other.gameObject.GetComponent<Hi5_Glove_Collider_Finger> ().mFinger.mHand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
					EEventObjectType.EPoke);
				Hi5InteractionManager.Instance.GetMessage ().DispenseMessage (Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
			}

			{
				Hi5_Glove_Interaction_Hand_Event_Data data = Hi5_Glove_Interaction_Hand_Event_Data.Instance (idObject,
					other.gameObject.GetComponent<Hi5_Glove_Collider_Finger> ().mFinger.mHand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
					EEventHandType.EPoke);
				Hi5InteractionManager.Instance.GetMessage ().DispenseMessage (Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageHandEvent, (object)data, null);
			}
		}

        
		internal bool IsLiftTrigger()
		{
			foreach(Hi5_Interaction_Item_Collider item in itemColliders)
			{
				foreach (Collider itemTemp in item.m_Triggers)
				{
					if (itemTemp.gameObject.GetComponent<Hi5_Glove_Collider_Finger> () != null) {
						return true;
					}
					if (itemTemp.gameObject.GetComponent<Hi5_Glove_Collider_Palm> () != null) {
						return true;
					}

				}
			}
			return false;
		
		}

		internal List<Collider> GetTriggers()
		{
			List<Collider> temp = new List<Collider>();
			foreach(Hi5_Interaction_Item_Collider item in itemColliders)
			{
				
				if(item.m_Triggers != null && item.m_Triggers.Count>0)
				{
					temp = temp.Union(item.m_Triggers).ToList();
				}
			}
			return temp;
		
		}
       
	

		internal float GetHeight()
		{
			float Height = 1.0f;
			if (GetComponent<BoxCollider> () != null) {
				Height = GetComponent<BoxCollider> ().size.y;
			}
			else if (GetComponent<SphereCollider> () != null) {
				Height = GetComponent<SphereCollider> ().radius*2;
			}

			else if (GetComponent<CapsuleCollider> () != null) {
				Height = GetComponent<CapsuleCollider> ().height;
			}
			return Height;
		}

		//判断两个bundle是否相交
		internal bool IsSurround(Collider colliderOther)
		{
		
			bool valueBack = false;
			foreach(Hi5_Interaction_Item_Collider item in itemColliders)
			{

                //if (Hi5_Interaction_Const.TestAssembly)
                //{

                //}
                //else
                //{
                    Collider SelfCollider = item.GetComponent<Collider>();
                    if (SelfCollider == null)
                    {
                        foreach (Hi5_Interaction_Item_Collider item_Collider in itemColliders)
                        {
                            if (item_Collider.GetComponent<Collider>().bounds.Intersects(colliderOther.bounds))
                            {
                                valueBack = true;
                                return valueBack;
                            }
                        }
                    }
                    else
                    {
                        if (SelfCollider != null && colliderOther != null)
                        {
                            if (SelfCollider.bounds.Intersects(colliderOther.bounds))
                            {
                                valueBack = true;
                                break;
                            }

                        }
                    }
               // }
			}
			return valueBack;
		
		}
        internal int ColliderNailNum = 0;
    }
}

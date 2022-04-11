using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
	public class Hi5_Interaction_Item_Collider : Hi5_Glove_Interaction_Collider 
	{
		internal Hi5_Glove_Interaction_Item itemObject = null;
        internal Hi5_Glove_Interaction_Item_Trigger trigger = null;
        internal Color orgColor;

        private void Awake()
        {

        }
        internal void SetOrgColor()
        {
            if (gameObject.GetComponent<MeshRenderer>() != null && itemObject != null && itemObject.IsChangeColor)
            {
               // Debug.Log("color"+gameObject.GetComponent<MeshRenderer>().material.color);
                orgColor = gameObject.GetComponent<MeshRenderer>().material.color;
            }
        }
        
         void Update()
		{
			if (itemObject != null && trigger == null) {
				trigger = gameObject.GetComponentInChildren<Hi5_Glove_Interaction_Item_Trigger> ();
                if(trigger != null)
				    trigger.itemObject = itemObject;
			}
			if (itemObject != null && trigger != null) {
				trigger.itemObject = itemObject;
			}
		}


		override protected void OnCollisionEnter(Collision collision)
		{
			
			base.OnCollisionEnter(collision);
			if (itemObject.mObjectType == EObject_Type.ECommon)
			{
				if ((collision.gameObject.layer == Hi5_Interaction_Const.PlaneLayer())
					||((collision.gameObject.layer == Hi5_Interaction_Const.ObjectGrasp() 
						&& (collision.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null
							&& collision.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().state == E_Object_State.EStatic) 
						|| (collision.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null
							&& collision.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().state == E_Object_State.EStatic) ))
				)
				{
					
					if (itemObject.state == E_Object_State.EPinch)
						return;
					//Debug.Log ("Hi5_Interaction_Item_Collider OnCollisionEnter");
					itemObject.mstatemanager.StopThrowMove(true);
					Vector3 separationVector = Vector3.zero;
					ContactPoint[] contactPoints = collision.contacts;
					if (contactPoints != null && contactPoints.Length > 0) {
						itemObject.contactPointTemp = new Hi5_Glove_Interaction_Item.ContactPointClass (); 
						itemObject.contactPointTemp.contactPoint= contactPoints [0];

						float separation = itemObject.contactPointTemp.contactPoint.separation;
						Vector3 contactPointNormal =itemObject.contactPointTemp.contactPoint.normal;
						contactPointNormal.Normalize();

						separationVector = (contactPointNormal) * separation;
						itemObject.transform.position = new Vector3(itemObject.transform.position.x, itemObject.transform.position.y+Mathf.Abs(separationVector.y), itemObject.transform.position.z);

					}
					itemObject.PlaneY = itemObject.transform.position.y;
					//transform.rotation = initRotation;
					//if (!Hi5_Interaction_Const.TestPlanePhycis)
					//	itemObject.transform.rotation = Quaternion.Euler(0.0f, transform.eulerAngles.y, 0.0f);
					//if (Hi5_Interaction_Const.TestChangeState)
					{
						Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance(itemObject.idObject, itemObject.mObjectType, EHandType.EHandLeft, EEventObjectType.EStatic);
						Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
					}

				}
				if (collision.gameObject.layer == Hi5_Interaction_Const.OtherFingerTailLayer() 
					/*|| collision.gameObject.layer == Hi5_Interaction_Const.ThumbTailLayer()*/)
				{
					
					if (collision.gameObject.GetComponent<Hi5_Hand_Collider_Visible_Finger>())
					{
						if (itemObject.mObjectType == EObject_Type.ECommon)
						{
							if (itemObject.state == E_Object_State.EStatic || itemObject.state == E_Object_State.EFlyLift)
							{
								if (!itemObject.IsPokeInLoop) 
								{
									ContactPoint[] contactPoints = collision.contacts;
									if (contactPoints != null && contactPoints.Length > 0)
									{
										Vector3 normal =  contactPoints [0].normal;
										float angle = Vector3.Angle(Hi5_Interaction_Object_Manager.GetObjectManager ().transform.up, normal);

										if (Mathf.Abs(angle)>25.0f) 
										{

											itemObject.NotifyPokeEvent (collision);
											itemObject.IsPokeInLoop = true;

										}
									}
								}
							}	
							{
								if (itemObject.mstatemanager.State == E_Object_State.EStatic 
									|| (itemObject.mstatemanager.State == E_Object_State.EMove && itemObject.mstatemanager.GetMoveState().mMoveType == Hi5ObjectMoveType.EPlaneMove))
								{
									Hi5_Hand_Visible_Hand handTemp = collision.gameObject.GetComponent<Hi5_Hand_Collider_Visible_Finger>().mHand;
									
									itemObject.ChangeState(E_Object_State.EMove);
                                    if (itemObject.mstatemanager != null)
                                        itemObject.mstatemanager.SetPlaneMove(collision);
                                    {
										Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance(itemObject.idObject, itemObject.mObjectType, handTemp.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight, EEventObjectType.EMove);
										Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
									}
								}
							}
						}
					}
				}
			}

		}

        

		override protected void OnTriggerEnter(Collider other)
		{
			base.OnTriggerEnter(other);
            if (Hi5_Interaction_Const.TestPinchOpenCollider)
            {
                if (other.gameObject.GetComponent<Hi5_Hand_Nail_Collider>() != null)
                {
                    itemObject.ColliderNailNum++;
                    if (itemObject.ColliderNailNum > 0)
                    {
                        if (other.gameObject.GetComponent<Hi5_Hand_Nail_Collider>().mHand != null)
                            other.gameObject.GetComponent<Hi5_Hand_Nail_Collider>().mHand.AddNailColliderCount();

                        //if (itemObject.state == E_Object_State.EStatic 
                        //    || (itemObject.state == E_Object_State.EMove && itemObject.moveType == Hi5ObjectMoveType.EPlaneMove))
                        //{
                        //    itemObject.SetIsKinematic(true);
                        //    itemObject.GetComponent<Rigidbody>().Sleep();
                        //} 
                    }
                }
                   
            }
               
            if (itemObject.mObjectType == EObject_Type.EButton || itemObject.mObjectType == EObject_Type.ECommon)
			{
				if (other.gameObject.layer == Hi5_Interaction_Const.OtherFingerTailLayer())
				{
					if (other.gameObject.GetComponent<Hi5_Glove_Collider_Finger>() != null
						&& other.gameObject.GetComponent<Hi5_Glove_Collider_Finger>().mFinger != null
						&& other.gameObject.GetComponent<Hi5_Glove_Collider_Finger>().mFinger.mHand != null)
					{

						if (itemObject.mObjectType == EObject_Type.EButton)
						{
							if (itemObject.state == E_Object_State.EStatic )
							{
								itemObject.ChangeState(E_Object_State.EPoke);
								itemObject.NotifyPokeEvent (other);
							}	
						}
						else
						{
							if (itemObject.state == E_Object_State.EStatic || itemObject.state == E_Object_State.EFlyLift)
							{


							}	
						}
					}
				}
			}
		}

		override protected void OnCollisionStay(Collision collision)
        {
            base.OnCollisionStay(collision);
           

            if (itemObject.mObjectType == EObject_Type.ECommon)
            {

				if (/*collision.gameObject.layer == Hi5_Interaction_Const.ThumbTailLayer()
					|| */collision.gameObject.layer == Hi5_Interaction_Const.OtherFingerTailLayer()
					/*|| collision.gameObject.layer == Hi5_Interaction_Const.PalmRigidbodyLayer()*/)
                {
					if (itemObject.mstatemanager == null)
						return;
                    // if (collision.gameObject.GetComponent<Hi5_Hand_Collider_Visible_Finger>())
                    {
                      
						//if (Hi5_Interaction_Const.TestChangeState)
						{
							if (itemObject.mstatemanager.State == E_Object_State.EStatic
								|| (itemObject.mstatemanager.State == E_Object_State.EMove && itemObject.mstatemanager.GetMoveState ().mMoveType == Hi5ObjectMoveType.EPlaneMove)) 
							{
								if (/*collision.gameObject.layer == Hi5_Interaction_Const.ThumbTailLayer() ||*/ collision.gameObject.layer == Hi5_Interaction_Const.OtherFingerTailLayer())
								{
									if (collision.gameObject.GetComponent<Hi5_Hand_Collider_Visible_Thumb_Finger>() == null)
										return;
									Hi5_Hand_Visible_Finger finger = collision.gameObject.GetComponent<Hi5_Hand_Collider_Visible_Thumb_Finger>().mFinger;
									if (finger && (finger is Hi5_Hand_Visible_Thumb_Finger))
									{
										if (!(finger as Hi5_Hand_Visible_Thumb_Finger).IsMoveTowardHand())
										{
											
											Hi5_Hand_Visible_Hand handTemp = collision.gameObject.GetComponent<Hi5_Hand_Collider_Visible_Finger> ().mHand;
											itemObject.ChangeState(E_Object_State.EMove);
                                            if (itemObject.mstatemanager != null)
                                                itemObject.mstatemanager.SetPlaneMove(collision);
                                            {
												Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance(itemObject.idObject, 
													itemObject.mObjectType, 
													handTemp.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
													EEventObjectType.EMove);
												Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
											}
										}
									}
								}
								else
								{

								}
							}
						}
                    }
                }
            }
        }
	
		override protected void OnTriggerExit(Collider other)
		{

			base.OnTriggerExit(other);
            if (Hi5_Interaction_Const.TestPinchOpenCollider)
            {
                if (other.gameObject.GetComponent<Hi5_Hand_Nail_Collider>() != null)
                {
                    itemObject.ColliderNailNum--;
                    if (itemObject.ColliderNailNum <= 0)
                    {
                        if (other.gameObject.GetComponent<Hi5_Hand_Nail_Collider>().mHand != null)
                            other.gameObject.GetComponent<Hi5_Hand_Nail_Collider>().mHand.RemoveNailColliderCount();
                        //if (itemObject.state == E_Object_State.EMove
                        //  || itemObject.state == E_Object_State.EPoke 
                        //  || itemObject.state == E_Object_State.EStatic)
                        //{
                        //    itemObject.SetIsKinematic(false);
                        //}
                    }
                }
                   
            }
            
            if (itemObject.mObjectType == EObject_Type.EButton) {
				if (/*other.gameObject.layer == Hi5_Interaction_Const.ThumbTailLayer ()
				    ||*/ other.gameObject.layer == Hi5_Interaction_Const.OtherFingerTailLayer ()) {

					if (other.gameObject.GetComponent<Hi5_Glove_Collider_Finger> () != null
						&& other.gameObject.GetComponent<Hi5_Glove_Collider_Finger> ().mFinger != null
						&& other.gameObject.GetComponent<Hi5_Glove_Collider_Finger> ().mFinger.mHand != null) {
						if (itemObject.state == E_Object_State.EPoke) {

                            //Debug.Log("EPoke");
                            itemObject.ChangeState (E_Object_State.EStatic);
							//if (Hi5_Interaction_Const.TestChangeState) 
							{
								Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance (itemObject.idObject,
									itemObject.mObjectType,
									other.gameObject.GetComponent<Hi5_Glove_Collider_Finger> ().mFinger.mHand.m_IsLeftHand ? EHandType.EHandLeft : EHandType.EHandRight,
									EEventObjectType.EStatic);
								Hi5InteractionManager.Instance.GetMessage ().DispenseMessage (Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
							}

						}	
					}
				}
			}
			else if (itemObject.mObjectType == EObject_Type.ECommon)
			{
				if (/*other.gameObject.layer == Hi5_Interaction_Const.ThumbTailLayer ()
				    ||*/ other.gameObject.layer == Hi5_Interaction_Const.OtherFingerTailLayer ()) {

					if (other.gameObject.GetComponent<Hi5_Glove_Collider_Finger> () != null
						&& other.gameObject.GetComponent<Hi5_Glove_Collider_Finger> ().mFinger != null
						&& other.gameObject.GetComponent<Hi5_Glove_Collider_Finger> ().mFinger.mHand != null) {
						itemObject.IsPokeInLoop = false;
					}
				}
			}
		}
	}
}

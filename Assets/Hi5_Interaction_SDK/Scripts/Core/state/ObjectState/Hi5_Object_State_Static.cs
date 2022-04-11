using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public class Hi5_Object_State_Static : Hi5_Object_State_Base
    {
        internal Vector3 prePosition = Vector3.zero;
        internal Quaternion preRotaion = Quaternion.identity;
		internal float staticStartY = 0.0f;
		internal Vector3 staticPosition = Vector3.zero;
        internal bool isTouchObject = false;
        override public void Start()
        {
			staticStartY = ObjectItem.transform.position.y;
			staticPosition = ObjectItem.transform.position;
            //ObjectItem.ChangeColor (ObjectItem.orgColor);
            //if (Hi5_Interaction_Const.TestModifyConstraints)
            {
                if (ObjectItem.mObjectType == EObject_Type.ECommon)
                {
                   

                    if (ObjectItem.GetComponent<Hi5_Object_Property>() != null
                        && ObjectItem.GetComponent<Hi5_Object_Property>().ObjectProperty.StaticProperty != null)
                    {
                        ObjectItem.SetIsKinematic(false);
                        ObjectItem.SetUseGravity(true);
                        ObjectItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                        ObjectItem.GetComponent<Hi5_Object_Property>().SetRotation(ObjectItem.GetComponent<Hi5_Object_Property>().ObjectProperty.StaticProperty.ConstraintsFreezeRotation,
                           ObjectItem.GetComponent<Hi5_Object_Property>().ObjectProperty.StaticProperty.ConstraintsFreezeRotation, 
                           ObjectItem.GetComponent<Hi5_Object_Property>().ObjectProperty.StaticProperty.ConstraintsFreezeRotation);
                    }
                    else
                    {
                        ObjectItem.SetIsKinematic(false);
                        ObjectItem.SetUseGravity(true);
                        ObjectItem.CleanLock();
                    }
                }
                else
                {
                    ObjectItem.SetIsKinematic(true);
                }
            }
              
         }

        internal void ResetPreTransform()
        {
            ObjectItem.transform.position =  new Vector3(ObjectItem.transform.position.x, prePosition.y, ObjectItem.transform.position.z);
            ObjectItem.transform.rotation = preRotaion;
        }

		bool IsTouchPlane()
		{
			if (ObjectItem.trigger != null)
			{
				Vector3 downPosition = new Vector3(ObjectItem.transform.position.x,
													ObjectItem.transform.position.y - 0.2f,
													ObjectItem.transform.position.z);	
				Ray ray = new Ray( ObjectItem.transform.position, ObjectItem.transform.position - downPosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
					if (hit.transform == ObjectItem.trigger.planeTransform) {
						return true;
					} else {
						return false;
					}
				}
				else {
					return true;
				}
			} 
			else
				return true;
		}

		float detachCd = 0.4f;
		bool StartY = false;
		float Y = 0.0f;
        override public void Update(float deltaTime)
        {
			//if (Hi5_Interaction_Const.TestChangeState1)
			{
				if (ObjectItem.transform.parent != Hi5_Interaction_Object_Manager.GetObjectManager ().transform) {
					///Debug.Log ("static palm parent");
					ObjectItem.transform.parent = Hi5_Interaction_Object_Manager.GetObjectManager ().transform;
				}
			}

			{
				if (ObjectItem.mObjectType == EObject_Type.ECommon)
				{
                    int touchIdtemp;
					if (((!ObjectItem.IsTouchPlane ()) && (!ObjectItem.IsTouchStaticObject (out touchIdtemp))))
					{
                        //静止转自由下落
                        if (!StartY)
						{
							StartY = true;
							Y = ObjectItem.transform.position.y;
						}
						else
						{
							if (StartY)
							{
								if (Mathf.Abs (Y - ObjectItem.transform.position.y) > 0.2f)
								{
                                    ObjectItem.ChangeState(E_Object_State.EMove);
                                    ObjectItem.mstatemanager.GetMoveState ().SetFreeMove (null);
									
									{
										Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance (ObjectItem.idObject,
											ObjectItem.mObjectType,
											EHandType.ENone,
											EEventObjectType.EMove);
										Hi5InteractionManager.Instance.GetMessage ().DispenseMessage (Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
									}
									return;
								}
							}
						}
					}
					

					//if (Hi5_Interaction_Const.TestPlanePhycis)
                    {
						//Debug.Log ("static move ");
						//静止转平面移动
						float distance = Vector3.Distance(staticPosition,ObjectItem.transform.position);
						if (distance > 0.008f && ObjectItem.IsTouchPlane()) 
						{
							
							//ObjectItem.ChangeColor (Color.green);
							ObjectItem.ChangeState(E_Object_State.EMove);
                            if (ObjectItem.mstatemanager != null)
                                ObjectItem.mstatemanager.SetPlaneMove(null);
                            {
								Hi5_Glove_Interaction_Object_Event_Data data = Hi5_Glove_Interaction_Object_Event_Data.Instance(ObjectItem.idObject, ObjectItem.mObjectType, EHandType.EHandLeft, EEventObjectType.EMove);
								Hi5InteractionManager.Instance.GetMessage().DispenseMessage(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent, (object)data, null);
							}
						}
					}
				}
               
            }


            if (ObjectItem.mObjectType == EObject_Type.EButton)
            {
                //if (!Hi5_Interaction_Const.TestModifyConstraints)
                //{
                //    ObjectItem.SetIsKinematic(true);
                //}
                //if (!Hi5_Interaction_Const.TestModifyConstraints)
                //    ObjectItem.SetIsLockYPosition(true);
            }
            int touchId;
           // Debug.Log("IsTouchStaticObject out");
            //if (ObjectItem.IsTouchStaticObject(out touchId) && !ObjectItem.IsItemTouchPlane())
            //{
            //   // Debug.Log("IsTouchStaticObject");
            //    if (touchId != -1000)
            //    {
            //       // Debug.Log("IsTouchStaticObject 1");
            //        Hi5_Glove_Interaction_Item itemTemp = Hi5_Interaction_Object_Manager.GetObjectManager().GetItemById(touchId);
            //        if (itemTemp.state == E_Object_State.EFlyLift || itemTemp.state == E_Object_State.EPinch || itemTemp.state == E_Object_State.EStatic)
            //        {
            //           // Debug.Log("IsTouchStaticObject 2");
            //            //itemTemp.gameObject.GetComponent<Collider>().material = itemTemp.planePhysicMaterial;
            //            if (itemTemp.itemColliders.Count>0)
            //            {
            //               /// Debug.Log("IsTouchStaticObject 3");
            //                foreach (Hi5_Interaction_Item_Collider itemCollider in itemTemp.itemColliders)
            //                {
            //                    if (itemCollider.GetComponent<BoxCollider>() != null)
            //                    {
            //                        float angle1 = Vector3.Angle(Hi5_Interaction_Object_Manager.GetObjectManager().transform.up, -itemCollider.transform.up);
            //                        float angle2 = Vector3.Angle(Hi5_Interaction_Object_Manager.GetObjectManager().transform.up, itemCollider.transform.right);
            //                        float angle3 = Vector3.Angle(Hi5_Interaction_Object_Manager.GetObjectManager().transform.up, itemCollider.transform.forward);
            //                        //Debug.Log("angle" + angle1);
            //                        bool isFollow = false;
            //                        if (angle1 < Hi5_Interaction_Const.FlyLiftPalmAngle || (180.0f - angle1) < Hi5_Interaction_Const.FlyLiftPalmAngle)
            //                        {
            //                            isFollow = true;
            //                        }
            //                        else if (angle2 < Hi5_Interaction_Const.FlyLiftPalmAngle || (180.0f - angle2) < Hi5_Interaction_Const.FlyLiftPalmAngle)
            //                        {
            //                            isFollow = true;
            //                        }
            //                        else if (angle3 < Hi5_Interaction_Const.FlyLiftPalmAngle || (180.0f - angle3) < Hi5_Interaction_Const.FlyLiftPalmAngle)
            //                        {
            //                            isFollow = true;
            //                        }
            //                        if (isFollow /*&& ObjectItem.transform.parent != Hi5_Interaction_Object_Manager.GetObjectManager().transform.parent*/)
            //                        {
            //                            //ObjectItem.GetComponent<Rigidbody>().useGravity = false;
            //                           // ObjectItem.transform.parent = itemCollider.transform;
            //                            //ObjectItem.GetComponent<Rigidbody>().Sleep();
            //                        }
                                        
            //                    }
            //                }
            //            }
            //        }
            //        ObjectItem.GetComponent<Rigidbody>().Sleep();
            //    }
            //}
        }

        override public void End()
        {
            prePosition = ObjectItem.transform.position;
            preRotaion = ObjectItem.transform.rotation;
			StartY = false;
			Y = 0.0f;
            isTouchObject = false;
        }

        override public  void FixedUpdate(float deltaTime)
        {

        }
    }
}

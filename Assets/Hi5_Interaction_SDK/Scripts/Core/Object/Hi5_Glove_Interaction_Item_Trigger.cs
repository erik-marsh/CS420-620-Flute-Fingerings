using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
	
    public class Hi5_Glove_Interaction_Item_Trigger : MonoBehaviour
    {
        internal  Hi5_Glove_Interaction_Item itemObject = null;
        public bool IsTrigger = false;
		public bool IsTriggerObject = false;
		public int mTriggerObjectId = -1000;
		internal Transform planeTransform = null;

		private float TouchObjectY = 0.0f;
		bool IsPreNoTouchObject = false;

		private float TouchPlaneY = 0.0f;
		bool IsPreNoTouchPlane = false;
        virtual protected void OnTriggerEnter(Collider other)
        {
			if (other.gameObject.layer == Hi5_Interaction_Const.PlaneLayer())
            {
				planeTransform = other.gameObject.transform;
                IsTrigger = true;

				TouchPlaneY = transform.position.y;
				IsPreNoTouchPlane = false;
				//Debug.Log("Hi5_Glove_Interaction_Item_Trigger IsTigger true");
				//if (!Hi5_Interaction_Const.TestFlyMoveUsedGravity)
                {
                    if (itemObject != null)
                    {
                        itemObject.OnItemTriggerEnter(other);
                    }
                }
				//Debug.Log("TouchPlaneY ="+TouchPlaneY);
            }

			if (other.gameObject.layer == Hi5_Interaction_Const.ObjectTriggerLayer()) {

				if (other.gameObject.GetComponent<Hi5_Glove_Interaction_Item_Trigger> () != null && other.gameObject.GetComponent<Hi5_Glove_Interaction_Item_Trigger> ().itemObject !=null  ) {
					mTriggerObjectId = other.gameObject.GetComponent<Hi5_Glove_Interaction_Item_Trigger> ().itemObject.idObject;
					IsTriggerObject = true;
					IsPreNoTouchObject = false;
					//Debug.Log("IsTiggerObject true");

					TouchObjectY = transform.position.y;
					//Debug.Log("TouchObjectY ="+TouchObjectY);
				}

			}
        }

        virtual protected void OnTriggerStay(Collider other)
        {
			if (other.gameObject.layer == Hi5_Interaction_Const.PlaneLayer())
            {
				planeTransform = other.gameObject.transform;
                IsTrigger = true;
				//TouchPlaneY = transform.position.y;
				IsPreNoTouchPlane = false;
				//Debug.Log("IsTigger true");
            }
			if (other.gameObject.layer == Hi5_Interaction_Const.ObjectTriggerLayer()) {
				if (other.gameObject.GetComponent<Hi5_Glove_Interaction_Item_Trigger> () != null && other.gameObject.GetComponent<Hi5_Glove_Interaction_Item_Trigger> ().itemObject !=null) {
                    mTriggerObjectId = other.gameObject.GetComponent<Hi5_Glove_Interaction_Item_Trigger> ().itemObject.idObject;
					IsTriggerObject = true;
					IsPreNoTouchObject = false;
					//TouchObjectY = transform.position.y;
					//Debug.Log("IsTiggerObject true");
				}
			}
            //Debug.Log("Hi5_Glove_Interaction_Item_Trigger OnTriggerStay true");
            //Debug.Log("IsTrigger true");
        }

        virtual protected void OnTriggerExit(Collider other)
        {
			if (other.gameObject.layer == Hi5_Interaction_Const.PlaneLayer())
            {
				IsPreNoTouchPlane = true;
				//Debug.Log("IsPreNoTouchPlane true");
               //IsTrigger = false;
            }
			if (other.gameObject.layer == Hi5_Interaction_Const.ObjectTriggerLayer()) {
				if (other.gameObject.GetComponent<Hi5_Glove_Interaction_Item_Trigger> () != null) {
					IsPreNoTouchObject = true;
					//mTiggerObjectId = -1000;
					//IsTiggerObject = false;
					//Debug.Log("IsTiggerObject true");
					//TouchObjectY = transform.position.y;

				}
			}
            //Debug.Log("Hi5_Glove_Interaction_Item_Trigger OnTriggerStay false");
            //Debug.Log("IsTrigger false");
        }

		bool isSetLayer = false;
		void Update()
		{
			//if (Hi5_Layer_Set.IsResetLayer  && !isSetLayer) 
			//{

			//	gameObject.layer = LayerMask.NameToLayer ("Hi5ObjectTigger");
			//	isSetLayer = true;
			//}
			if (IsPreNoTouchObject && itemObject != null)
			{
				//Debug.Log("UpdateOther 1");
				//Debug.Log("TouchObjectY ="+TouchObjectY);
				float disY = TouchObjectY - transform.position.y;
				//Debug.Log("IsPreNoTouchObject disY ="+disY);
				if ((Mathf.Abs(disY)) > 0.001f) {
					//Debug.Log("UpdateOther 2");
					mTriggerObjectId = -1000;
					IsTriggerObject = false;
					IsPreNoTouchObject = false;
					//Debug.Log("IsTiggerObject false");
				}
			}
			if (IsPreNoTouchPlane  && itemObject != null) 
				//if (IsTrigger)
			{
				//Debug.Log("UpdateOther 3");
				float disY = TouchPlaneY - transform.position.y;
				//Debug.Log("IsPreNoTouchPlane disY ="+disY);
				if ((Mathf.Abs(disY)) > 0.001f) {
					//Debug.Log("UpdateOther 4");
					//Debug.Log("IsTiggerPlane false");
					IsTrigger = false;
					IsPreNoTouchPlane = false;
				}
			}

		}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public class Hi5_Hand_Palm : Hi5_Glove_Interaction_Collider
    {
        internal Hi5_Hand_Visible_Hand mHand;
        public Transform transformChild;
        override protected void OnTriggerEnter(Collider other)
        {
            //if (other.gameObject.GetComponent<Hi5_Glove_Interraction_Item>())
            //{
            //    other.gameObject.GetComponent<Hi5_Glove_Interraction_Item>().ChangeState(E_Object_State.EStatic);
            //}
        }

		internal void OpenBoxCollider(bool IsOpen)
		{
            transformChild.gameObject.SetActive (IsOpen);
		}
		bool isSetLayer = false;
        internal bool IsOpenCollider = true;
		void Update()
		{

            Dictionary<int, Hi5_Glove_Interaction_Item> dic=  Hi5_Interaction_Object_Manager.GetObjectManager().GetItems();
            bool temp = false;
            foreach(KeyValuePair<int, Hi5_Glove_Interaction_Item> item in dic)
            {
                float distance = 100.0f;
                foreach (Hi5_Interaction_Item_Collider itemCollider in item.Value.itemColliders)
                {
                    //Vector3 temp = Other.GetComponent<Collider>().ClosestPoint(transform.position)
                   Vector3 point = itemCollider.GetComponent<Collider>().ClosestPoint(transform.position);
                    float distanceTemp = Vector3.Distance(transform.position, point);
                    if (distanceTemp < distance)
                    {
                        distance = distanceTemp;
                    }
                }
                if (distance < 0.1f)
                {
                    IsOpenCollider = false;
                    temp = true;
                }
            }
            if(!temp)
                IsOpenCollider = true;
            //if (Hi5_Layer_Set.IsResetLayer  && !isSetLayer) 
            //{

            //	gameObject.layer = LayerMask.NameToLayer ("Hi5Palm");
            //	transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer ("Hi5Palm");
            //	isSetLayer = true;
            //}
        }
    }
}

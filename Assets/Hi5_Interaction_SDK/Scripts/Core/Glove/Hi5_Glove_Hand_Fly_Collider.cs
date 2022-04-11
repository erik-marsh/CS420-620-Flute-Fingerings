using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Hi5_Interaction_Core
{
    public class Hi5_Glove_Hand_Fly_Collider : Hi5_Glove_Interaction_Collider
    {
        internal bool IsFlyPinch(out List<int> collisions)
        {
            List<Collider> colliders = GetTriggers();
            if (colliders != null && colliders.Count > 0)
            {
                //Debug.Log("Hi5_Glove_Hand_Collider IsFlyPinchIsFlyPinch");
                collisions = GetObjectIdByCollider(colliders);
                return true;
            }
            else
            {
                collisions = null;
                return false;
            }
        }

        private List<int> GetObjectIdByCollider(List<Collider> colliders)
        {
            if (colliders == null)
                return null;
            List<int> colliderStrings = new List<int>();
            foreach (Collider item in colliders)
            {
				if (item.transform.parent != null &&  item.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>() != null)
                {
                    colliderStrings.Add(item.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>().idObject);
                }
				if(item.transform.parent.transform.parent != null &&  item.transform.parent.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>() != null)
				{
					colliderStrings.Add(item.transform.parent.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>().idObject);
				}
            }
            if (colliderStrings.Count == 0)
                return null;
            List<int> ListResult = colliderStrings.Distinct().ToList();
            return ListResult;
        }
        
		internal bool IsColliderSurround(out List<int> handList)
		{
			handList = new List<int>();
			bool isSurround = false;
			Collider SelfCollider = GetComponent<Collider> ();
			if (SelfCollider != null)
			{
				Hi5_Interaction_Object_Manager objecManger = Hi5_Interaction_Object_Manager.GetObjectManager ();
				if (objecManger != null && objecManger.GetItems() != null)
				{
					foreach (KeyValuePair<int, Hi5_Glove_Interaction_Item> item in objecManger.GetItems()) 
					{
						
							isSurround = item.Value.IsSurround (SelfCollider);
							if (isSurround) 
							{
								handList.Add(item.Key);
								isSurround = true;
							}
					}
				}
			}
			return isSurround;
		}

		bool isSetLayer = false;
		void Update()
		{
			//if (Hi5_Layer_Set.IsResetLayer  && !isSetLayer) 
			//{

			//	gameObject.layer = LayerMask.NameToLayer ("Hi5Palm");
			//	isSetLayer = true;
			//}
		}

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Hi5_Interaction_Core
{
	public class Hi5_Glove_Hand_Collider : Hi5_Glove_Interaction_Collider
	{
		internal bool IsPinch(out List<int> collisions)
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

		internal bool IsColliderSurround(out int objectId)
		{
			objectId = -1000;
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
							objectId = item.Key;
							break;
						}

					}
				}
			}
			return isSurround;
		}


		private List<int> GetObjectIdByCollider(List<Collider> colliders)
		{
			if (colliders == null)
				return null;
			List<int> colliderStrings = new List<int>();
			foreach (Collider item in colliders)
			{
				if (item.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>() != null)
				{
					colliderStrings.Add(item.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>().idObject);
				}
			}
			if (colliderStrings.Count == 0)
				return null;
			List<int> ListResult = colliderStrings.Distinct().ToList();
			return ListResult;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hi5_Interaction_Core;
namespace Hi5_Interaction_Interface
{
	public class Hi5_Interface_Object_Manager : MonoBehaviour 
	{
		public static Hi5_Interface_Object_Manager GetObjectManager()
		{
			return mManager;
		}

		static Hi5_Interface_Object_Manager mManager = null;
		private void Awake()
		{
			mManager = this;
		}
			
		public void  GetItemObject<T>(int objectId,out T backValue) where T : Hi5_Interface_Object_Base
		{
			backValue = null;

			Hi5_Interaction_Object_Manager object_Manager = GetComponent<Hi5_Interaction_Object_Manager>();
			if(object_Manager != null)
			{
				Hi5_Glove_Interaction_Item item = object_Manager.GetItemById(objectId);
				if(item != null)
				{
					T  scrip = item.GetComponent<T>();
					backValue = scrip;
				}
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hi5_Interaction_Core;
namespace Hi5_Interaction_Interface
{
	public  class Hi5_Interface_Object_Base : MonoBehaviour 
	{
		protected Hi5_Glove_Interaction_Item mItem = null;

		protected Hi5_Glove_Interaction_Item ObjectItem
		{
			get
			{
				if (mItem == null)
					mItem = gameObject.GetComponent<Hi5_Glove_Interaction_Item>();
				return mItem;
			}
		}
	    /*
	     * Get object state.
	     */
		internal protected E_Object_State GetObjectItemState
		{
			get
			{
				if (mItem == null)
				{
					return E_Object_State.EStatic;
				}
				else
					return mItem.state;
			}
		}
		/*
		 * Get object id. 
		 */
		internal protected int GetObjectId()
		{
			if (ObjectItem != null) {
				return ObjectItem.idObject;
			} else
				return -1000;
		}

	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
   
    public class Hi5_Hand_Nail_Collider : Hi5_Glove_Interaction_Collider
    {
        internal Hi5_Hand_Visible_Hand mHand = null;
        Hi5_Hand_Visible_Hand hand = null;
        override protected void Awake()
        {
            base.Awake();
            hand = gameObject.GetComponentInParent<Hi5_Hand_Visible_Hand>();
        }

        Dictionary<int, Hi5_Glove_Interaction_Item> m_dicInteraction = new Dictionary<int, Hi5_Glove_Interaction_Item>();
        internal bool IsNail(int objectId)
        {
            foreach (Collider item in m_Triggers)
            {
                if (item.GetComponent<Hi5_Glove_Interaction_Item>() != null)
                {
                    if (item.GetComponent<Hi5_Glove_Interaction_Item>().idObject == objectId)
                    {
                        return true;
                    }
                }
				if ( item.transform.parent != null&&  item.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>() != null)
				{
					if (item.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>().idObject == objectId)
					{
						return true;
					}
				}

            }
            return false;
        }
        //override protected void OnTriggerEnter(Collider other)
        //{
        //    if (other.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
        //    {
        //        int objectId = other.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
        //        //Debug.Log ("Hi5_Glove_Interaction_Item OnTriggerEnter"+objectId);
        //    }
        //    if (other.transform.parent != null && other.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>() != null)
        //    {
        //        int objectId = other.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>().idObject;

        //    }
        //    m_Triggers.Add(other);
        //}

        //private void Update()
        //{
        //    //foreach (KeyValuePair<int, int> item in m_TriggerCount)
        //    //{
        //    //    Debug.Log("object id ="+item.Key+ "count ="+ item.Value);
        //    //}
        //}

        //override protected void OnTriggerStay(Collider other)
        //{

        //}


        //override protected void OnTriggerExit(Collider other)
        //{
        //    if (other.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
        //    {
        //        int objectId = other.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
        //        //Debug.Log ("Hi5_Glove_Interaction_Item OnTriggerExit"+objectId);
        //    }
        //    if (other.transform.parent != null && other.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>() != null)
        //    {
        //        int objectId = other.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>().idObject;

        //    }
        //    m_Triggers.Remove(other);
        //    //Debug.Log("OnTriggerExit" + mNodeName);
        //}
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public class Hi5_Glove_Interaction_Collider : MonoBehaviour
    {
        protected string mNodeName;
        protected int mColliderLayer = 0;
        [SerializeField]
        internal List<Collider> m_Triggers = new List<Collider>();
        //internal Dictionary<int, int> m_TriggerCount = new Dictionary<int, int>();
       
        [SerializeField]
        internal List<Collision> m_Colliders = new List<Collision>();

        #region unity system
        virtual protected void Awake()
        {
            if(transform.parent != null)
                mNodeName =  transform.parent.name;
            mColliderLayer = gameObject.layer;
        }

        virtual protected void OnEnable()
        {
            m_Triggers.Clear();
            m_Colliders.Clear();
            //m_TriggerCount.Clear();
        }
        virtual protected void OnDisable()
        {
            m_Triggers.Clear();
            m_Colliders.Clear();
        }
        #endregion
        #region trigger
        virtual protected void OnTriggerEnter(Collider other)
        {
			if (other.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
            {
				int objectId = other.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
				//Debug.Log ("Hi5_Glove_Interaction_Item OnTriggerEnter"+objectId);
            }
            if (other.transform.parent != null && other.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>() != null)
            {
                int objectId = other.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
                
            }
            m_Triggers.Add(other);
        }

        //private void Update()
        //{
        //    //foreach (KeyValuePair<int, int> item in m_TriggerCount)
        //    //{
        //    //    Debug.Log("object id ="+item.Key+ "count ="+ item.Value);
        //    //}
        //}

        virtual protected void OnTriggerStay(Collider other)
        {

        }


        virtual protected void OnTriggerExit(Collider other)
        {
			if (other.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
			{
				int objectId = other.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
				//Debug.Log ("Hi5_Glove_Interaction_Item OnTriggerExit"+objectId);
			}
            if (other.transform.parent != null && other.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>() != null)
            {
                int objectId = other.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>().idObject;

            }
            m_Triggers.Remove(other);
            //Debug.Log("OnTriggerExit" + mNodeName);
        }
        #endregion

        #region Collision


		virtual  protected void OnCollisionEnter(Collision collision)
        {
			if(!m_Colliders.Contains(collision))
            	m_Colliders.Add(collision);
        }

		virtual protected void OnCollisionStay(Collision collision)
        {

        }

		virtual protected void OnCollisionExit(Collision collision)
        {
			if(m_Colliders.Contains(collision))
            	m_Colliders.Remove(collision);
        }
        #endregion

        internal int GetCollisiponLayer()
        {
            return gameObject.layer;
        }

        internal List<Collider> GetTriggers()
        {
            return m_Triggers;
        }

        internal List<Collision> GetCollisions()
        {
            return m_Colliders;
        }
        //internal bool IsTriggerObject(int objectId)
        //{
        //    if (m_TriggerCount.ContainsKey(objectId))
        //        return true;
        //    else
        //        return false;
        //}
    }
}

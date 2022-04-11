using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Hi5_Interaction_Core
{
    public class Hi5_Glove_Collider_Palm : Hi5_Glove_Interaction_Collider
    {
        #region message
        internal Transform mAnchor;
        //Hi5_Glove_Interaction_Message mMessage = null;
        internal protected Hi5_Glove_Interaction_Hand mHand = null;
		internal Transform mChildCollider = null;
        internal Hi5_Record mRecord = new Hi5_Record();
        internal protected void SetHi5Message(Hi5_Glove_Interaction_Message message)
        {
            //mMessage = message;
        }
        #endregion
        /// <summary>
        /// judge whether the object is on the inside or outside
        /// </summary>
        protected internal bool JudgeObjectHandInside(Transform objectTransform)
        {
            if(mAnchor == null)
                mAnchor = transform.GetChild(0);
            Vector3 palmUp = -mAnchor.up;
            Vector3 v1 = objectTransform.position - mAnchor.position;
            //float dotValue = Vector3.Dot(v1, pamlUp);
            float angle = Vector3.Angle(v1, palmUp);
            if (angle > 30.0f && angle < 75.0f)
            {
                //Debug.Log("angle" + angle);
                return true;
            }
            else
            {
                //Debug.Log("angle" + angle);
                return false;
            }
        }

        #region unity system
        protected override void Awake()
        {
            base.Awake();
            mAnchor = transform.GetChild(0);
			mChildCollider = transform.GetChild(1);
        }
        override protected void OnEnable()
        {
            base.OnEnable();
            
        }
        override protected void OnDisable()
        {
            base.OnDisable();
        }
        #endregion

		//internal bool IsLift(out List<int> collisions)
		//{
		//	if (mChildCollider != null) {
		//		List<Collision> colliders = mChildCollider.GetCollisions();
		//		if (colliders != null && colliders.Count > 0)
		//		{
		//			collisions = GetObjectIdByCollision(colliders);
		//			return true;
		//		}
		//		else
		//		{
		//			collisions = null;
		//			return false;
		//		}
		//	}
		//	else {
		//		collisions = null;
		//		return false;
		//	}

		//}

        internal bool IsClap(out List<int> collisions)
        {
            List<Collider> colliders = GetTriggers();
            if (colliders != null && colliders.Count > 0)
            {
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
				if (item.GetComponent<Hi5_Glove_Interaction_Item>() != null ||item.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>() != null )
                {
					int objectId = -1000;
					if(item.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
						objectId = item.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
					else if(item.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
						objectId = item.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
					if(objectId != -1000)
						colliderStrings.Add(objectId);
                }
            }
            if (colliderStrings.Count == 0)
                return null;
            List<int> ListResult = colliderStrings.Distinct().ToList();
            return ListResult;
        }

		private List<int> GetObjectIdByCollision(List<Collision> colliders)
		{
			if (colliders == null)
				return null;
			List<int> colliderStrings = new List<int>();
			foreach (Collision item in colliders)
			{
				if (item.collider.GetComponent<Hi5_Glove_Interaction_Item>() != null ||item.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>() != null)
				{
					int objectId = -1000;
					if(item.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
						objectId = item.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
					else if(item.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
						objectId = item.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
					if(objectId != -1000)
						colliderStrings.Add(item.collider.GetComponent<Hi5_Glove_Interaction_Item>().idObject);
				}
			}
			if (colliderStrings.Count == 0)
				return null;
			List<int> ListResult = colliderStrings.Distinct().ToList();
			return ListResult;
		}
		internal void OpenPhyCollider(bool open)
		{
			if (mAnchor == null)
				mAnchor = transform.GetChild (0);
			if (mAnchor != null) {
				if (open)
					mAnchor.gameObject.SetActive (true);
				else
					mAnchor.gameObject.SetActive (false);
			
			}
			if (mHand != null && mHand.mVisibleHand != null && mHand.mVisibleHand.palm != null) {
				if (mHand.mVisibleHand.palm.GetComponent<Hi5_Hand_Palm> () != null) {
					mHand.mVisibleHand.palm.GetComponent<Hi5_Hand_Palm> ().OpenBoxCollider (!open);
				}
			}

		}


//		internal bool IsColliderSurround(out int objectId)
//		{
//			objectId = -1000;
//			bool isSurround = false;
//			Collider SelfCollider = GetComponent<Collider> ();
//			if (SelfCollider != null)
//			{
//				Hi5_Interaction_Object_Manager objecManger = Hi5_Interaction_Object_Manager.GetObjectManager ();
//				if (objecManger != null && objecManger.GetItems() != null)
//				{
//					foreach (KeyValuePair<int, Hi5_Glove_Interaction_Item> item in objecManger.GetItems()) 
//					{
//
//						isSurround = item.Value.IsSurround (SelfCollider);
//						if (isSurround) 
//						{
//							objectId = item.Key;
//							break;
//						}
//
//					}
//				}
//			}
//			return isSurround;
//		}
		bool isSetLayer = false;
		void Update()
		{
            mRecord.RecordPosition(Time.deltaTime, transform);
            //if (Hi5_Layer_Set.IsResetLayer  && !isSetLayer) 
            //{

            //	gameObject.layer = LayerMask.NameToLayer ("Hi5Palm");
            //	transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer ("Hi5Palm");
            //	isSetLayer = true;
            //}
        }
        internal Hi5_Record GetHi5Record()
        {
            return mRecord;
        }

        public Queue<Hi5_Position_Record> GetRecord()
        {
            return mRecord.GetRecord();
        }
    }
}

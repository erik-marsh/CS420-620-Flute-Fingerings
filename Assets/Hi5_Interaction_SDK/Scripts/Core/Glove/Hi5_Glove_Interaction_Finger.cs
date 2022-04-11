using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Hi5_Interaction_Core
{
    public enum Hi5_Glove_Interaction_Finger_Type
    {
        ENone = -1,
        EThumb = 0,
        EIndex,
        EMiddle,
        ERing,
        EPinky
    }
    public class Hi5_Glove_Interaction_Finger : MonoBehaviour
    { 
        internal Hi5_Glove_Interaction_Hand mHand = null;
        #region message
        Hi5_Glove_Interaction_Message mMessage = null;
        internal protected void SetHi5Hand(Hi5_Glove_Interaction_Hand hand)
        {
           
            mHand = hand;
        }
        #endregion

        #region Hi5_Glove_Interaction_Finger_Type
        public Hi5_Glove_Interaction_Finger_Type m_finger_type = Hi5_Glove_Interaction_Finger_Type.EThumb;
        #endregion

        //float mFingerAngle = 180.0f;
        //float mFingerPercentage = 200.0f;
        //public float mAngleMin = 130f;
        //public float mAngleMax = 180f;

        //public int m_GrabPercentage = 50;
        //public int m_GrabReleasePercentage = 40;

        //public int m_PinchPercentage = 80;
        //public int m_PinchReleasePercentage = 70;

        
        internal float colliderAngle = 0.0f;
        internal float flyPinch = 0.0f;
        bool isPinchRelease = false;
        internal  bool IsPinchRelease
        {
            get { return isPinchRelease; }
        }

        bool isFlyPinch = false;
        internal bool IsFlyPinch
        {
            get { return isFlyPinch; }
        }


		internal bool IsFlyRunPinch()
		{
			float currentfingerAngle = GetAngle(mChildNodes[1], mChildNodes[3], mChildNodes[4]);
            if (Hi5_Interaction_Const.TestPinchOpenCollider)
            {
                if (currentfingerAngle < 140.0f)
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (currentfingerAngle < Hi5_Interaction_Const.FingerFlyPinchAngle)
               // if (currentfingerAngle < 140.0f)
                {
                    return true;
                }
                else
                    return false;
            }
			//Debug.Log("m_finger_type" + m_finger_type+"currentfingerAngle=" + currentfingerAngle);
			////if (currentfingerAngle < Hi5_Interaction_Const.FingerFlyPinchAngl)
   //          if (currentfingerAngle < 140.0f)
   //          {
			//	return  true;
			//}
			//else
			//	return false;
		}
        #region unity system
        void Awake()
        {
            
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (mChildNodes != null && mChildNodes.Count == 4)
            {
                //pinch realse
                {
                    float currentfingerAngle = GetAngle(mChildNodes[1], mChildNodes[3], mChildNodes[4]);
                    colliderAngle = currentfingerAngle;

                    if (currentfingerAngle > Hi5_Interaction_Const.FingerPinchReleaseAngle)
                    {
                        isPinchRelease = true;
                    }
                    else
                        isPinchRelease = false;
                }

                //fly pinch
                {
                    float currentfingerAngle = GetAngle(mChildNodes[1], mChildNodes[3], mChildNodes[4]);
                    flyPinch = currentfingerAngle;
                    //Debug.Log("m_finger_type" + m_finger_type+"currentfingerAngle=" + currentfingerAngle);
                    if (currentfingerAngle < Hi5_Interaction_Const.FingerFlyPinchAngle)
                    {
                        isFlyPinch = true;
                    }
                    else
                        isFlyPinch = false;
                }
            }
        }
        #endregion

        #region nodes Bone_Collider
        protected Dictionary<int, Hi5_Glove_Collider_Finger> mBoneCollider = new Dictionary<int, Hi5_Glove_Collider_Finger>();
        public Dictionary<int, Transform> mChildNodes = new Dictionary<int, Transform>();

        internal void AddChildNode(List<Transform> childs)
        {
            mChildNodes.Clear();
            int index = 1;
            for (int i = 0; i < childs.Count; i++)
            {
                mChildNodes.Add(index, childs[i]);
                //hudfas.Add(childs[i]);
                AddBoneCollider(childs[i], index);
                index++;
            }   
        }

        private void AddBoneCollider(Transform bone,int index)
        {
            if (m_finger_type == Hi5_Glove_Interaction_Finger_Type.EThumb && index == 1)
                return;
            Transform temp = null;
            if (index != 4)
                temp =  bone.GetChild(1);
            else
                temp = bone.GetChild(0);
            if (temp != null)
            {
                if (temp.GetComponent<Hi5_Glove_Collider_Finger>() != null)
                {
                    mBoneCollider.Add(index, temp.GetComponent<Hi5_Glove_Collider_Finger>());
                    //temo.Add(temp.GetComponent<Hi5_Glove_Collider_Finger>());
                    temp.GetComponent<Hi5_Glove_Collider_Finger>().SetAttribute(index, m_finger_type,this);
                    //tyu.Add(index);
                }
            }
        }
        #endregion nodes

        #region Angle
        float GetAngle(Transform leftTransform, Transform midTransform, Transform rightTransform)
        {
            Vector3 left = leftTransform.position;
            Vector3 middle = midTransform.position;
            Vector3 right = rightTransform.position; // finger tip
            Vector3 vLeft = left - middle;
            Vector3 vRight = right - middle;
            float angle = Vector3.Angle(vLeft, vRight);
           // float angle1 = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(vLeft, vRight) / (vLeft.magnitude * vRight.magnitude));
            return angle;
        }

        float NormalizedAngle(float angleMin, float angleMax, float angle)
        {
            float normalizedAngle, percantageValue;
            normalizedAngle = Mathf.InverseLerp(angleMax, angleMin, angle);
            percantageValue = (int)(normalizedAngle * 100);
            return percantageValue;
        }
        #endregion

        //protected internal bool IsGrabbing()
        //{
        //    if (mFingerPercentage > m_GrabPercentage)
        //    {
        //        return true;
        //    }
        //    else if (mFingerPercentage > m_GrabReleasePercentage)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

      

        protected internal void NotifyEnterPinchState()
        {
            //colliderAngle = GetAngle(mChildNodes[1], mChildNodes[3], mChildNodes[4]);
            mHand.mState.AddPinchCollider(this);
        }

        protected internal void NotifyEnterPinch2State()
        {
            //colliderAngle = GetAngle(mChildNodes[1], mChildNodes[3], mChildNodes[4]);
            mHand.mState.AddPinch2Collider(this);
        }

        protected internal bool IsCollider()
        {

            //ruige redo 暂时怎么处理
            int ObjectId = GetColliderObjectId();

           // Debug.Log("IsCollider"+ ObjectId);
            bool IsCollider = false;
            if (mBoneCollider.ContainsKey(1) && mBoneCollider[1].IsTriggerObjectById(ObjectId))
            {
                IsCollider = true;
                return IsCollider;
            }
            if (mBoneCollider.ContainsKey(2) && mBoneCollider[2].IsTriggerObjectById(ObjectId))
            {
                IsCollider = true;
                return IsCollider;
            }
            if (mBoneCollider.ContainsKey(3) && mBoneCollider[3].IsTriggerObjectById(ObjectId))
            {
                IsCollider = true;
                return IsCollider;
            }
            if (mBoneCollider.ContainsKey(4) && mBoneCollider[4].IsTriggerObjectById(ObjectId))
            {
                IsCollider = true;
                return IsCollider;
            }
            return IsCollider;
        }

        int GetColliderObjectId()
        {
            Dictionary<int, int> count = new Dictionary<int, int>();
            List<Collider> collider4s = mBoneCollider[4].GetTriggers();
            foreach(Collider item in collider4s)
            {
				if (item.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null ||  item.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>() != null)
                {
					int objectId = -1000;
					if(item.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
						objectId = item.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
					else if(item.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
						objectId = item.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
                    if (objectId != -1000)
                    {
                        if(!count.ContainsKey(objectId))
                            count.Add(objectId, 1);
                    }
						
                }
            }
            List<Collider> collider3s = mBoneCollider[3].GetTriggers();
            foreach (Collider item in collider3s)
            {
				if (item.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null ||  item.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>() != null)
                {
					int objectId = -1000;
					if(item.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
						objectId = item.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
					else if(item.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
						objectId = item.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
					if (objectId != -1000) {
						if (count.ContainsKey(objectId))
						{
							count[objectId] += 1;
						}
						else
						{
                            if (!count.ContainsKey(objectId))
                                count.Add(objectId, 1);
						}
					}


                }
            }
            List<Collider> collider2s = mBoneCollider[2].GetTriggers();
            foreach (Collider item in collider2s)
            {
				if (item.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null ||  item.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>() != null)
                {
					int objectId = -1000;
					if(item.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
						objectId = item.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
					else if(item.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
						objectId = item.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
					if (objectId != -1000)
					{
						if (count.ContainsKey(objectId))
						{
							count[objectId] += 1;
						}
						else
						{
                            if (!count.ContainsKey(objectId))
                                count.Add(objectId, 1);
						}
					}
                   
                }
            }
            if (mBoneCollider.ContainsKey(1))
            {
                List<Collider> collider1s = mBoneCollider[1].GetTriggers();
                foreach (Collider item in collider1s)
                {
					if (item.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null ||  item.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>() != null)
                    {
						int objectId = -1000;
						if(item.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
							objectId = item.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
						else if(item.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
							objectId = item.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
						if (objectId != -1000) {
							if (count.ContainsKey(objectId))
							{
								count[objectId] += 1;
							}
							else
							{
                                if (!count.ContainsKey(objectId))
                                    count.Add(objectId, 1);
							}
						}
                    }
                }
            }
            Dictionary<int, int> dic1_SortedByKey = count.OrderByDescending(p => p.Value).ToDictionary(p => p.Key, o => o.Value);
            int ObjectId = 0;
            foreach (KeyValuePair<int, int> keyValue in dic1_SortedByKey)
            {
                ObjectId = keyValue.Key;
                break;
            }
            return ObjectId;
        }




        protected internal bool IsPinch2TailTrigger(out List<int> collisionObjects)
        {
            collisionObjects = null;
            List<Collider> collider4s = mBoneCollider[4].GetTriggers();
            if (collider4s != null && collider4s.Count > 0)
            {
                List<int> colliderString4 = GetObjectIdByCollider(collider4s);
                if (collider4s != null)
                {
                    collisionObjects = colliderString4;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

		protected internal bool IsPinch2Finger()
		{
			float currentfingerAngle = GetAngle(mChildNodes[1], mChildNodes[3], mChildNodes[4]);
			if (currentfingerAngle < 140.0f) {
				return true;
			}
			else
			{
				return false;
			}

		}

//		internal bool IsPokeAngel(Collider other,Transform item)
//		{
//			float currentfingerAngle = GetAngle(mHand.mVisibleHand.handTransform, mChildNodes[1], mChildNodes[4]);
//			if (currentfingerAngle < 0.0f) {
//				currentfingerAngle += 180.0f;
//			}
//			Debug.Log ("currentfingerAngle"+currentfingerAngle);
//			if (colliderAngle > Hi5_Interaction_Const.FingerClapFourMaxAngel && colliderAngle < (180.0f - Hi5_Interaction_Const.FingerClapFourMaxAngel)) {
//				return true;
//			}
//			else
//			{
//				Vector3 closePoint = other.ClosestPoint (item.transform.position);
//				float angle = Vector3.Angle(Hi5_Interaction_Object_Manager.GetObjectManager().transform.up, (closePoint - item.transform.position));
//				if (angle < 0.0f) {
//					angle += 180.0f;
//				}
//
//			}
//				
//
//
//
//
//		}

        protected internal bool IsPinch2Trigger(out List<int> collisionObjects)
        {
            collisionObjects = null;
            List<Collider> collider4s = mBoneCollider[4].GetTriggers();
            
            if (collider4s != null && collider4s.Count > 0)
            {
                List<int> colliderString4 = GetObjectIdByCollider(collider4s);
                if (collider4s != null)
                {
					if (IsPinch2Finger ())
					{
						collisionObjects = colliderString4;
						return true;
					}
                    
                }
               
            }
            List<Collider> collider3s = mBoneCollider[3].GetTriggers();
            if (collider4s != null && collider3s.Count > 0)
            {
                List<int> colliderString3 = GetObjectIdByCollider(collider3s);
                if (colliderString3 != null)
                {
					if (IsPinch2Finger ()) 
					{
					
						collisionObjects = colliderString3;
						return true;
					}
                    
                }

            }
            return false;
        }

        internal bool IsThumbColliderIndex()
        {
            if (m_finger_type == Hi5_Glove_Interaction_Finger_Type.EThumb)
            { 
                 Hi5_Glove_Collider_Finger colliderItem;
                if (mBoneCollider.TryGetValue(3, out colliderItem))
                {
                    return colliderItem.IsIndexColliderThumb();
                }
                else
                    return false;
            }
            else
                return false;
        }

        protected internal bool IsPinchTrigger(out List<int> collisionObjects)
        {
            float Angle1 = GetAngle(mChildNodes[2], mChildNodes[3], mChildNodes[4]);
            float Angle2 = GetAngle(mChildNodes[1], mChildNodes[2], mChildNodes[3]);
            bool isPinchTemp = true;
            if (m_finger_type == Hi5_Glove_Interaction_Finger_Type.EIndex)
            {
                if (Angle1 > 103.0f && Angle1 < 123.0f && Angle2 > 93.0f && Angle2 < 103.0f)
                    isPinchTemp = false;
                //Debug.Log("Hi5_Glove_Interaction_Finger_Type.EIndex Angle1=" + Angle1 + "   Angle2=" + Angle2);
            }
            else if (m_finger_type == Hi5_Glove_Interaction_Finger_Type.EMiddle)
            {
                if (Angle1 > 100.0f && Angle1 < 1110.0f && Angle2 > 75.0f && Angle2 < 85.0f)
                    isPinchTemp = false;
               // Debug.Log("Hi5_Glove_Interaction_Finger_Type.EMiddle Angle1=" + Angle1 + "   Angle2=" + Angle2);
            }
            else if (m_finger_type == Hi5_Glove_Interaction_Finger_Type.ERing)
            {
                if (Angle1 > 100.0f && Angle1 < 109.0f && Angle2 > 75.0f && Angle2 < 85.0f)
                    isPinchTemp = false;
                //Debug.Log("Hi5_Glove_Interaction_Finger_Type.ERing Angle1=" + Angle1 + "   Angle2=" + Angle2);
            }
            collisionObjects = null;
            List<Collider> collider4s = mBoneCollider[4].GetTriggers();
            List<Collider> collider3s = mBoneCollider[3].GetTriggers();
            List<int> colliderString4 = GetObjectIdByCollider(collider4s);
            List<int> colliderString3 = GetObjectIdByCollider(collider3s);
            if (colliderString4 != null && colliderString3 != null)
                collisionObjects = colliderString4.Union(colliderString3).ToList();
            else if (colliderString4 != null)
                collisionObjects = colliderString4;
            else if (colliderString3 != null)
                collisionObjects = colliderString3;
            //if (colliderString4 != null)
            //    collisionObjects = colliderString4;
            if (collisionObjects == null)
                return false;
            else
            {
				float currentfingerAngle = GetAngle(mChildNodes[1], mChildNodes[3], mChildNodes[4]);
				if (currentfingerAngle < 140.0f) {
                    if (isPinchTemp)
                        return true;
                    else
                        return false;
				}
				else
				{
					return false;
				}
                //return true;
            }
        }

        

        private List<int> GetObjectIdByCollider(List<Collider> colliders)
        {
            if (colliders == null)
                return null;
            List<int> colliderStrings = new List<int>();
            foreach (Collider item in colliders)
            {
				if (item.GetComponent<Hi5_Glove_Interaction_Item>() != null
                    || item.transform.parent.GetComponent<Hi5_Glove_Interaction_Item>() != null)
                {
					int objectId = -1000;
					if(item.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
						objectId = item.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
					else if(item.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
						objectId = item.transform.parent.gameObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
					if (objectId != -1000) {
						colliderStrings.Add (objectId);
					}
                }
            }
            if (colliderStrings.Count == 0)
                return null;
            List<int>  ListResult = colliderStrings.Distinct().ToList();
            return ListResult;
        }

        
        internal protected void TailFingerCollider(int objectId)
        {
            
        }

        internal bool PalmPinch()
        {
            float colliderAngle = GetAngle( mChildNodes[1], mChildNodes[3], mChildNodes[4]);
            //Debug.Log("m_finger_type"+ m_finger_type+ "colliderAngle"+ colliderAngle);
            if (colliderAngle < Hi5_Interaction_Const.PalmPinchFingerAngle)
                return true;
            else
                return false;
        }

        internal bool FlyCollision(out List<int> collision)
        {
            List<int>  collisionObjects = null;
            List<Collider> collider4s = mBoneCollider[4].GetTriggers();
            List<Collider> collider3s = mBoneCollider[3].GetTriggers();
            List<Collider> collider2s = mBoneCollider[2].GetTriggers();
            List<Collider> collider1s = null;
            if (mBoneCollider.ContainsKey(1))
                collider1s = mBoneCollider[1].GetTriggers();

            List<int> colliderString4 = GetObjectIdByCollider(collider4s);
            List<int> colliderString3 = GetObjectIdByCollider(collider3s);
            List<int> colliderString2 = GetObjectIdByCollider(collider2s);
            List<int> colliderString1 = null;
            if (collider1s != null)
                colliderString1 = GetObjectIdByCollider(collider1s);
            if (colliderString4 != null && colliderString3 != null)
                collisionObjects = colliderString4.Union(colliderString3).ToList();
            else if (colliderString4 != null)
                collisionObjects = colliderString4;
            else if (colliderString3 != null)
                collisionObjects = colliderString3;
            if (collisionObjects != null)
            {
                if (colliderString2 != null)
                    collisionObjects = collisionObjects.Union(colliderString2).ToList();
            }
            else
                collisionObjects = colliderString2;

            //if (collisionObjects != null)
            //{
            //    if (colliderString1 != null)
            //        collisionObjects = collisionObjects.Union(colliderString1).ToList();
            //}
            //else
            //    collisionObjects = colliderString1;

            //if (collisionObjects != null && colliderString2 != null)
            //    collisionObjects = collisionObjects.Union(colliderString2).ToList();
            collision = collisionObjects;
            if (collisionObjects == null)
                return false;
            else
                return true;
        }

        internal bool FlyPinchAngle(out List<int> collisions)
        {
            collisions = null;
            List<int> collision;
            bool Iscollision = FlyCollision(out collision);
            if (!Iscollision)
                return false;
            float colliderAngle = GetAngle(mHand.mVisibleHand.handTransform, mChildNodes[1], mChildNodes[4]);
            //Debug.Log("m_finger_type"+ m_finger_type+ "colliderAngle"+ colliderAngle);
            if (colliderAngle < Hi5_Interaction_Const.FingerFlyPinchAngle)
            {
                collisions = collision;
                return true;
            }
            else
                return false;
        }

        internal bool IsTailColliderById(int objectId)
        {
            List<Collider> collider4s = mBoneCollider[4].GetTriggers();
            if (collider4s != null && collider4s.Count > 0)
            {
                List<int> colliderString4 = GetObjectIdByCollider(collider4s);
                if(colliderString4 != null)
                {
                    if (colliderString4.Contains(objectId))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;
        }


		internal bool IsLift(out List<int> collisions)
		{
			collisions = null;
			bool IsThreeClap = false;
			bool IsTwoClap = false;
			bool IsOneClap = false;
			//第四指是否拍
			{
				List<Collider> collider3s = mBoneCollider[3].GetTriggers();
				if (collider3s != null && collider3s.Count > 0)
				{
					float colliderAngle = GetAngle(mHand.mVisibleHand.handTransform, mChildNodes[1], mChildNodes[4]);
					if (colliderAngle > 90.0f)
						colliderAngle = colliderAngle - 180.0f;
					//Debug.Log("collider3s colliderAngle="+ colliderAngle);
					if (colliderAngle > -45.0f
						&& colliderAngle < 45.0f)
					{
						//Debug.Log("collider3s contact");
						List<int> colliderString3 = GetObjectIdByCollider(collider3s);
						collisions = colliderString3;
						IsThreeClap = true;

					}
				}
			}
			// 第三指是否拍
			{
				List<Collider> collider2s = mBoneCollider[2].GetTriggers();
				if (collider2s != null && collider2s.Count > 0)
				{
					float colliderAngle = GetAngle(mHand.mVisibleHand.handTransform, mChildNodes[1], mChildNodes[3]);
					if (colliderAngle > 90.0f)
						colliderAngle = colliderAngle - 180.0f;
					if (colliderAngle > -45.0f
						&& colliderAngle <  45.0f)
					{
						List<int> colliderString2 = GetObjectIdByCollider(collider2s);
						collisions = colliderString2;
						IsTwoClap = true;
					}
				}
			}
			// 第二指是否拍
			{
				if(mBoneCollider.ContainsKey(1))
				{
					List<Collider> collider1s = mBoneCollider[1].GetTriggers();
					if (collider1s != null && collider1s.Count > 0)
					{
						float colliderAngle = GetAngle(mHand.mVisibleHand.handTransform, mChildNodes[1], mChildNodes[2]);
						if (colliderAngle > 90.0f)
							colliderAngle = colliderAngle - 180.0f;
						if (colliderAngle > -45.0f
							&& colliderAngle <  45.0f)
						{
							List<int> colliderString1 = GetObjectIdByCollider(collider1s);
							collisions = colliderString1;
							IsOneClap = true;
						}
					}
				}

			}

			if (IsOneClap || IsTwoClap || IsThreeClap)
				return true;
			else
				return false;
		}

        

		internal bool IsFingerPlane()
		{
			if (mHand == null || mHand.mVisibleHand == null)
				return false;
			float colliderAngle = GetAngle(mHand.mVisibleHand.handTransform, mChildNodes[1], mChildNodes[4]);
//			if (colliderAngle > 90.0f)
//				colliderAngle = colliderAngle - 180.0f;
			//Debug.Log("IsFingerPlane colliderAngle=" + colliderAngle);
			if (colliderAngle > 150.0f)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		internal bool IsFingerFist()
		{
            float Angle1 = GetAngle(mChildNodes[2], mChildNodes[3], mChildNodes[4]);
            float Angle2 = GetAngle(mChildNodes[1], mChildNodes[2], mChildNodes[3]);
            float Angle3 = GetAngle(mChildNodes[1].parent, mChildNodes[1], mChildNodes[2]);
            float colliderAngle1 = GetAngle(mChildNodes[1], mChildNodes[2], mChildNodes[3]);
			bool isTrigger1 = false;
            if (m_finger_type == Hi5_Glove_Interaction_Finger_Type.EIndex)
            {
                if (colliderAngle1 > 80.0f && colliderAngle1 < 128.0f)
                    isTrigger1 = true;
                //Debug.Log ("index colliderAngle1  " + colliderAngle1);
                //Debug.Log("index Angle2  " + Angle2);
                //Debug.Log("index Angle3  " + Angle3);
            }
            if (m_finger_type == Hi5_Glove_Interaction_Finger_Type.EMiddle)
            {
                if (colliderAngle1 > 68.0f && colliderAngle1 < 114.0f)
                    isTrigger1 = true;
                //Debug.Log("middle colliderAngle1  " + colliderAngle1);
            }
            if (m_finger_type == Hi5_Glove_Interaction_Finger_Type.ERing)
            {
                if (colliderAngle1 > 70.0f && colliderAngle1 < 107.0f)
                    isTrigger1 = true;
               // Debug.Log("ring colliderAngle1  " + colliderAngle1);
            }
            if (m_finger_type == Hi5_Glove_Interaction_Finger_Type.EPinky)
            {
                if (colliderAngle1 > 65.0f && colliderAngle1 < 100.0f)
                    isTrigger1 = true;
                //Debug.Log ("pink colliderAngle1  "+colliderAngle1);
            }
            float colliderAngle2 = GetAngle(mChildNodes[2], mChildNodes[3], mChildNodes[4]);
            bool isTrigger2 = false;
            if (m_finger_type == Hi5_Glove_Interaction_Finger_Type.EIndex)
            {
                if (colliderAngle2 > 100.0f && colliderAngle2 < 127.0f)
                    isTrigger2 = true;
               // Debug.Log("index colliderAngle2  " + colliderAngle2);
            }

            if (m_finger_type == Hi5_Glove_Interaction_Finger_Type.EMiddle)
            {
                if (colliderAngle2 > 95.0f && colliderAngle2 < 130.0f)
                    isTrigger2 = true;
               // Debug.Log("middle colliderAngle2  " + colliderAngle2);
            }
            if (m_finger_type == Hi5_Glove_Interaction_Finger_Type.ERing)
            {
                if (colliderAngle2 > 95.0f && colliderAngle2 < 125.0f)
                    isTrigger2 = true;
               // Debug.Log("ring colliderAngle2  " + colliderAngle2);
            }
            if (m_finger_type == Hi5_Glove_Interaction_Finger_Type.EPinky) {
                if (colliderAngle2 > 95.0f && colliderAngle2 < 126.0f)
                    isTrigger2 = true;
               // Debug.Log ("pinky colliderAngle2  "+colliderAngle2);
            }
            if (isTrigger1 && isTrigger2)
				return true;
			else
				return false;
		}

        internal bool IsClap(out List<int> collisions)
        {
            collisions = null;
			if (mHand == null || mHand.mVisibleHand == null)
				return false;
            bool IsThreeClap = false;
            bool IsTwoClap = false;
            bool IsOneClap = false;
            //第四指是否拍
            {
                List<Collider> collider3s = mBoneCollider[3].GetTriggers();
                if (collider3s != null && collider3s.Count > 0)
                {
                    //if (mHand == null)
                    //    Debug.Log("mHand empty");
                    //if(mHand.mVisibleHand == null)
                    //    Debug.Log("(mHand.mVisibleHand empty");
                    //if (mHand.mVisibleHand.handTransform == null)
                    //    Debug.Log("(mHand.mVisibleHand.handTransform empty");
                    float colliderAngle = GetAngle(mHand.mVisibleHand.handTransform, mChildNodes[1], mChildNodes[4]);
                    if (colliderAngle > 90.0f)
                        colliderAngle = colliderAngle - 180.0f;
                    //Debug.Log("collider3s colliderAngle="+ colliderAngle);
                    if (colliderAngle > Hi5_Interaction_Const.FingerClapFourMinAngle
                        && colliderAngle < Hi5_Interaction_Const.FingerClapFourMaxAngle)
                    {
                        //Debug.Log("collider3s contact");
                        List<int> colliderString3 = GetObjectIdByCollider(collider3s);
                        collisions = colliderString3;
                        IsThreeClap = true;
                         
                    }
                }
            }
            // 第三指是否拍
            {
                List<Collider> collider2s = mBoneCollider[2].GetTriggers();
                if (collider2s != null && collider2s.Count > 0)
                {
                    //if (mHand == null)
                    //    Debug.Log("mHand empty");
                    //if (mHand.mVisibleHand == null)
                    //    Debug.Log("(mHand.mVisibleHand empty");
                    //if (mHand.mVisibleHand.handTransform == null)
                    //    Debug.Log("(mHand.mVisibleHand.handTransform empty");
                    float colliderAngle = GetAngle(mHand.mVisibleHand.handTransform, mChildNodes[1], mChildNodes[3]);
                    if (colliderAngle > 90.0f)
                        colliderAngle = colliderAngle - 180.0f;
                    if (colliderAngle > Hi5_Interaction_Const.FingerClapThreeMinAngle
                        && colliderAngle < Hi5_Interaction_Const.FingerClapThreeMaxAngle)
                    {
                        List<int> colliderString2 = GetObjectIdByCollider(collider2s);
                        collisions = colliderString2;
                        IsTwoClap = true;
                    }
                }
            }
            // 第二指是否拍
            {
                if(mBoneCollider.ContainsKey(1))
                {
                    List<Collider> collider1s = mBoneCollider[1].GetTriggers();
                    if (collider1s != null && collider1s.Count > 0)
                    {
                        //if (mHand == null)
                        //    Debug.Log("mHand empty");
                        //if (mHand.mVisibleHand == null)
                        //    Debug.Log("(mHand.mVisibleHand empty");
                        //if (mHand.mVisibleHand.handTransform == null)
                        //    Debug.Log("(mHand.mVisibleHand.handTransform empty");
                        float colliderAngle = GetAngle(mHand.mVisibleHand.handTransform, mChildNodes[1], mChildNodes[2]);
                        if (colliderAngle > 90.0f)
                            colliderAngle = colliderAngle - 180.0f;
                        if (colliderAngle > Hi5_Interaction_Const.FingerClapTwoMinAngle
                            && colliderAngle < Hi5_Interaction_Const.FingerClapTwoMaxAngle)
                        {
                            List<int> colliderString1 = GetObjectIdByCollider(collider1s);
                            collisions = colliderString1;
                            IsOneClap = true;
                        }
                    }
                }
               
            }

            if (IsOneClap || IsTwoClap || IsThreeClap)
                return true;
            else
                return false;
        }

        internal float GetPinch2OneToFour()
        {
            float backValue;
            if (m_finger_type != Hi5_Glove_Interaction_Finger_Type.EThumb)
                backValue = mChildNodes[1].transform.localEulerAngles.z;
            else
                backValue = mChildNodes[1].transform.localEulerAngles.x;
            return backValue;
        }

        internal Vector3 GetTailPosition()
        {
            return mChildNodes[4].transform.position;
        }

        internal bool IsPinch2Realease(float angel)
        {
            float backValue;
            if (m_finger_type != Hi5_Glove_Interaction_Finger_Type.EThumb)
                backValue = mChildNodes[1].transform.localEulerAngles.z;
            else
                backValue = mChildNodes[1].transform.localEulerAngles.x;
            if ((angel - backValue) > Hi5_Interaction_Const.FingerPinch2Release)
            {
                return true;
            }
            else
                return false;
        }

        internal float Pich2RealseTailDistance(Hi5_Glove_Interaction_Item item,out bool ContactIsSelf)
        {
           float distance = Hi5_Interaction_Const.GetDistance(mChildNodes[4].transform, item, out ContactIsSelf);
            //Debug.DrawLine(mChildNodes[4].transform.position, item.transform.position, Color.red, 1);
            return distance;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hi5_Interaction_Core
{
    public static class Extensions
    {
        public static Transform Search(this Transform target, string name)
        {
            if (target.name == name) return target;

            for (int i = 0; i < target.childCount; ++i)
            {
                var result = Search(target.GetChild(i), name);

                if (result != null) return result;
            }

            return null;
        }
    }
    public class Hi5_Hand_Visible_Hand : MonoBehaviour
    {
        public HI5.HI5_VIVEInstance mGlove_Hand;

        public bool IsFollowGlovHand
        {
            set { mIsFollowGlovHand = value; }
            get { return mIsFollowGlovHand; }
        }
        private bool mIsFollowGlovHand = true;
        internal Color orgColor;
        #region Bind Hand finger bone
        /// <summary>
        /// Optional Variables that may be assigned if m_AutoAssignHandRig is false
        /// </summary>
        public Transform armTransform;
        public Transform handTransform;
        public List<Transform> m_ThumbFingerTransforms;
        public List<Transform> m_IndexFingerTransforms;
        public List<Transform> m_MiddleFingerTransforms;
        public List<Transform> m_RingFingerTransforms;
        public List<Transform> m_PinkyFingerTransforms;

        public Transform renderTransform;
        internal Transform palm;
		internal Hi5_Hand_Palm_Move palmMove = null;
        public bool m_IsLeftHand = true;
        private string PrefixName = "Human_";
        private string RighthandInPrefix = "RightHand";
        private string LefthandInPrefix = "LeftHand";
        private string RightInhandInPrefix = "RightInHand";
        private string LeftInhandInPrefix = "LeftInHand";


        internal Dictionary<Hi5_Glove_Interaction_Finger_Type, Hi5_Hand_Visible_Finger> mFingers = new Dictionary<Hi5_Glove_Interaction_Finger_Type, Hi5_Hand_Visible_Finger>();
        internal void SetThumbAndIndexFingerCollider(bool isOpen)
        {
            mFingers[Hi5_Glove_Interaction_Finger_Type.EThumb].OpenCollider(isOpen);
            mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].OpenCollider(isOpen);
        }

		internal int Pinch2ObjectId()
		{
			if (mGlove_Hand != null)
				return mGlove_Hand.gameObject.GetComponent<Hi5_Glove_Interaction_Hand> ().Pinch2ObjectId ();
			else
				return -1000;
			
		}

		internal int PinchObjectId()
		{
			if (mGlove_Hand != null)
				return mGlove_Hand.gameObject.GetComponent<Hi5_Glove_Interaction_Hand> ().PinchObjectId ();
			else
				return -1000;
		}

		internal int LiftObjectId()
		{
			if (mGlove_Hand != null)
				return mGlove_Hand.gameObject.GetComponent<Hi5_Glove_Interaction_Hand> ().LiftObjectId ();
			else
				return -1000;
		}

        private void AssignPNJoints()
        {
            Transform rightHand = transform.Search(PrefixName + RighthandInPrefix);
            Transform leftHand = transform.Search(PrefixName + LefthandInPrefix);

            if (!m_IsLeftHand)
            {
                AssignPNHandJoints(rightHand, PrefixName + RighthandInPrefix, PrefixName + RightInhandInPrefix);
            }
            else
            {
                AssignPNHandJoints(leftHand, PrefixName + LefthandInPrefix, PrefixName + LeftInhandInPrefix);
            }
        }

        private void AssignPNHandJoints(Transform handBase, string handPrefix, string handIn)
        {
            mFingers.Clear();
            Transform temp = handBase.Search(handPrefix + "Thumb1");
            m_ThumbFingerTransforms.Add(handBase.Search(handPrefix + "Thumb1"));
            m_ThumbFingerTransforms.Add(m_ThumbFingerTransforms[0].Find(handPrefix + "Thumb2"));
            m_ThumbFingerTransforms.Add(m_ThumbFingerTransforms[1].Find(handPrefix + "Thumb3"));
            m_ThumbFingerTransforms.Add(m_ThumbFingerTransforms[2].Find(handPrefix + "Thumb4"));
            Hi5_Hand_Visible_Finger tempFingerinteraction = temp.GetComponent<Hi5_Hand_Visible_Thumb_Finger>() as Hi5_Hand_Visible_Finger;
            if (tempFingerinteraction != null)
            {
                Hi5_Hand_Visible_Finger tempValue = tempFingerinteraction as Hi5_Hand_Visible_Finger;
                mFingers.Add(Hi5_Glove_Interaction_Finger_Type.EThumb, tempValue);
                tempFingerinteraction.AddChildNode(m_ThumbFingerTransforms, this);
                //tempFingerinteraction.SetHi5Message(mMessage);
            }

            temp = handBase.Search(handIn + "Index");
            m_IndexFingerTransforms.Add(handBase.Search(handPrefix + "Index1"));
            m_IndexFingerTransforms.Add(m_IndexFingerTransforms[0].Find(handPrefix + "Index2"));
            m_IndexFingerTransforms.Add(m_IndexFingerTransforms[1].Find(handPrefix + "Index3"));
            m_IndexFingerTransforms.Add(m_IndexFingerTransforms[2].Find(handPrefix + "Index4"));
            tempFingerinteraction = temp.GetComponent<Hi5_Hand_Visible_Finger>();
            if (tempFingerinteraction != null)
            {
                mFingers.Add(Hi5_Glove_Interaction_Finger_Type.EIndex, tempFingerinteraction);
                tempFingerinteraction.AddChildNode(m_IndexFingerTransforms, this);
                //tempFingerinteraction.SetHi5Message(mMessage);
            }

            temp = handBase.Search(handIn + "Middle");
            m_MiddleFingerTransforms.Add(handBase.Search(handPrefix + "Middle1"));
            m_MiddleFingerTransforms.Add(m_MiddleFingerTransforms[0].Find(handPrefix + "Middle2"));
            m_MiddleFingerTransforms.Add(m_MiddleFingerTransforms[1].Find(handPrefix + "Middle3"));
            m_MiddleFingerTransforms.Add(m_MiddleFingerTransforms[2].Find(handPrefix + "Middle4"));
            tempFingerinteraction = temp.GetComponent<Hi5_Hand_Visible_Finger>();
            if (tempFingerinteraction != null)
            {
                mFingers.Add(Hi5_Glove_Interaction_Finger_Type.EMiddle, tempFingerinteraction);
                tempFingerinteraction.AddChildNode(m_MiddleFingerTransforms, this);
                //tempFingerinteraction.SetHi5Message(mMessage);
            }

            temp = handBase.Search(handIn + "Ring");
            m_RingFingerTransforms.Add(handBase.Search(handPrefix + "Ring1"));
            m_RingFingerTransforms.Add(m_RingFingerTransforms[0].Find(handPrefix + "Ring2"));
            m_RingFingerTransforms.Add(m_RingFingerTransforms[1].Find(handPrefix + "Ring3"));
            m_RingFingerTransforms.Add(m_RingFingerTransforms[2].Find(handPrefix + "Ring4"));
            tempFingerinteraction = temp.GetComponent<Hi5_Hand_Visible_Finger>();
            if (tempFingerinteraction != null)
            {
                mFingers.Add(Hi5_Glove_Interaction_Finger_Type.ERing, tempFingerinteraction);
                tempFingerinteraction.AddChildNode(m_RingFingerTransforms, this);
            }

            temp = handBase.Search(handIn + "Pinky");
            m_PinkyFingerTransforms.Add(handBase.Search(handPrefix + "Pinky1"));
            m_PinkyFingerTransforms.Add(m_PinkyFingerTransforms[0].Find(handPrefix + "Pinky2"));
            m_PinkyFingerTransforms.Add(m_PinkyFingerTransforms[1].Find(handPrefix + "Pinky3"));
            m_PinkyFingerTransforms.Add(m_PinkyFingerTransforms[2].Find(handPrefix + "Pinky4"));
            tempFingerinteraction = temp.GetComponent<Hi5_Hand_Visible_Finger>();
            if (tempFingerinteraction != null)
            {
                mFingers.Add(Hi5_Glove_Interaction_Finger_Type.EPinky, tempFingerinteraction);
                tempFingerinteraction.AddChildNode(m_PinkyFingerTransforms, this);
            }
        }
        #endregion


        #region unity system
        void Awake()
        {
            if (mGlove_Hand != null)
                mGlove_Hand.gameObject.GetComponent<Hi5_Glove_Interaction_Hand>().mVisibleHand = this;
            palm = transform.GetComponentInChildren<Hi5_Hand_Palm>().transform;
            palm.GetComponent<Hi5_Hand_Palm>().mHand = this;
			palmMove = transform.GetComponentInChildren<Hi5_Hand_Palm_Move>();
            orgColor = renderTransform.GetComponent<Renderer>().material.color;
            AssignPNJoints();
            NailColliderCount = 0;
           // SetBoxColliderActive(false);
        }
        #endregion

        private void Update()
        {
            if (mGlove_Hand != null)
                mGlove_Hand.gameObject.GetComponent<Hi5_Glove_Interaction_Hand>().mVisibleHand = this;
            UpdateBone();
        }

        internal Hi5_Glove_Interaction_Hand  GetHand()
        {
            if (mGlove_Hand == null)
                return null;
            return mGlove_Hand.gameObject.GetComponent<Hi5_Glove_Interaction_Hand>();
        }

        internal void SetBoxColliderActive(bool isActive)
        {
            BoxCollider[] array = gameObject.GetComponentsInChildren<BoxCollider>();
            for (int i = 0; i < array.Length; i++)
            {
                array[i].enabled = isActive;
                Destroy(array[i].GetComponent<Rigidbody>());
               
            }
                
        }
        private void FixedUpdate()
		{
		}

        internal void ReleaseFingerFollow()
        {
            //mFingers[Hi5_Glove_Interaction_Finger_Type.EThumb].RealeaseFingerFollow();
            //mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].RealeaseFingerFollow();
            //mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].RealeaseFingerFollow();
            //mFingers[Hi5_Glove_Interaction_Finger_Type.ERing].RealeaseFingerFollow();
            //mFingers[Hi5_Glove_Interaction_Finger_Type.EPinky].RealeaseFingerFollow();
        }
        private void UpdateBone()
        {
            ///bool isMove = true;
            //if (mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].isLockFingerRotation()
            //    || mFingers[Hi5_Glove_Interaction_Finger_Type.ERing].isLockFingerRotation()
            //    || mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].isLockFingerRotation())
            //    isMove = false;
            if (mGlove_Hand != null)
            {
           
                Transform[] bones = mGlove_Hand.HandBones;
                if (armTransform != null )
                {
                    armTransform.position = bones[0].position;
                    armTransform.rotation = bones[0].rotation;
                }
                if (handTransform != null)
                {
                    handTransform.position = bones[1].position;
                    handTransform.rotation = bones[1].rotation;
                }
                //Thumb
                List<Transform> thumbs = new List<Transform>();
                thumbs.Add(bones[2]);
                thumbs.Add(bones[3]);
                thumbs.Add(bones[4]);
				if(mFingers.ContainsKey(Hi5_Glove_Interaction_Finger_Type.EThumb))
                	mFingers[Hi5_Glove_Interaction_Finger_Type.EThumb].ApplyFingerRotation(thumbs);
                //Index
                List<Transform> indexs = new List<Transform>();
                indexs.Add(bones[5]);
                indexs.Add(bones[6]);
                indexs.Add(bones[7]);
                indexs.Add(bones[8]);
				if(mFingers.ContainsKey(Hi5_Glove_Interaction_Finger_Type.EIndex))
                	mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].ApplyFingerRotation(indexs);
                //Middle
                List<Transform> middles = new List<Transform>();
                middles.Add(bones[9]);
                middles.Add(bones[10]);
                middles.Add(bones[11]);
                middles.Add(bones[12]);
				if(mFingers.ContainsKey(Hi5_Glove_Interaction_Finger_Type.EMiddle))
                	mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].ApplyFingerRotation(middles);
                //Ring
                List<Transform> rings = new List<Transform>();
                rings.Add(bones[13]);
                rings.Add(bones[14]);
                rings.Add(bones[15]);
                rings.Add(bones[16]);
				if(mFingers.ContainsKey(Hi5_Glove_Interaction_Finger_Type.ERing))
               		mFingers[Hi5_Glove_Interaction_Finger_Type.ERing].ApplyFingerRotation(rings);
                //Pinky
                List<Transform> pinks = new List<Transform>();
                pinks.Add(bones[17]);
                pinks.Add(bones[18]);
                pinks.Add(bones[19]);
                pinks.Add(bones[20]);
				if(mFingers.ContainsKey(Hi5_Glove_Interaction_Finger_Type.EPinky))
                	mFingers[Hi5_Glove_Interaction_Finger_Type.EPinky].ApplyFingerRotation(pinks);
            }
        }

        public Hi5_Glove_Interaction_Hand GetGloveHand()
        {
            return mGlove_Hand.GetComponent<Hi5_Glove_Interaction_Hand>();
        }

        internal void ChangeColor(Color color)
        {

            renderTransform.GetComponent<Renderer>().material.color = color;
        }

        private int NailColliderCount = 0;
        internal void AddNailColliderCount()
        {
            NailColliderCount++;
            SetBoxColliderActive(false);
        }

        internal void RemoveNailColliderCount()
        {
            NailColliderCount--;
            if (NailColliderCount <= 0)
                SetBoxColliderActive(true);
        }
    }
}

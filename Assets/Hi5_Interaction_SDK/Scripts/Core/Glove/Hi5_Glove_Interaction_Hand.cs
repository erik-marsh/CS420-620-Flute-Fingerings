using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Hi5_Interaction_Core
{
   // using FingerType = Hi5_Glove_Interaction_Finger_Type;
    public class Hi5_Glove_Interaction_Hand : MonoBehaviour
    {
        public  Hi5_Hand_Visible_Hand mVisibleHand = null;
        internal int mPinchObjectId;
        //internal int mPinch2ObjectId;
        #region Bind Hand finger bone
        /// <summary>
        /// Optional Variables that may be assigned if m_AutoAssignHandRig is false
        /// </summary>
        public List<Transform> m_ThumbFingerTransforms;
        public List<Transform> m_IndexFingerTransforms;
        public List<Transform> m_MiddleFingerTransforms;
        public List<Transform> m_RingFingerTransforms;
        public List<Transform> m_PinkyFingerTransforms;
        internal Hi5_Glove_Collider_Palm mPalm = null;
		internal Hi5_Glove_Hand_Fly_Collider mHandFlyCollider = null;
		internal Hi5_Glove_Hand_Collider mHandCollider = null;
		public  Transform MoveAnchor; 
		public bool m_IsLeftHand = true;
        private string PrefixName = "Human_";
        private string RighthandInPrefix = "RightHand";
        private string LefthandInPrefix = "LeftHand";
        private string RightInhandInPrefix = "RightInHand";
        private string LeftInhandInPrefix = "LeftInHand";
		public E_Hand_State mCurrentState = E_Hand_State.ERelease;
       
        internal Transform PinchObject;
        internal Dictionary<Hi5_Glove_Interaction_Finger_Type, Hi5_Glove_Interaction_Finger> mFingers = new Dictionary<Hi5_Glove_Interaction_Finger_Type, Hi5_Glove_Interaction_Finger>();

		internal Hi5_Glove_Gesture_Recognition mGestureRecognition;
		internal int Pinch2ObjectId()
		{
			if (mState == null)
				return -1000;
			if (mState.State == E_Hand_State.EPinch2) {
				return mState.GetPinch2 ().objectId;
			}
			else
				return -1000;
		}

		internal int PinchObjectId()
		{
			if (mState == null)
				return -1000;
			if (mState.State == E_Hand_State.EPinch) {
				return mState.GetPinch().objectId;
			}
			else
				return -1000;
		}

		internal int LiftObjectId()
		{
			if (mState == null)
				return -1000;
			if (mState.State == E_Hand_State.ELift) {
				return mState.GetLift ().objectId;
			}
			else
				return -1000;
		}
		virtual protected void OnEnable()
		{
			//Hi5_Interaction_Message.GetInstance().RegisterMessage(PichOtherHandRelease, Hi5_MessageKey.messageLiftObject);

			//m_TriggerCount.Clear();
		}
		virtual protected void OnDisable()
		{
			//Hi5_Interaction_Message.GetInstance().RegisterMessage(PichOtherHandRelease, Hi5_MessageKey.messagePinchObject);
		}


		void PinchOtherHandRelease(string messageKey, object param1, object param2, object param3, object param4)
		{
			if (messageKey.CompareTo (Hi5_MessageKey.messagePinchOtherHandRelease) == 0) 
			{
				Hi5_Glove_Interaction_Hand hand = param1 as Hi5_Glove_Interaction_Hand;
				if (this != hand) {
					mState.ChangeState (E_Hand_State.ERelease);
					mPinchObjectId = -1;
				}
			}
		}

        private void AssignPNJoints()
        {
            Transform rightHand = transform.Search(PrefixName + RighthandInPrefix);
            Transform leftHand = transform.Search(PrefixName + LefthandInPrefix);
            if (!m_IsLeftHand)
            {
                AssignPNHandJoints(rightHand, PrefixName + RighthandInPrefix, PrefixName+RightInhandInPrefix);
            }
            else
            {
                AssignPNHandJoints(leftHand, PrefixName + LefthandInPrefix, PrefixName + LeftInhandInPrefix);
            }
        }

        private void AssignPNHandJoints(Transform handBase, string handPrefix,string handIn)
        {
            mFingers.Clear();
            Transform  temp =  handBase.Search(handPrefix + "Thumb1");
            m_ThumbFingerTransforms.Add(handBase.Search(handPrefix + "Thumb1"));
            m_ThumbFingerTransforms.Add(m_ThumbFingerTransforms[0].Find(handPrefix + "Thumb2"));
            m_ThumbFingerTransforms.Add(m_ThumbFingerTransforms[1].Find(handPrefix + "Thumb3"));
            m_ThumbFingerTransforms.Add(m_ThumbFingerTransforms[2].Find(handPrefix + "Thumb4"));
            Hi5_Glove_Interaction_Finger tempFingerinteraction = temp.GetComponent<Hi5_Glove_Interaction_Finger>();
            if (tempFingerinteraction != null)
            {
                mFingers.Add(Hi5_Glove_Interaction_Finger_Type.EThumb, tempFingerinteraction);
                tempFingerinteraction.AddChildNode(m_ThumbFingerTransforms);
                tempFingerinteraction.SetHi5Hand(this);
            }
                

            temp = handBase.Search(handIn + "Index");
            m_IndexFingerTransforms.Add(handBase.Search(handPrefix + "Index1"));
            m_IndexFingerTransforms.Add(m_IndexFingerTransforms[0].Find(handPrefix + "Index2"));
            m_IndexFingerTransforms.Add(m_IndexFingerTransforms[1].Find(handPrefix + "Index3"));
            m_IndexFingerTransforms.Add(m_IndexFingerTransforms[2].Find(handPrefix + "Index4"));
            tempFingerinteraction = temp.GetComponent<Hi5_Glove_Interaction_Finger>();
            if (tempFingerinteraction != null)
            {
                mFingers.Add(Hi5_Glove_Interaction_Finger_Type.EIndex, tempFingerinteraction);
                tempFingerinteraction.AddChildNode(m_IndexFingerTransforms);
                tempFingerinteraction.SetHi5Hand(this);
                //tempFingerinteraction.SetHi5Message(mMessage, this);
            }


            temp = handBase.Search(handIn + "Middle");
            m_MiddleFingerTransforms.Add(handBase.Search(handPrefix + "Middle1"));
            m_MiddleFingerTransforms.Add(m_MiddleFingerTransforms[0].Find(handPrefix + "Middle2"));
            m_MiddleFingerTransforms.Add(m_MiddleFingerTransforms[1].Find(handPrefix + "Middle3"));
            m_MiddleFingerTransforms.Add(m_MiddleFingerTransforms[2].Find(handPrefix + "Middle4"));
            tempFingerinteraction = temp.GetComponent<Hi5_Glove_Interaction_Finger>();
            if (tempFingerinteraction != null)
            {
                mFingers.Add(Hi5_Glove_Interaction_Finger_Type.EMiddle, tempFingerinteraction);
                tempFingerinteraction.AddChildNode(m_MiddleFingerTransforms);
                tempFingerinteraction.SetHi5Hand(this);
                //tempFingerinteraction.SetHi5Message(mMessage, this);
            }


            temp = handBase.Search(handIn + "Ring");
            m_RingFingerTransforms.Add(handBase.Search(handPrefix + "Ring1"));
            m_RingFingerTransforms.Add(m_RingFingerTransforms[0].Find(handPrefix + "Ring2"));
            m_RingFingerTransforms.Add(m_RingFingerTransforms[1].Find(handPrefix + "Ring3"));
            m_RingFingerTransforms.Add(m_RingFingerTransforms[2].Find(handPrefix + "Ring4"));
            tempFingerinteraction = temp.GetComponent<Hi5_Glove_Interaction_Finger>();
            if (tempFingerinteraction != null)
            {
                mFingers.Add(Hi5_Glove_Interaction_Finger_Type.ERing, tempFingerinteraction);
                tempFingerinteraction.AddChildNode(m_RingFingerTransforms);
                tempFingerinteraction.SetHi5Hand(this);
                //tempFingerinteraction.SetHi5Message(mMessage, this);
            }

            temp = handBase.Search(handIn + "Pinky");
            m_PinkyFingerTransforms.Add(handBase.Search(handPrefix + "Pinky1"));
            m_PinkyFingerTransforms.Add(m_PinkyFingerTransforms[0].Find(handPrefix + "Pinky2"));
            m_PinkyFingerTransforms.Add(m_PinkyFingerTransforms[1].Find(handPrefix + "Pinky3"));
            m_PinkyFingerTransforms.Add(m_PinkyFingerTransforms[2].Find(handPrefix + "Pinky4"));
            tempFingerinteraction = temp.GetComponent<Hi5_Glove_Interaction_Finger>();
            if (tempFingerinteraction != null)
            {
                mFingers.Add(Hi5_Glove_Interaction_Finger_Type.EPinky, tempFingerinteraction);
                tempFingerinteraction.AddChildNode(m_PinkyFingerTransforms);
                tempFingerinteraction.SetHi5Hand(this);
                //tempFingerinteraction.SetHi5Message(mMessage, this);
            }
        }
        #endregion
        internal List<Collider> IsPalmTrigger()
        {
            List<Collider>  colliders = mPalm.GetTriggers();
            return colliders;
        }
        #region unity system
        void Awake()
        {
			mGestureRecognition = new Hi5_Glove_Gesture_Recognition (this);
            //mMessage = new Hi5_Glove_Interaction_Message();
            AssignPNJoints();
            mPalm = gameObject.GetComponentInChildren<Hi5_Glove_Collider_Palm>();
            mPalm.mHand = this;
			mHandFlyCollider = gameObject.GetComponentInChildren<Hi5_Glove_Hand_Fly_Collider>();
			mHandCollider  = gameObject.GetComponentInChildren<Hi5_Glove_Hand_Collider>();
            //mPalm.SetHi5Message(mMessage);
            mState = Hi5_Glove_Interaction_State.CreateState(this);
            mPinchObjectId = -1;
        }

        void Start()
        {
            
        }

        void Update()
        {
			if (mGestureRecognition != null) {
				mGestureRecognition.Update (Time.deltaTime);
			}
            if (Hi5_Interaction_Const.IsUseVisibleHand)
            {
                gameObject.GetComponent<HI5.HI5_VIVEInstance>().isVisible = true;
            }
            else
                gameObject.GetComponent<HI5.HI5_VIVEInstance>().isVisible = false;
          
           
            if (mState != null)
                mState.Update(Time.deltaTime);
        }

        private void FixedUpdate()
        {
             if(mState != null)
                mState.FixedUpdate(Time.deltaTime);
        }
        #endregion

        #region Hand state
        internal Hi5_Glove_Interaction_State mState = null;
        #endregion

        #region Glove_Message
        //internal Hi5_Glove_Interaction_Message mMessage = null;
        #endregion

        internal void AddPinchObject(Transform pinchObject, Transform transformParent)
        {
            if (mPalm != null && pinchObject.GetComponent<Hi5_Glove_Interaction_Item>() != null)
            {

                //PinchObject = pinchObject.parent;
                if (Hi5_Interaction_Const.TestPinch2)
                {
                    pinchObject.parent = transformParent;
                }
                else
                    pinchObject.parent = mPalm.transform;
               // pinchObject.transform.localPosition = Vector3.zero;
                mPinchObjectId = pinchObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
            }
        }

        internal void AddPinch2Object(Transform pinchObject,Transform transformParent)
        {
            if (mPalm != null && pinchObject.GetComponent<Hi5_Glove_Interaction_Item>())
            {
                //PinchObject = pinchObject.parent;
                pinchObject.parent = transformParent;
                //mPinch2ObjectId = pinchObject.GetComponent<Hi5_Glove_Interraction_Item>().idObject;
                mPinchObjectId = pinchObject.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
            }
        }


        internal protected bool IsTriggerObjectById(Hi5_Glove_Interaction_Finger_Type finger_type)
        {
            
            return mFingers[finger_type].IsCollider();
        }

        internal Vector3 GetThumbAndMiddlePoint()
        {
            Vector3 point = new Vector3((m_ThumbFingerTransforms[2].position.x + m_MiddleFingerTransforms[2].position.x) / 2,
                (m_ThumbFingerTransforms[2].position.y + m_MiddleFingerTransforms[2].position.y) / 2,
                (m_ThumbFingerTransforms[2].position.z + m_MiddleFingerTransforms[2].position.z) / 2);
            return point;
        }

        internal Hi5_Glove_Gesture_Recognition_State GetRecognitionState()
        {
           return  mGestureRecognition.GetRecognitionState();
        }
    }

}

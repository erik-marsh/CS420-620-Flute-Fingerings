using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hi5_Interaction_Core
{
	public class Hi5_Glove_State_Clap : Hi5_Glove_State_Base
	{
		internal int objectId = -1000;
		//private float cd = Hi5_Interaction_Const.Clapcd;
		public override void Start()
		{
			//cd = Hi5_Interaction_Const.Clapcd;
		}

		public override void FixedUpdate(float deltaTime)
		{

		}

		public override void Update(float deltaTime)
		{
			if (Hand == null || Hand.mState == null || Hand.mState.mJudgeMent == null || mDecision == null)
				return;
			if (mDecision.IsPinch ())
			{
				return;
			}

			if (mDecision.IsPinch2 ())
			{
				return;
			}

		}



		private bool IsRealese()
		{
			Hi5_Glove_Interaction_Item itemObject = Hi5_Interaction_Object_Manager.GetObjectManager().GetItemById(objectId);
			if (itemObject == null)
				return false;
			List<Collider>  triggers =  itemObject.GetTriggers();
			if(triggers != null  && triggers.Count>0)
			{
				foreach (Collider item in triggers)
				{
					if (item.gameObject.GetComponent<Hi5_Glove_Collider_Finger>() != null
						&& item.gameObject.GetComponent<Hi5_Glove_Collider_Finger>().mFinger != null
						&& item.gameObject.GetComponent<Hi5_Glove_Collider_Finger>().mFinger.mHand != null)
					{
						
						if (Hand.m_IsLeftHand)
						{
							if (item.gameObject.GetComponent<Hi5_Glove_Collider_Finger>().mFinger.mHand.m_IsLeftHand)
							{
								return false;
							}
						}
						else 
						{
							if (!item.gameObject.GetComponent<Hi5_Glove_Collider_Finger>().mFinger.mHand.m_IsLeftHand)
							{
								return false;
							}
						}

					}
					else if (item.gameObject.GetComponent<Hi5_Glove_Collider_Palm>() != null
						&& item.gameObject.GetComponent<Hi5_Glove_Collider_Palm>().mHand != null)
					{
						if (Hand.m_IsLeftHand)
						{
							if (item.gameObject.GetComponent<Hi5_Glove_Collider_Palm>().mHand.m_IsLeftHand)
							{
								return false;
							}
						}
						else if (Hand.m_IsLeftHand)
						{
							if (!item.gameObject.GetComponent<Hi5_Glove_Collider_Palm>().mHand.m_IsLeftHand)
							{ 
								return false;
							}
						}

					}
				}
			}
			return true;
		}



		public override void End()
		{
			
		}
	}
}



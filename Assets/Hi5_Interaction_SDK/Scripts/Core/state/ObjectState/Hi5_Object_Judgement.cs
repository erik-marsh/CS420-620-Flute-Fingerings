using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Hi5_Interaction_Core
{
    public class Hi5_Object_JudgeMent
    {
        internal  Hi5_Glove_Interaction_Hand mHand = null;
        internal Hi5_Glove_Interaction_State mStateManager = null;
        internal protected Hi5_Glove_Interaction_Hand Hand
        {
            set { mHand = value; }
            get { return mHand; }
        }

        #region IsPinch
        internal bool IsPinch(out List<int> pinchs,out int ObjectId)
        {
            List<int> pinch1s;
            int ObjectIdtemp = -1;
            bool isPinch = IsPinchOne(out pinch1s, out ObjectIdtemp);
            if (isPinch)
            {
				//Debug.Log ("IsPinch");
                pinchs = pinch1s;
                ObjectId = ObjectIdtemp;
                return true;
            }
            else
            {
//				List<int> pinch2s;
//				isPinch = IsPalmPinch(out pinch2s, out ObjectIdtemp);
//				if (isPinch) {
//					pinchs = pinch2s;
//					ObjectId = ObjectIdtemp;
//					return true;
//				}
//				else
//				{
					pinchs = null;
					ObjectId = -1;
					return false;
				//}
               
            }
        }

        private bool IsPinchOne(out List<int> pinchs, out int ObjectId)
        {
            bool resultbool = false;
            List<int> thumbs;
            bool thumb = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EThumb].IsPinchTrigger(out thumbs);
            List<int> indexs;
            bool index = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].IsPinchTrigger(out indexs);
            List<int> mids;
            bool mid = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].IsPinchTrigger(out mids);
            List<int> rings;
            bool ring = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.ERing].IsPinchTrigger(out rings);

            List<int> results = null;
            if (thumb)
            {
                if (index && mid)
                {
                    List<int> intersects = indexs.Intersect(mids).ToList();
                    if (intersects != null && intersects.Count > 0)
                    {
                        results = thumbs.Intersect(intersects).ToList();
                    }
                }
                else if (ring && mid)
                {
                    List<int> intersects = rings.Intersect(mids).ToList();
                    if (intersects != null && intersects.Count > 0)
                    {
                        results = thumbs.Intersect(intersects).ToList();
                    }
                }
            }
            int distanceminId = -1;
            if (results != null && results.Count > 0)
            {
                List<int> listresult = new List<int>();
                if (Hand.mPalm != null)
                {
                    List<int> removeIds = new List<int>();
                    foreach (int id in results)
                    {
                        Transform temp = null;
                        //mPalm.JudgeObjectHandInside();
                        List<Hi5_Glove_Interaction_Item> array = new List<Hi5_Glove_Interaction_Item>();
                        Hi5_Interaction_Message.GetInstance().DispenseMessage(Hi5_MessageKey.messageGetObjecById, (object)id, (object)array);

                        if (array != null && array.Count > 0)
                        {
                            Transform value = array[0].transform;
                            if (!Hand.mPalm.JudgeObjectHandInside(value))
                                removeIds.Add(id);
                        }
                        //else
                        //    Debug.Log("temp = empty");
                    }

                    listresult = results.Except(removeIds).ToList();
                }
                float distanceMin = 20.0f;
                


                for (int i = 0; i < listresult.Count; i++)
                {
                    int objectIdTemp = listresult[i];
                    Hi5_Glove_Interaction_Item item = Hi5_Interaction_Object_Manager.GetObjectManager().GetItemById(objectIdTemp);
                    if (item != null)
                    {
                        bool ContactIsSelf = false;
                        float distance = Hi5_Interaction_Const.GetDistance(mHand.mPalm.transform, item, out ContactIsSelf);
                        if (item.gameObject.GetComponent<Hi5_Object_Property>() != null)
                        {
                            if (!item.gameObject.GetComponent<Hi5_Object_Property>().IsPinch)
                            {
                               // Debug.Log("isPinch");
                                ContactIsSelf = false;
                            }
                            else
                                ContactIsSelf = true;

                        }
                        if (ContactIsSelf)
                        {
                            if (distance < distanceMin)
                            {
                                distanceMin = distance;
                                distanceminId = objectIdTemp;
                            }
                        }
                    }
                }
                //没用

                if (listresult != null && listresult.Count > 0)
                {
                    pinchs = results;
                    if (thumb)
                        Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EThumb].NotifyEnterPinchState();
                    if (index)
                        Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].NotifyEnterPinchState();
                    if (mid)
                        Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].NotifyEnterPinchState();
                    if (ring)
                        Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.ERing].NotifyEnterPinchState();
                    resultbool = true;
                }
                else
                    pinchs = null;
            }
            else
                pinchs = null;

            if (distanceminId != -1)
            {
                ObjectId = distanceminId;
                return true;
            }
            else
            {
                ObjectId = -1;
                return false;
            }
         
        }

        //        private bool IsPalmPinch(out List<int> pinchs, out int ObjectId)
        //        {
        //            List<Collider> colliders = Hand.IsPalmTrigger();
        //            if (colliders == null)
        //            {
        //                pinchs = null;
        //                ObjectId = -1;
        //                return false;
        //            }
        //
        //            int idObject = -10000;
        //            List<int> temp = new List<int>();
        //            foreach (Collider item in colliders)
        //            {
        //                if (item.GetComponent<Hi5_Glove_Interaction_Item>() != null)
        //                {
        //                    idObject = item.GetComponent<Hi5_Glove_Interaction_Item>().idObject;
        //                    if (Hand.mPalm.JudgeObjectHandInside(item.transform))
        //                        temp.Add(idObject);
        //                }
        //            }
        //            if (idObject == -10000)
        //            {
        //                pinchs = null;
        //                ObjectId = -1;
        //                return false;
        //            }
        //
        //            List<int> thumbs;
        //            bool thumb = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EThumb].IsPinchTrigger(out thumbs);
        //            if (thumb)
        //            {
        //                Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EThumb].NotifyEnterPinchState();
        //                int count = 0;
        //                if (Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].PalmPinch())
        //                {
        //                    ///Debug.Log("Hi5_Glove_Interaction_Finger_Type.EIndex");
        //                    Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].NotifyEnterPinchState();
        //                    count++;
        //                }
        //
        //                if (Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].PalmPinch())
        //                {
        //                    Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].NotifyEnterPinchState();
        //                    //Debug.Log("Hi5_Glove_Interaction_Finger_Type.EMiddle");
        //                    count++;
        //                }
        //
        //                if (Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.ERing].PalmPinch())
        //                {
        //                    Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.ERing].NotifyEnterPinchState();
        //                    //Debug.Log("Hi5_Glove_Interaction_Finger_Type.ERing");
        //                    count++;
        //                }
        //
        //                if (Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EPinky].PalmPinch())
        //                {
        //                    Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EPinky].NotifyEnterPinchState();
        //                    // Debug.Log("Hi5_Glove_Interaction_Finger_Type.EPinky");
        //                    count++;
        //                }
        //                if (count > 2)
        //                {
        //                    pinchs = temp;
        //                    float distanceMin = 20.0f;
        //                    int distanceminId = -1;
        //                    for (int i = 0; i < temp.Count; i++)
        //                    {
        //                        int objectIdTemp = temp[i];
        //                        Hi5_Glove_Interaction_Item item = Hi5_Interaction_Object_Manager.GetObjectManager().GetItemById(objectIdTemp);
        //                        if (item != null)
        //                        {
        //                            bool ContactIsSelf = false;
        //                            float distance = Hi5_Interaction_Const.GetDistance(mHand.mPalm.transform, item, out ContactIsSelf);
        //                            if (ContactIsSelf)
        //                            {
        //                                if (distance < distanceMin)
        //                                {
        //                                    distanceMin = distance;
        //                                    distanceminId = objectIdTemp;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    ObjectId = distanceminId;
        //                    return true;
        //                }
        //                else
        //                {
        //                    pinchs = null;
        //                    ObjectId = -1;
        //                    return false;
        //                }
        //            }
        //            else
        //            {
        //                pinchs = null;
        //                ObjectId = -1;
        //                return false;
        //            }
        //            
        //        }
        #endregion

        #region IsFlyIngPinch


        internal bool IsGesturePinch2()
        {

            Vector3 tempThumb = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EThumb].GetTailPosition();
            Vector3 tempIndex = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].GetTailPosition();
            bool IndexPinch = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].IsFlyRunPinch();
            bool IndexMiddle =   Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].IsFlyRunPinch();
            Vector3 tempMiddle = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].GetTailPosition();
            float distance1 = Vector3.Distance(tempThumb, tempIndex);
            float distance2 = Vector3.Distance(tempThumb, tempMiddle);
            if ((distance1 < 0.080f && IndexPinch) || (distance2 < 0.080f && IndexMiddle))
            {
                return true;

            }
            else
                return false;
            //bool ThumbPinch = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EThumb].IsFlyRunPinch();
            ////bool ThumbPinch = !Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EThumb].IsPinch2Realease(Hi5_Interaction_Const.FingerPich2Release);
            //bool IndexPinch = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].IsFlyRunPinch();
            ////bool IndexPinch = !Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].IsPinch2Realease(Hi5_Interaction_Const.FingerPich2Release);
            //bool MiddlePinch = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].IsFlyRunPinch();
            ////bool MiddlePinch = !Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].IsPinch2Realease(Hi5_Interaction_Const.FingerPich2Release);
            //if (IndexPinch || MiddlePinch)
            //    return true;
            //else
            //    return false;
        }
		//手势姿势判断
		internal bool IsGestureFlyPinch()
		{
			int count = 0;
			bool thumb = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EThumb].IsFlyRunPinch();
			//Debug.Log("Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EThumb] angel "+ Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EThumb].flyPinch);
			if (thumb)
			{
				count++;
			}
			bool index = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].IsFlyRunPinch();
			//Debug.Log("Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex] angel " + Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].flyPinch);
			if (index)
			{
				count++;
			}

			bool middle = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].IsFlyRunPinch();
			//Debug.Log("Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle] angel " + Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].flyPinch);
			if (middle)
			{
				count++;

			}

            bool ring = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.ERing].IsFlyRunPinch();
            // Debug.Log("Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.ring] angel " + Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.ERing].flyPinch);
            if (ring)
            {
                count++;

            }

            bool pinky = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EPinky].IsFlyRunPinch();
			if (pinky)
			{
				count++;

			}
			if (count >= 3) {
				return true;
			} else
				return false;
		}

         internal bool IsFlyIngPinch(out List<int> results,out int ObjectId)
        {
			//if (Hi5_Interaction_Const.TestFlyPinch)
            {
				bool IsCollider = false;
				List<int> handList;

				if (Hand.mHandFlyCollider.IsFlyPinch (out handList)) {
					IsCollider = true;
				}
				if (!IsCollider) 
				{
					Hand.mHandFlyCollider.IsColliderSurround (out handList);
					IsCollider = true;
				}
				if (IsCollider && handList != null && handList.Count > 0) {
					//if (Hand.mGestureRecognition != null && Hand.mGestureRecognition.IsRecordFlyPinch ())
					if (Hand.mGestureRecognition != null && Hand.mGestureRecognition.IsFlyPinch ())
					{
						results = handList;
						if (results != null)
						{
							float distanceMin = 20.0f;
							int distanceminId = -1;
							for (int i = 0; i < results.Count; i++)
							{
								int objectIdTemp = results [i];
								Hi5_Glove_Interaction_Item item = Hi5_Interaction_Object_Manager.GetObjectManager ().GetItemById (objectIdTemp);
								if (item != null && item.state == E_Object_State.EMove &&
									//&& (item.mstatemanager.GetMoveState().mMoveType == Hi5ObjectMoveType.EThrowMove || item.mstatemanager.GetMoveState().mMoveType == Hi5ObjectMoveType.EFree)
									//&&
								    !item.mstatemanager.GetMoveState ().IsProtectionFly ())
								{
									bool ContactIsSelf = false;
									float distance = Hi5_Interaction_Const.GetDistance (mHand.mPalm.transform, item, out ContactIsSelf);
                                    if (item.gameObject.GetComponent<Hi5_Object_Property>() != null)
                                    {
                                        if (!item.gameObject.GetComponent<Hi5_Object_Property>().IsPinch)
                                            ContactIsSelf = false;
                                        else
                                            ContactIsSelf = true;
                                    }
                                    if (ContactIsSelf) {
										if (distance < distanceMin) {
											distanceMin = distance;
											distanceminId = objectIdTemp;
										}
									}
								}
							}
							if (distanceminId != -1) {
								ObjectId = distanceminId;
								//Hi5_Glove_Interraction_Item item = Hi5_Interaction_Object_Manager.GetObjectManager().GetItemById(ObjectId);
								//item.transform.position = Hand.mPalm.transform.position;
								return true;
							} else {
								results = null;
								ObjectId = -1;
								return false;
							}
						} 
						else
						{
							results = null;
							ObjectId = -1;
							return false;
						}
					}
					else 
					{
						results = null;
						ObjectId = -1;
						return false;
					}
				} 
				else
				{
					results = null;
					ObjectId = -1;
					return false;
				}
			}
   //         else
			//{
   //             int count = 0;
   //             List<int> handList;
			//	if (Hand.mHandFlyCollider.IsFlyPinch(out handList))
   //             {
   //                // Debug.Log("Hand.mHandCollider.IsFlyPinch");
   //                 bool thumb = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EThumb].IsFlyPinch;
   //                 //Debug.Log("Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EThumb] angel "+ Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EThumb].flyPinch);
   //                 if (thumb)
   //                 {
   //                     count++;
   //                 }
   //                 bool index = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].IsFlyPinch;
   //                 //Debug.Log("Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex] angel " + Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].flyPinch);
   //                 if (index)
   //                 {
   //                     count++;
   //                 }
                   
   //                 bool middle = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].IsFlyPinch;
   //                 //Debug.Log("Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle] angel " + Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].flyPinch);
   //                 if (middle)
   //                 {
   //                     count++;
                       
   //                 }
               
   //                 bool ring = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.ERing].IsFlyPinch;
   //                // Debug.Log("Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.ring] angel " + Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.ERing].flyPinch);
   //                 if (ring)
   //                 {
   //                     count++;

   //                 }

   //                 bool pinky = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EPinky].IsFlyPinch;
   //                 if (pinky)
   //                 {
   //                     count++;

   //                 }
   //                 if (count >= 2)
   //                 {
   //                     results = handList;
   //                     if (results != null)
   //                     {
   //                         float distanceMin = 20.0f;
   //                         int distanceminId = -1;
   //                         for (int i = 0; i < results.Count; i++)
   //                         {
   //                             int objectIdTemp = results[i];
   //                             Hi5_Glove_Interaction_Item item = Hi5_Interaction_Object_Manager.GetObjectManager().GetItemById(objectIdTemp);
   //                             if (item != null && item.state == E_Object_State.EMove &&
   //                                 //&& (item.mstatemanager.GetMoveState().mMoveType == Hi5ObjectMoveType.EThrowMove || item.mstatemanager.GetMoveState().mMoveType == Hi5ObjectMoveType.EFree)
   //                                 //&&
   //                                 !item.mstatemanager.GetMoveState().IsProtectionFly())
   //                             {
   //                                 bool ContactIsSelf = false;
   //                                 float distance = Hi5_Interaction_Const.GetDistance(mHand.mPalm.transform, item, out ContactIsSelf);
   //                                 if (ContactIsSelf)
   //                                 {
   //                                     if (distance < distanceMin)
   //                                     {
   //                                         distanceMin = distance;
   //                                         distanceminId = objectIdTemp;
   //                                     }
   //                                 }
   //                             }
   //                         }
   //                         if (distanceminId != -1)
   //                         {
   //                             ObjectId = distanceminId;
   //                             //Hi5_Glove_Interraction_Item item = Hi5_Interaction_Object_Manager.GetObjectManager().GetItemById(ObjectId);
   //                             //item.transform.position = Hand.mPalm.transform.position;
   //                             return true;
   //                         }
   //                         else
   //                         {
   //                             results = null;
   //                             ObjectId = -1;
   //                             return false;
   //                         }
   //                     }
   //                     else
   //                     {
   //                         results = null;
   //                         ObjectId = -1;
   //                         return false;
   //                     }
                        

   //                 }
   //                 else
   //                 {
   //                     results = null;
   //                     ObjectId = -1;
   //                     return false;
   //                 }
   //             }
   //             else
   //             {
   //                 results = null;
   //                 ObjectId = -1;
   //                 return false;
   //             }
   //         }
            


        }
        #endregion

        #region IsPinch2
		internal bool IsPinch2(out List<int> results, out List<Hi5_Glove_Interaction_Finger> fingers, out int ObjectId)
        {
			//resultValues = null;
			 results = null;
            List<int> thumbs;
            bool index = false;

           
			List<int> indexcs;
			bool index1 = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].IsPinch2Trigger(out indexcs);
			if (index1 && indexcs != null)
			{
               

			}

            bool thumb = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EThumb].IsPinch2TailTrigger(out thumbs);
            if (thumb && thumbs !=null)
            {
               
                List<int> indexs;
                index = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].IsPinch2Trigger(out indexs);
                if (index && indexs != null)
                {
					

					//foreach (int indexTemp in indexs)
					//{
					//	Debug.Log ("indexIndex" + indexTemp);
					//}
     //               foreach (int indexTemp in thumbs)
     //               {
     //                   Debug.Log("thumbIndex" + indexTemp);
     //               }
                    List<int>  intersects = indexs.Intersect(thumbs).ToList();
                    if (intersects != null)
                    {
                        //foreach (int itemo in intersects)
                        //    Debug.Log("itemo"+ itemo);
                        results = intersects;
                    }
                       
                }

                if (results == null)
                {
					

                    List<int> middles;
                    bool middle = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].IsPinch2Trigger(out middles);
                    if (middle && middles != null)
                    {

                        List<int> intersects = middles.Intersect(thumbs).ToList();
                        if (intersects != null)
                            results = intersects;
                    }
                }
            }


			  
			if (results != null && results.Count>0)
            {
                List<Hi5_Glove_Interaction_Finger>  fingertemps = new List<Hi5_Glove_Interaction_Finger>();
                fingertemps.Add(Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EThumb]);
   
                if (index)
                {
                  
                    fingertemps.Add(Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex]);
                }
                else
                {
                    fingertemps.Add(Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle]);

                }
                float distanceMin = 20.0f;
                int distanceminId = -1;
				for (int i = 0; i < results.Count; i++)
                {
					
					int objectIdTemp = results[i];
                    Hi5_Glove_Interaction_Item item = Hi5_Interaction_Object_Manager.GetObjectManager().GetItemById(objectIdTemp);
                    if (item != null)
                    {
						
                        bool ContactIsSelf = false;
                        float distance = Hi5_Interaction_Const.GetDistance(mHand.mPalm.transform, item, out ContactIsSelf);
                        if (item.gameObject.GetComponent<Hi5_Object_Property>() != null)
                        {
                            if (!item.gameObject.GetComponent<Hi5_Object_Property>().IsPinch)
                                ContactIsSelf = false;
                            else
                                ContactIsSelf = true;
                        }
                        if (ContactIsSelf)
                        {
                            if (distance < distanceMin)
                            {
                                distanceMin = distance;
                                distanceminId = objectIdTemp;
                            }
                        }
                    }
                }
                ObjectId = distanceminId;
                if (fingertemps != null && fingertemps.Count >= 2)
                {
                    fingers = fingertemps;
                }
                else
                    fingers = null;
                return true;
            }
            else
            {
                fingers = null;
                ObjectId = -1;
                return false;
            }
                
        }
        #endregion

        internal bool IsOK()
        {
            if (mHand.mFingers[Hi5_Glove_Interaction_Finger_Type.EThumb].IsThumbColliderIndex())
            {
                int count = 0;

                if (Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].IsFingerPlane())
                    count++;
                if (Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.ERing].IsFingerPlane())
                    count++;
                if (Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EPinky].IsFingerPlane())
                    count++;
                if (count == 3)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        internal bool IsCloseThumbAndIndexCollider()
        {
            Transform tailThumb =  mHand.mFingers[Hi5_Glove_Interaction_Finger_Type.EThumb].mChildNodes[4];
            Transform tailIndex = mHand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].mChildNodes[4];
            float distance = Vector3.Distance(tailThumb.position, tailIndex.position);
            if (mHand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].IsPinch2Finger())
            {
                if (distance < 0.05f)
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
            
        internal bool IsFingerPlane()
		{
			int count = 0;

			if (Hand.mFingers [Hi5_Glove_Interaction_Finger_Type.EIndex].IsFingerPlane ())
				count++;
            if (Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].IsFingerPlane())
                count++;
            if (Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.ERing].IsFingerPlane())
                count++;
            if (Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EPinky].IsFingerPlane())
                count++;
            if (count >= 4)
				return true;
			else
				return false;
		}

       

        internal bool IsHandIndexPoint()
        {
            if (Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].IsFingerPlane())
            {
                int count = 0;
                if (Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].IsFingerFist())
                {
                    count++;
                    
                }
                if (Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.ERing].IsFingerFist())
                {
                    count++;
                  
                }

                if (Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EPinky].IsFingerFist())
                {
                    count++;
                    
                }
                if (count == 3)
                    return true;
                else
                    return false;
            }
            return false;
        }

		internal bool IsHandFist()
		{
			int count = 0;
            if (Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].IsFingerFist())
            {
                count++;
                // Debug.Log("IsHandIndexPoint EIndex");
            }

            if (Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].IsFingerFist())
            {
                // Debug.Log("IsHandIndexPoint EMiddle");
                count++;
            }
            if (Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.ERing].IsFingerFist())
            {
                count++;
                //Debug.Log("IsHandIndexPoint ERing");
            }

            if (Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EPinky].IsFingerFist())
            {
                count++;
                //Debug.Log("IsHandIndexPoint EPinky");
            }
            if (count == 4)
            //if (count == 1)
                return true;
			else
				return false;
		}

		internal bool IsClap(out int objectId, out Hi5_Glove_Interaction_Finger_Type fingerOneType, out Hi5_Glove_Interaction_Finger_Type fingerTwoType)
        {
            List<int> collisionIndexs;
            bool index = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].IsClap(out collisionIndexs);
            List<int> collisionMiddles;
            bool middle = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].IsClap(out collisionMiddles);
            List<int> collisionRings;
            bool ring = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.ERing].IsClap(out collisionRings);
            List<int> collisionPalms;
//			bool pinky = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EPinky].IsClap(out collisionRings);
//			List<int> collisionPinkys;
            //手腕
            bool palm = Hand.mPalm.IsClap(out collisionPalms);
            
            if (palm && collisionPalms != null && collisionPalms.Count > 0)
            {
                for (int i = 0; i < collisionPalms.Count; i++)
                {
                    
                    int idObject = collisionPalms[i];

                    Hi5_Glove_Interaction_Item item = Hi5_Interaction_Object_Manager.GetObjectManager().GetItemById(idObject);

                    
                    if (item.gameObject.GetComponent<Hi5_Object_Property>() != null)
                    {
                        if (!item.gameObject.GetComponent<Hi5_Object_Property>().IsClap)
                            break;
                    }

                    //判断夹角
                    Transform palmTransform = Hand.mPalm.transform;
                    Transform onPoint = item.transform;
                    Vector3 temp = onPoint.position - palmTransform.position;
                    float tempValue = Vector3.Dot(temp, palmTransform.up);
                    if (tempValue < 0)
                    {
                       
                        objectId = collisionPalms[i];
						fingerOneType = Hi5_Glove_Interaction_Finger_Type.ENone;
						fingerTwoType = Hi5_Glove_Interaction_Finger_Type.ENone;
                        float angleUpWorld = Vector3.Angle(palmTransform.transform.up, Hi5_Interaction_Object_Manager.GetObjectManager().transform.up);
                        if (angleUpWorld < 45.0f && angleUpWorld > 0.0f)
                        {
                           
                            return true;
                        }
                            
                        else
                        {
                            objectId = -1;
							fingerOneType = Hi5_Glove_Interaction_Finger_Type.ENone;
							fingerTwoType = Hi5_Glove_Interaction_Finger_Type.ENone;
                            return false;
                        }
                    }
                }
            }

			int middleObjectId = -1;
			Hi5_Glove_Interaction_Finger_Type fingerMiddleType = Hi5_Glove_Interaction_Finger_Type.ENone;
			{
				//中指
				if (middle && collisionMiddles != null && collisionMiddles.Count > 0)
				{
					for (int i = 0; i < collisionMiddles.Count; i++)
					{
						int idObject = collisionMiddles[i];
						Hi5_Glove_Interaction_Item item = Hi5_Interaction_Object_Manager.GetObjectManager().GetItemById(idObject);
						//判断夹角
						Transform palmTransform = Hand.mPalm.transform;
						Transform onPoint = item.transform;
						Vector3 temp = onPoint.position - palmTransform.position;
						float tempValue = Vector3.Dot(temp, palmTransform.up);
						if (tempValue < 0)
						{

							float angleUpWorld = Vector3.Angle(palmTransform.transform.up, Hi5_Interaction_Object_Manager.GetObjectManager().transform.up);
							//Debug.Log("angelUpWorld=" + angelUpWorld);
							if (angleUpWorld < 45.0f && angleUpWorld > 0.0f)
							{
								middleObjectId = collisionMiddles[i];
								fingerMiddleType = Hi5_Glove_Interaction_Finger_Type.EMiddle;
								//return true;
							}

							else
							{
								middleObjectId = -1;
								fingerMiddleType = Hi5_Glove_Interaction_Finger_Type.ENone;
								//return false;
							}

						}
					}
				}

			}


			int indexObjectId = -1;
			Hi5_Glove_Interaction_Finger_Type fingerIndexType = Hi5_Glove_Interaction_Finger_Type.ENone;
			{
				//index
				if (index && collisionIndexs != null && collisionIndexs.Count > 0)
				{
					for (int i = 0; i < collisionIndexs.Count; i++)
					{
						int idObject = collisionIndexs[i];
						Hi5_Glove_Interaction_Item item = Hi5_Interaction_Object_Manager.GetObjectManager().GetItemById(idObject);
						//判断夹角
						Transform palmTransform = Hand.mPalm.transform;
						Transform onPoint = item.transform;
						Vector3 temp = onPoint.position - palmTransform.position;
						float tempValue = Vector3.Dot(temp, palmTransform.up);
						if (tempValue < 0)
						{
							float angleUpWorld = Vector3.Angle(palmTransform.transform.up, Hi5_Interaction_Object_Manager.GetObjectManager().transform.up);
							//Debug.Log("angelUpWorld=" + angelUpWorld);
							if (angleUpWorld < 45.0f && angleUpWorld > 0.0f)
							{
								indexObjectId = collisionIndexs[i];
								fingerIndexType = Hi5_Glove_Interaction_Finger_Type.EIndex;
								//return true;
							}
							else
							{
								indexObjectId = -1;
								fingerIndexType = Hi5_Glove_Interaction_Finger_Type.ENone;
								//return false;
							}
						}

					}
				}
			}
          
			int ringObjectId = -1;
			Hi5_Glove_Interaction_Finger_Type fingerRingType = Hi5_Glove_Interaction_Finger_Type.ENone;
			{
				//ring
				if (ring && collisionRings != null && collisionRings.Count > 0)
				{
					for (int i = 0; i < collisionRings.Count; i++)
					{
						int idObject = collisionRings[i];
						Hi5_Glove_Interaction_Item item = Hi5_Interaction_Object_Manager.GetObjectManager().GetItemById(idObject);
						//判断夹角
						Transform palmTransform = Hand.mPalm.transform;
						Transform onPoint = item.transform;
						Vector3 temp = onPoint.position - palmTransform.position;
						float tempValue = Vector3.Dot(temp, palmTransform.up);
						if (tempValue < 0)
						{
							float angleUpWorld = Vector3.Angle(palmTransform.transform.up, Hi5_Interaction_Object_Manager.GetObjectManager().transform.up);
							if (angleUpWorld < 45.0f && angleUpWorld > 0.0f)
							{
								ringObjectId = collisionRings[i];
								fingerRingType = Hi5_Glove_Interaction_Finger_Type.EMiddle;
								//return true;
							}

							else
							{
								ringObjectId = -1;
								fingerRingType = Hi5_Glove_Interaction_Finger_Type.ENone;
								//return false;
							}
						}
					}
				}
			}
            
			if (middleObjectId != -1 && indexObjectId != -1 && middleObjectId == indexObjectId) {
				objectId = middleObjectId;
				fingerOneType = Hi5_Glove_Interaction_Finger_Type.EMiddle;
				fingerTwoType = Hi5_Glove_Interaction_Finger_Type.EIndex;
				return true;
			}
			if (middleObjectId != -1 && ringObjectId != -1 && middleObjectId == ringObjectId) {
				objectId = middleObjectId;
				fingerOneType = Hi5_Glove_Interaction_Finger_Type.EMiddle;
				fingerTwoType = Hi5_Glove_Interaction_Finger_Type.ERing;
				return true;
			}
			if (indexObjectId != -1 && ringObjectId != -1 && indexObjectId == ringObjectId) {
				objectId = ringObjectId;
				fingerOneType = Hi5_Glove_Interaction_Finger_Type.EIndex;
				fingerTwoType = Hi5_Glove_Interaction_Finger_Type.ERing;
				return true;
			}

			else
			{
				objectId = -1;
				fingerOneType = Hi5_Glove_Interaction_Finger_Type.ENone;
				fingerTwoType = Hi5_Glove_Interaction_Finger_Type.ENone;
				return false;
			}
           
        }

        internal bool IsLift(out int objectId)
        {
            //if (Hi5_Interaction_Const.TestFlyMoveNoUsedGravity)
            {
				List<int> collisionIndexs;
				bool index = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EIndex].IsLift(out collisionIndexs);
				List<int> collisionMiddles;
				bool middle = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.EMiddle].IsLift(out collisionMiddles);
				List<int> collisionRings;
				bool ring = Hand.mFingers[Hi5_Glove_Interaction_Finger_Type.ERing].IsLift(out collisionRings);



				List<int> collisionResults1 = null;
				if (index && middle &&collisionIndexs != null && collisionMiddles != null && collisionIndexs.Count>0 && collisionMiddles.Count>0) 
				{
					collisionResults1 = collisionIndexs.Intersect(collisionMiddles).ToList();
				}

				List<int> collisionResults2 = null;
				if (ring && middle &&collisionRings != null && collisionMiddles != null && collisionRings.Count>0 && collisionMiddles.Count>0) 
				{
					collisionResults2 = collisionRings.Intersect(collisionMiddles).ToList();
				}

                List<int> collisionPalms;
                int tempId = -1;
//
                bool palm = Hand.mPalm.IsClap(out collisionPalms);
				//bool palm = Hand.mPalm.IsLift(out collisionPalms);
				List<int> collisionResult = null;
//
				if (palm && collisionPalms != null && collisionPalms.Count > 0)
				{
					collisionResult = collisionPalms;
							
				}


				if (collisionResult == null  && collisionResults1 != null && collisionResults1.Count>0) {
					collisionResult = collisionResults1;
				}
				else
				{
					if (collisionResult != null && collisionResult.Count>0 && collisionResults1 != null && collisionResults1.Count>0)
					{
						List<int> collisionResultValue = collisionResult.Intersect (collisionResults1).ToList ();
						collisionResult = collisionResultValue;
					}
					else
					{
						if(collisionResults1 != null && collisionResults1.Count>0)
							collisionResult = collisionResults1;
					}
				}

				if (collisionResult == null  && collisionResults2 != null&& collisionResults2.Count>0) 
				{
					collisionResult = collisionResults2;
				}
				else
				{
					if (collisionResult != null && collisionResult.Count > 0 && collisionResults2 != null && collisionResults2.Count > 0) {
						List<int> collisionResultValue = collisionResult.Intersect (collisionResults2).ToList ();
						collisionResult = collisionResultValue;
					}
					else
					{
						if(collisionResults2 != null && collisionResults2.Count>0)
							collisionResult = collisionResults2;
					}
				}

				if(collisionResult!= null && collisionResult.Count>0)
                {
					for (int i = 0; i < collisionResult.Count; i++)
                    {
						int idObject = collisionResult[i];
                        Hi5_Glove_Interaction_Item item = Hi5_Interaction_Object_Manager.GetObjectManager().GetItemById(idObject);
                        if (item.gameObject.GetComponent<Hi5_Object_Property>() != null)
                        {
                            if (!item.gameObject.GetComponent<Hi5_Object_Property>().IsLift)
                                break;
                        }
                       
                        if ((item.mstatemanager.State == E_Object_State.EMove && (item.mstatemanager.GetMoveState().mMoveType == Hi5ObjectMoveType.EThrowMove ||  item.mstatemanager.GetMoveState().mMoveType == Hi5ObjectMoveType.EFree))
							||item.mstatemanager.State == E_Object_State.EStatic)
                        {
                            float angle = Vector3.Angle(Hi5_Interaction_Object_Manager.GetObjectManager().transform.up, -Hand.mPalm.transform.up);
                            if (angle < Hi5_Interaction_Const.FlyLiftPalmAngle && angle > 0.0f)
                            {
								
								if (Hand.mPalm.mAnchor == null) {
									Hand.mPalm.mAnchor = Hand.mPalm.transform.GetChild (0);
								}
								Vector3 pamlUp = -Hand.mPalm.mAnchor.up;
								Vector3 v1 = item.transform.position - Hand.mPalm.mAnchor.position;
									//float dotValue = Vector3.Dot(v1, pamlUp);
								float angleValue = Vector3.Angle(v1, pamlUp);
								//Debug.Log ("angleVale =" +angleVale);
								if (angleValue < 0.0f)
                                    angleValue = 180.0f - angleValue;
								//Debug.Log ("angleVale =" +angleVale);
								if (angleValue > 85.0f)
								{
										

								}
								else
								{
									tempId = idObject;

								}
                                break;
                            }
                        }
                    }
                    if (tempId == -1)
                    {
                        objectId = -1;
                        return false;
                    }
                    else
                    {
                        objectId = tempId;
                        return true;
                    }
                }
                else
                {
                    objectId = -1;
                    return false;
                }
                
            }
            //else
            //{
            //    objectId = -1;
            //    return false;
            //}
        }
    }
}

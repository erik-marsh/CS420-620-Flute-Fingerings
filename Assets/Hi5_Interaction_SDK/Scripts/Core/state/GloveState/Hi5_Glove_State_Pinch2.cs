using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public class Hi5_Glove_State_Pinch2 : Hi5_Glove_State_Base
    {
        internal int objectId = -1000;
        
        Dictionary<Hi5_Glove_Interaction_Finger_Type, Hi5_Glove_Interaction_Finger> fingerColliders = new Dictionary<Hi5_Glove_Interaction_Finger_Type, Hi5_Glove_Interaction_Finger>();
        Dictionary<Hi5_Glove_Interaction_Finger_Type, float> fingerColliderVecotr3 = new Dictionary<Hi5_Glove_Interaction_Finger_Type, float>();
        override public void Start()
        {
            
        }

        override public void FixedUpdate(float deltaTime)
        {

        }
        // Update is called once per frame
        override public void Update(float deltaTime)
        {
            if (fingerColliders.ContainsKey(Hi5_Glove_Interaction_Finger_Type.EThumb))
            {
                Hi5_Glove_Interaction_Finger fingerThumb = fingerColliders[Hi5_Glove_Interaction_Finger_Type.EThumb];
                Hi5_Glove_Interaction_Finger otherFinger = null;
                if (fingerColliders.ContainsKey(Hi5_Glove_Interaction_Finger_Type.EIndex))
                    otherFinger = fingerColliders[Hi5_Glove_Interaction_Finger_Type.EIndex];
                else if (fingerColliders.ContainsKey(Hi5_Glove_Interaction_Finger_Type.EMiddle))
                    otherFinger = fingerColliders[Hi5_Glove_Interaction_Finger_Type.EMiddle];
                if (fingerThumb != null && otherFinger != null)
                {
//                    Vector3 point = new Vector3((fingerThumb.GetTailPosition().x + otherFinger.GetTailPosition().x) / 2,
//                                                (fingerThumb.GetTailPosition().y + otherFinger.GetTailPosition().y) / 2,
//                                                (fingerThumb.GetTailPosition().z + otherFinger.GetTailPosition().z) / 2);
//                    Hi5_Glove_Interaction_Item item = Hi5_Interaction_Object_Manager.GetObjectManager().GetItemById(objectId);
//                    if (item != null)
//                        item.transform.position = point;

                    //Debug.Log("Pich2 position"+ point);
                }
            }
            if (fingerColliders.Count > 1)
            {
               // Debug.Log("fingerColliders.Count > 1" );
                bool isRelease = false;
                int countValue = 0;
                foreach (KeyValuePair<Hi5_Glove_Interaction_Finger_Type, Hi5_Glove_Interaction_Finger> item in fingerColliders)
                {
                    ////碰撞释放
                    //if (!item.Value.IsTailColliderById(objectId))
                    //{
                    //    isRelease = true;

                    //}
                    //角度释放
                    if (fingerColliders[item.Key].IsPinch2Realease(Hi5_Interaction_Const.FingerPinch2Release))
                    {
                        isRelease = true;
                        return;
                     }
                    
                   
                }
                if (fingerColliders.ContainsKey(Hi5_Glove_Interaction_Finger_Type.EThumb))
                {
                    Hi5_Glove_Interaction_Finger fingerThumb = fingerColliders[Hi5_Glove_Interaction_Finger_Type.EThumb];
                    Hi5_Glove_Interaction_Finger otherFinger = null;
                    if (fingerColliders.ContainsKey(Hi5_Glove_Interaction_Finger_Type.EIndex))
                        otherFinger = fingerColliders[Hi5_Glove_Interaction_Finger_Type.EIndex];
                    else if (fingerColliders.ContainsKey(Hi5_Glove_Interaction_Finger_Type.EMiddle))
                        otherFinger = fingerColliders[Hi5_Glove_Interaction_Finger_Type.EMiddle];
                    if (fingerThumb != null && otherFinger != null)
                    {
                        Vector3 tempThumb = fingerThumb.GetTailPosition();
                        Vector3 tempOther = otherFinger.GetTailPosition();
                        float distance = Vector3.Distance(tempThumb, tempOther);
                        if (distance > 0.080f)
                        {
                            isRelease = true;
                           
                        }
                    }
                }
                if (isRelease || countValue >= 2)
                {
					mState.ChangeState(E_Hand_State.ERelease);
                    Hi5_Interaction_Message.GetInstance().DispenseMessage(Hi5_MessageKey.messageUnPinchObject2, (object)Hand.mPinchObjectId, Hand);   
                }
                else
                {
                   
                }
            }
            else
            {
				mState.ChangeState(E_Hand_State.ERelease);
                Hi5_Interaction_Message.GetInstance().DispenseMessage(Hi5_MessageKey.messageUnPinchObject2, (object)Hand.mPinchObjectId, Hand);                
            }
        }

        override public void End()
        {
            fingerColliders.Clear();
            fingerColliderVecotr3.Clear();
        }

        internal void AddFingerCollider(Hi5_Glove_Interaction_Finger finger)
        {
            //if (fingerColliders.Count >= 2)
            //    return;
            if (!fingerColliders.ContainsKey(finger.m_finger_type))
                fingerColliders.Add(finger.m_finger_type, finger);
            if (!fingerColliderVecotr3.ContainsKey(finger.m_finger_type))
            {
                fingerColliderVecotr3.Add(finger.m_finger_type, finger.GetPinch2OneToFour());
            }
            else
            {
                fingerColliderVecotr3[finger.m_finger_type] = finger.GetPinch2OneToFour();
            }
        }


        //private bool IsFingerRelease()
        //{
            
        //}
    }
}

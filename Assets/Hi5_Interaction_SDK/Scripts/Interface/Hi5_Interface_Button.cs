using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hi5_Interaction_Core;
namespace Hi5_Interaction_Interface
{
	public class Hi5_Interface_Button : Hi5_Interface_Object_Base 
    {
		bool isRegister = false;
        protected void OnEnable()
        {
			if (Hi5InteractionManager.Instance != null)
            {
				Hi5InteractionManager.Instance.GetMessage().RegisterMessage(MessageFun, Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent);
               
				isRegister = true;
            }
        }

        protected void Update()
        {
			if (Hi5InteractionManager.Instance != null && !isRegister)
            {
				Hi5InteractionManager.Instance.GetMessage().RegisterMessage(MessageFun, Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent);
       
				isRegister = true;
            }
			if (mItem != null && GetObjectItemState == E_Object_State.EStatic)
			{
				
                if(mItem.GetComponent<Hi5_Interaction_Item_Collider>() != null)
				    mItem.ChangeColor(mItem.GetComponent<Hi5_Interaction_Item_Collider>().orgColor);
			}
        }


        protected void OnDisable()
        {
			if(Hi5InteractionManager.Instance != null && Hi5InteractionManager.Instance.GetMessage() != null)
				Hi5InteractionManager.Instance.GetMessage().UnRegisterMessage(MessageFun, Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent);
			isRegister = false;
        }

		/**
     	* Get button state evnet event.
    	**/
		virtual public void MessageFun(string messageKey, object param1, object param2)
        {
            if (messageKey.CompareTo(Hi5_Glove_Interaction_Message.Hi5_MessageMessageKey.messageObjectEvent) == 0)
            {
                Hi5_Glove_Interaction_Object_Event_Data data = param1 as Hi5_Glove_Interaction_Object_Event_Data;
                if (data.mObjectId == ObjectItem.idObject)
                {
                    if (data.mEventType == EEventObjectType.EClap)
                    {
                        
                    }
					else if (data.mEventType == EEventObjectType.EPoke)
                    {
                        
                    }
                    else if (data.mEventType == EEventObjectType.EStatic)
                    {

                    }
                }
            }
        }
    }

}

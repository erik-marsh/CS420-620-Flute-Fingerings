using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{

   
    public class Hi5_Glove_Interaction_Message
    {

        public class Hi5_MessageMessageKey
        {
            public static readonly string messageObjectEvent = "messageObjectEvent";
            public static readonly string messageHandEvent = "messageHandEvent";
            
        }

        public delegate void MessageFun(string messageKey, object param1, object param2);
        public Dictionary<string, MessageFun> dicMessage = new Dictionary<string, MessageFun>();

        public void DispenseMessage(string messageKey, object param1, object param2)
        {
            if (dicMessage.ContainsKey(messageKey))
            {
                dicMessage[messageKey](messageKey, param1, param2);
            }
        }

        public void RegisterMessage(MessageFun fun, string messageKey)
        {
            if (!dicMessage.ContainsKey(messageKey))
            {
                dicMessage.Add(messageKey, null);
                dicMessage[messageKey] += fun;
            }
            else
            {
                dicMessage[messageKey] += fun;
            }
        }

        public void UnRegisterMessage(MessageFun fun, string messageKey)
        {
            if (dicMessage.ContainsKey(messageKey))
            {
                dicMessage[messageKey] -= fun;
            }
        }
    }
}


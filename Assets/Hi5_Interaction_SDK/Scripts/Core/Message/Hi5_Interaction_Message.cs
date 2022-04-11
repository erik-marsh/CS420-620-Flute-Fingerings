using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public class Hi5_MessageKey
    {
        public static readonly string messagePinchObject = "messagePinchObject";
		public static readonly string messageUnPinchObject = "messageUnPinchObject";
        public static readonly string messageGetObjecById = "messageGetObjecById";
        public static readonly string messageObjectReset = "messageObjectReset";
        public static readonly string messagePinchObject2 = "messagePinchObject2";
        public static readonly string messageUnPinchObject2 = "messageUnPinchObject2";
        public static readonly string messageFlyPinchObject = "messageFlyPinchObject";
        public static readonly string messageLiftObject = "messageLiftObject";
		public static readonly string messagePinchOtherHandRelease = "messagePinchOtherHandRelease";
    }
    public class Hi5_Interaction_Message
    {
        private static Hi5_Interaction_Message instance;
        private static object _lock = new object();

        private Hi5_Interaction_Message()
        {

        }

        public static Hi5_Interaction_Message GetInstance()
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new Hi5_Interaction_Message();
                    }
                }
            }
            return instance;
        }



        public delegate void MessageFun(string messageKey, object param1, object param2, object param3, object param4);
        public Dictionary<string, MessageFun> dicMessage = new Dictionary<string, MessageFun>();

        public void DispenseMessage(string messageKey, object param1, object param2 = null, object param3 = null, object param4 = null)
        {
            if (dicMessage.ContainsKey(messageKey))
            {
                dicMessage[messageKey](messageKey, param1, param2, param3, param4);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    abstract public class Hi5_Object_State_Base
    {
        internal protected Hi5_Glove_Interaction_Item ObjectItem
        {
            set { mObjectItem = value; }
            get { return mObjectItem; }
        }

        virtual internal protected void Init(Hi5_Glove_Interaction_Item itemObject, Hi5_Object_State_Manager state)
        {
            mObjectItem = itemObject;
            mState = state;
        }

        protected Hi5_Glove_Interaction_Item mObjectItem = null;
        internal protected Hi5_Object_State_Manager mState = null;

        public abstract void Start();
        public abstract void Update(float deltaTime);
        public abstract void End();
        public abstract void FixedUpdate(float deltaTime);
    }
}

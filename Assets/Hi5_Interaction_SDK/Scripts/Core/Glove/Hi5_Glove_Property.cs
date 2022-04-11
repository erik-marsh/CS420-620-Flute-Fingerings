using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    [System.Serializable]
    public class Hi5_Glove_Property_Data
    {
        public bool IsPinch = true;
        public bool IsClap = true;
        public bool IsLift = true;
       
    }

    public class Hi5_Glove_Property : MonoBehaviour
    {
        public Hi5_Glove_Property_Data Property_Data;

        public bool IsPinch
        {
            set { Property_Data.IsPinch = value; }
            get { return Property_Data.IsPinch; }
        }

        public bool IsClap
        {
            set { Property_Data.IsClap = value; }
            get { return Property_Data.IsClap; }
        }

        public bool IsLift
        {
            set { Property_Data.IsLift = value; }
            get { return Property_Data.IsLift; }
        }
    }
}

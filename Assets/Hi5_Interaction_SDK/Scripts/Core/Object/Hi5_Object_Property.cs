using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    [System.Serializable]
    public class Hi5_Object_Property_Data
    {
        private float AirFrictionRate;
        public Hi5_Object_Rigidbody AirMoveProperty;
        public Hi5_Object_Rigidbody StaticProperty;
        public Hi5_Object_Rigidbody PlaneMoveProperty;
        public bool IsPinch = true;
        public bool IsClap = true;
        public bool IsLift = true;
    }

    [System.Serializable]
    public class Hi5_Object_Rigidbody
    {
        public bool ConstraintsFreezeRotation = false;
        //public bool ConstraintsFreezeRotationX = false;
        //public bool ConstraintsFreezeRotationY = false;
        //public bool ConstraintsFreezeRotationZ = false;
    }

    public class Hi5_Object_Property : MonoBehaviour
    {
        public Hi5_Object_Property_Data ObjectProperty;
        public bool IsPinch
        {
            set { ObjectProperty.IsPinch = value; }
            get { return ObjectProperty.IsPinch; }
        }
        public bool IsClap
        {
            set { ObjectProperty.IsClap = value; }
            get { return ObjectProperty.IsClap; }
        }
        public bool IsLift
        {
            set { ObjectProperty.IsLift = value; }
            get { return ObjectProperty.IsLift; }
        }
        public void SetRotation(bool X, bool Y, bool Z)
        {
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            if (gameObject.GetComponent<Rigidbody>() != null)
            {
                if (X && Y && Z)
                {
                    gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                }
                else if (!X && Y && Z)
                {
                    gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                }
                else if (X && !Y && Z)
                {
                    gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                }
                else if (X && Y && !Z)
                {
                    gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
                }
                else if (X && !Y && !Z)
                {
                    gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;
                }
                else if (!X && Y && !Z)
                {
                    gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
                }
                else if (!X && !Y && Z)
                {
                    gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ;
                }
            }
            
        }
    }
}

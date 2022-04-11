using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public class Hi5_Hand_Nail_Controller : MonoBehaviour
    {
        Hi5_Hand_Nail_Collider[] colliders = null;
        void Awake()
        {
            colliders = gameObject.GetComponentsInChildren<Hi5_Hand_Nail_Collider>();
        }

        internal bool IsNail(int ObjectId)
        {
            if (colliders == null)
                return false;
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].IsNail(ObjectId))
                    return true;

            }
            return false;
        }

    }
}

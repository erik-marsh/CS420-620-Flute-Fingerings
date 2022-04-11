using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public class Hi5_Button_Item : Hi5_Glove_Interaction_Item_Trigger
    {
        override protected void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
        }

        override protected void OnTriggerStay(Collider other)
        {
            base.OnTriggerStay(other);
        }

        override protected void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
        }
    }
}

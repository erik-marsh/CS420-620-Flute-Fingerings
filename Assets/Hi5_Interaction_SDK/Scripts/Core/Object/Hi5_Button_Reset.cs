using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hi5_Interaction_Core
{
    public class Hi5_Button_Reset : Hi5_Button_Item
    {
        bool isTrigger = false;
        float cd = 0.8f;
        override protected void OnTriggerEnter(Collider other)
        {
            if (!isTrigger)
            {
                base.OnTriggerEnter(other);
                Hi5_Interaction_Message.GetInstance().DispenseMessage(Hi5_MessageKey.messageObjectReset, null, null);
                isTrigger = true;
            }
        }

        private void Update()
        {
            if (isTrigger)
            {
                cd -= Time.deltaTime;
                if (cd <= 0.0f)
                {
                    cd = 0.5f;
                    isTrigger = false;
                }
            }
        }
    }
}

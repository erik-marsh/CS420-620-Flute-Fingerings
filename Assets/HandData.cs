using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Hi5_Interaction_Core;

public class HandData : MonoBehaviour
{
    public Text[] texts;
    public Text fingeringName;
    //public Hi5_Hand_Collider_Visible_Thumb_Finger[] thumbs;
    public Hi5_Hand_Visible_Finger[] fingersVisible;
    public Hi5_Glove_Interaction_Finger[] fingers;

    public float updatePeriod = 0.5f;
    private float timer = 0.0f;

    private void Awake()
    {
        fingeringName.text = "";
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer <= updatePeriod) return;

        timer = 0.0f;
        // WARNING: this is heavily volatile - the finger array is assumed to be set up identically to Fingering.knownFingerings
        uint keyCombination = 0;
        for (int i = 0; i < texts.Length; i++)
        {
            var text = texts[i];
            var finger = fingers[i];
            float angle = finger.GetAngle(finger.mChildNodes[1], finger.mChildNodes[3], finger.mChildNodes[4]);
            text.text = text.gameObject.name + ": " + angle.ToString("0.00");
            if (angle < 160.0f)
            {
                text.color = Color.green;
                keyCombination |= (uint)1 << i;
            }
            else
            {
                text.color = Color.black;
            }
        }

        var fingering = Fingering.GetFingeringByCombination(keyCombination);
        fingeringName.text = fingering.name;
    }
}

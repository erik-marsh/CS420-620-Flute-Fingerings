using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Hi5_Interaction_Core;

public class HandData : MonoBehaviour
{
    // every list should have the fingers in the correct order:
    // LIndex, LMiddle, LRing, LPinky, LThumb, RIndex, RMiddle, RRing, RPinky, RThumb

    public PianoRoll pianoRoll;
    public Text[] texts;
    public Text fingeringName;
    //public Hi5_Hand_Collider_Visible_Thumb_Finger[] thumbs;
    public Image[] UIFingerImages;
    public Hi5_Hand_Visible_Finger[] fingersVisible;
    public Hi5_Glove_Interaction_Finger[] fingers;
    public Transform[] fingerReferenceNodes;
    private float[] fingerThresholds = new float[10];

    public float updatePeriod = 0.5f;
    private float timer = 0.0f;

    private int lastFingerSet = 0;

    public static event System.EventHandler<Fingering> OnNoteStart;
    public static event System.EventHandler<Fingering> OnNoteEnd;

    private Fingering lastFingering = Fingering.nullFingering;

    private void Awake()
    {
        fingeringName.text = "";
        fingerThresholds = new float[10]
        {
            170.0f,
            158.0f,
            145.0f,
            120.0f,
            157.0f,

            160.0f,
            160.0f,
            160.0f,
            150.0f,
            0.0f
        };
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
            // these are indexed starting at 1 for some reason
            //float angle = finger.GetAngle(finger.mChildNodes[1], finger.mChildNodes[3], finger.mChildNodes[4]);
            //text.text = angle.ToString("0.00");

            float angle = 0.0f;
            foreach (var node in finger.mChildNodes)
            {
                float normalizedAngle = node.Value.localEulerAngles.z;

                // the right hand angles are the negative of the left hand angles
                if (i >= 5) normalizedAngle = (360.0f - normalizedAngle);
                if (i == 5)
                    Debug.Log(node.Key + " " + normalizedAngle);

                // the range of angles for finger joints in the editor is [-20, 89]
                // unity does not use negative euler angles internally, so this range is actually the union of [340, 360] and [0, 89]
                // hence, angles above 90 degrees get normalized to 0
                if (normalizedAngle > 90.0f) normalizedAngle = 0.0f;

                // just in case
                if (normalizedAngle < 0.0f) normalizedAngle = 0.0f;

                if (i == 5)
                    Debug.Log(node.Key + " " + normalizedAngle);

                angle += normalizedAngle;
            }

            text.text = angle.ToString("0.00");

            // TODO: tbh the best way to go about this is having one threshold per finger, since the GetAngle gives different results depending on the finger
            if (angle < fingerThresholds[i])
            {
                text.color = Color.green;
                UIFingerImages[i].color = Color.green;
                keyCombination |= (uint)1 << i;
            }
            else
            {
                text.color = Color.black;
                UIFingerImages[i].color = Color.white;
            }
        }

        var fingering = Fingering.GetFingeringByCombination(keyCombination);
        fingeringName.text = "Detected: ";
        pianoRoll.SetKey(fingering.midiNote);

        if (fingering != Fingering.nullFingering)
        {
            fingeringName.text += fingering.name + " (" + fingering.frequency.ToString("0.00") + "Hz)";
        }

        // the fingerings are all retrieved from a static array, so object equality should work for this
        if (fingering != lastFingering)
        {
            // any time the fingering changes, end a note
            // even if the new fingering is the null fingering
            // null fingerings would change the sound being produced
            // the timing will overlap a little here, unfortunately
            if (lastFingering != Fingering.nullFingering) // corrects a small error on startup
            {
                OnNoteEnd?.Invoke(this, lastFingering);
            }

            // if we have moved to a new (non-null) fingering
            // null fingerings do not make sounds that we care about for this project, so we ignore them
            if (fingering != Fingering.nullFingering)
            {
                OnNoteStart?.Invoke(this, fingering);
            }
        }

        lastFingering = fingering;
    }
}

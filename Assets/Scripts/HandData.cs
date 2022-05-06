using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Hi5_Interaction_Core;
using NotePlayer;

/// <summary>
/// Class that manages and interprets input from the gloves.
/// </summary>
public class HandData : MonoBehaviour
{
    #region UI References
    // every array should have the fingers in the correct order:
    // LIndex, LMiddle, LRing, LPinky, LThumb, RIndex, RMiddle, RRing, RPinky, RThumb
    public PianoRoll pianoRoll;
    public Text fingeringName;
    public Text detected;
    public Text[] texts;
    public Image[] UIFingerImages;
    public Image[] FingeringDiagramImages;
    #endregion

    #region Script Parameters
    [Tooltip("How often the angles should update (in seconds)")]
    public float updatePeriod = 0.5f;

    public Color activatedColor = Color.green;
    public Color activatedColorRecording = Color.blue;
    public Color textInactiveColor = Color.black;
    public Color imageInactiveColor = Color.white;
    #endregion

    #region Members
    private float timer = 0.0f;

    private Fingering lastFingering = Fingering.nullFingering;

    public Hi5_Glove_Interaction_Finger[] fingers;

    public bool isPlayingRecording = false;
    #endregion

    #region Static Data
    /// <summary>
    /// Thresholds for finger activation and deactivation.
    /// RThumb is unused, since that thumb does not press any keys on the flute.
    /// </summary>
    private static float[] fingerThresholds = new float[10]
    {
        30.0f, // LIndex
        80.0f, // LMiddle
        90.0f, // LRing
        80.0f, // LPinky
        157.0f, // LThumb

        45.0f, // RIndex
        70.0f, // RMiddle
        80.0f, // RRing
        120.0f, // RPinky
        0.0f    // RThumb
    };
    #endregion

    #region Events
    /// <summary>
    /// Invoked when a new valid fingering is detected.
    /// Passes the new note to the event handlers.
    /// </summary>
    public static event System.EventHandler<Fingering> OnNoteStart;

    /// <summary>
    /// Invoked when a new fingering is detected (valid or invalid).
    /// Passes the ended note to the event handlers.
    /// </summary>
    public static event System.EventHandler<Fingering> OnNoteEnd;
    #endregion

    #region Initialization
    private void Awake()
    {
        fingeringName.text = "";
    }
    #endregion

    #region Event Handlers
    private void OnEnable()
    {
        PN_NotePlaybackManager.OnNotePlayed += OnNotePlayed;
    }

    private void OnDisable()
    {
        PN_NotePlaybackManager.OnNotePlayed -= OnNotePlayed;
    }

    /// <summary>
    /// When a recording plays a note, mark it as beign played on the UI.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="midiNote"></param>
    private void OnNotePlayed(object sender, int midiNote)
    {
        if (!isPlayingRecording)
        {
            // clear visualizations
            pianoRoll.SetKey(-1, activatedColorRecording);
            for (int i = 0; i < 9; i++)
            {
                FingeringDiagramImages[i].color = imageInactiveColor;
                UIFingerImages[i].color = imageInactiveColor;
                texts[i].color = textInactiveColor;
            }

            isPlayingRecording = true;
        }

        SetRecordingNote(midiNote);
    }
    #endregion

    #region Main Logic
    /// <summary>
    /// The main logic loop. Executes every updatePeriod.
    /// Measures the angle of each finger and determines if that finger is pressing a key.
    /// </summary>
    private void Update()
    {
        // when playing a recording, we disable the green hand/fingering/piano visualizations in favor of blue recording visualizations
        if (isPlayingRecording) return;

        timer += Time.deltaTime;
        if (timer <= updatePeriod) return;

        timer = 0.0f;

        uint keyCombination = 0;
        // WARNING: this is heavily volatile - the finger array is assumed to be set up identically to Fingering.knownFingerings
        // subtracting 1 because RThumb (i == 9) is not used for fingerings
        for (int i = 0; i < texts.Length - 1; i++)
        {
            float angle;
            bool isFingerActive;

            // if the finger is a thumb
            // different algorithms and predicates are needed for thumbs since the gloves track thumbs with 3-DOF angles
            // but other fingers only have one angular DOF
            if (i == 4 || i == 9)
            {
                angle = GetThumbAngle(i);
                isFingerActive = angle < fingerThresholds[i];
            }
            else
            {
                angle = GetFingerAngle(i);
                isFingerActive = angle > fingerThresholds[i];
            }

            texts[i].text = angle.ToString("0.00");

            if (isFingerActive)
            {
                texts[i].color = activatedColor;
                UIFingerImages[i].color = activatedColor;
                FingeringDiagramImages[i].color = activatedColor;

                keyCombination |= (uint)1 << i;
            }
            else
            {
                texts[i].color = textInactiveColor;
                UIFingerImages[i].color = imageInactiveColor;
                FingeringDiagramImages[i].color = imageInactiveColor;
            }
        }

        var fingering = Fingering.GetFingeringByCombination(keyCombination);
        detected.text = "Detected: ";
        fingeringName.text = "No note detected.";
        pianoRoll.SetKey(fingering.midiNote, activatedColor);

        if (fingering != Fingering.nullFingering)
        {
            fingeringName.text = fingering.name + " (" + fingering.frequency.ToString("0.00") + "Hz)";
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
    #endregion

    #region Helpers
    /// <summary>
    /// Sums the z angles of each finger joint and returns that sum.
    /// </summary>
    /// <param name="fingerIndex">The finger to measure.</param>
    /// <returns></returns>
    private float GetFingerAngle(int fingerIndex)
    {
        var finger = fingers[fingerIndex];
        float angle = 0.0f;

        foreach (var node in finger.mChildNodes)
        {
            float normalizedAngle = node.Value.localEulerAngles.z;

            // the right hand angles are the negative of the left hand angles
            if (fingerIndex >= 5) normalizedAngle = (360.0f - normalizedAngle);

            // the range of angles for finger joints in the editor is [-20, 89]
            // unity does not use negative euler angles internally, so this range is actually the union of [340, 360] and [0, 89]
            // hence, angles above 90 degrees get normalized to 0
            if (normalizedAngle > 90.0f) normalizedAngle = 0.0f;

            // just in case
            if (normalizedAngle < 0.0f) normalizedAngle = 0.0f;

            angle += normalizedAngle;
        }

        return angle;
    }

    /// <summary>
    /// Uses the default Hi5 angle measurement and returns that value.
    /// </summary>
    /// <param name="fingerIndex">The finger to measure.</param>
    /// <returns></returns>
    private float GetThumbAngle(int fingerIndex)
    {
        var finger = fingers[fingerIndex];
        // these are indexed starting at 1 for some reason
        return finger.GetAngle(finger.mChildNodes[1], finger.mChildNodes[3], finger.mChildNodes[4]);
    }

    /// <summary>
    /// Sets the UI state (piano roll, hands, fingering diagram) to reflect the currently playing MIDI note.
    /// This is different from the UI updates in the main Update loop, as this sets the elements to a different color.
    /// </summary>
    /// <param name="midiNote"></param>
    public void SetRecordingNote(int midiNote)
    {
        if (midiNote < 0)
        {
            pianoRoll.SetKey(midiNote, activatedColorRecording);
            for (int i = 0; i < 9; i++)
            {
                FingeringDiagramImages[i].color = imageInactiveColor;
                UIFingerImages[i].color = imageInactiveColor;
            }
        }

        pianoRoll.SetKey(midiNote, activatedColorRecording);
        Fingering currentFingering = Fingering.GetFingeringByMIDINote(midiNote);
        detected.text = "Playing: ";
        fingeringName.text = currentFingering.name + " (" + currentFingering.frequency.ToString("0.00") + "Hz)";

        for (int i = 0; i < 9; i++)
        {
            if ((currentFingering.keyCombination & (1 << i)) > 0L)
            {
                FingeringDiagramImages[i].color = activatedColorRecording;
                UIFingerImages[i].color = activatedColorRecording;
            }
            else
            {
                FingeringDiagramImages[i].color = imageInactiveColor;
                UIFingerImages[i].color = imageInactiveColor;
            }
        }
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Object that shows notes on a piano roll.
/// </summary>
public class PianoRoll : MonoBehaviour
{
    #region GameObject References
    public RectTransform rectTransform;
    [Tooltip("Shows the extents in which the piano roll will be created")]
    public Image pianoRollPlaceholder;
    public Text lowKeyText;
    public Text highKeyText;
    #endregion

    #region Prefab References
    public GameObject whiteKey;
    public GameObject blackKey;
    #endregion

    #region Parameter Variables
    [Tooltip("The MIDI value of the first note on the piano roll.")]
    public int startNote = 62;

    [Tooltip("The number of semitones above startNote that the piano roll will extend to.")]
    public int semitoneSpan = 25;

    [Tooltip("The sum of the left and right margins of the keys")]
    public float keyMargins = 1.0f;
    #endregion

    #region UI State
    [SerializeField]
    private float keyWidth = 25.0f;
    [SerializeField]
    private float keyHeight = 100.5f;
    [SerializeField]
    private float pianoRollWidth;
    [SerializeField]
    private float pianoRollHeight;

    private List<GameObject> keyObjects = new List<GameObject>();
    private int lastSetKeyIndex = 0;
    #endregion

    #region Initialization
    /// <summary>
    /// Sets up each key of the piano roll and the corresponding key labels.
    /// </summary>
    private void Start()
    {
        pianoRollPlaceholder.enabled = false;

        pianoRollWidth = rectTransform.sizeDelta.x;
        pianoRollHeight = rectTransform.sizeDelta.y;

        keyWidth = pianoRollWidth / (float)semitoneSpan;
        keyHeight = pianoRollHeight;

        for (int i = 0; i < semitoneSpan; i++)
        {
            bool isWhite = IsWhiteKey(startNote + i);
            var prefab = isWhite ? whiteKey : blackKey;
            var key = Instantiate(prefab, this.transform);

            var keyTransform = key.GetComponent<RectTransform>();
            keyTransform.position += new Vector3(i * keyWidth, 0.0f, 0.0f);
            keyTransform.sizeDelta = new Vector2(
                keyWidth - (2.0f * keyMargins),
                isWhite ? keyHeight : keyHeight - 20.0f
            ); // sorry
            keyObjects.Add(key);
        }

        Fingering lowKey = Fingering.GetFingeringByMIDINote(startNote);
        Fingering highKey = Fingering.GetFingeringByMIDINote(startNote + semitoneSpan - 1);

        lowKeyText.text = lowKey.name;
        highKeyText.text = highKey.name;

        lowKeyText.transform.position = keyObjects[0].transform.position + new Vector3(-keyWidth, keyHeight, 0.0f);
        float highKeyYOffset = IsWhiteKey(startNote + semitoneSpan - 1) ? keyHeight : keyHeight - 20.0f;
        highKeyText.transform.position = keyObjects[semitoneSpan - 1].transform.position + new Vector3(0.0f, highKeyYOffset, 0.0f);
    }
    #endregion

    #region Helpers
    /// <summary>
    /// Sets a key on the piano keyboard to be green.
    /// Giving this function -1 as a parameter will reset it (by convention).
    /// </summary>
    /// <param name="midiNote">The MIDI note to highlight.</param>
    public void SetKey(int midiNote)
    {
        // this is a little jank, but we clear the keyboard colors before checking if the note is valid
        // in order to implicitly have invalid notes reset the keyboard colors
        var lastKeyImage = keyObjects[lastSetKeyIndex].GetComponent<Image>();
        Color lastKeyNormalColor = IsWhiteKey(startNote + lastSetKeyIndex) ? Color.white : Color.black;
        lastKeyImage.color = lastKeyNormalColor;

        if (midiNote < startNote || midiNote >= startNote + semitoneSpan) return;

        int keyIndex = midiNote - startNote;
        var keyImage = keyObjects[keyIndex].GetComponent<Image>();
        keyImage.color = Color.green;
        lastSetKeyIndex = keyIndex;
    }
    #endregion

    #region Static Helpers
    /// <summary>
    /// Returns whether or not the given MIDI note is a white key on a piano.
    /// </summary>
    /// <param name="midiNote">The MIDI note to test.</param>
    /// <returns>Whether or not the given MIDI note is a white key.</returns>
    public static bool IsWhiteKey(int midiNote)
    {
        // starting at A0 (MIDI note 21) because there is really no need to go lower
        int normalizedNote = (midiNote - 21) % 12;
        // but i guess we can anyway for consistency's sake
        if (normalizedNote < 0) normalizedNote *= -1;

        switch (normalizedNote)
        {
            case 0: case 2: case 3: case 5:
            case 7: case 8: case 10:
                return true;
            default:
                return false;
        }
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PianoRoll : MonoBehaviour
{
    public GameObject whiteKey;
    public GameObject blackKey;
    public Text lowKeyText;
    public Text highKeyText;

    public int startNote = 72;
    public int semitoneSpan = 12;
    public float keyWidth = 25.0f;
    public float keyHeight = 100.5f;
    public float pianoRollWidth;

    private List<GameObject> keyObjects = new List<GameObject>();
    private int lastSetKeyIndex = 0;

    private void Start()
    {
        for (int i = 0; i < semitoneSpan; i++)
        {
            var prefab = IsWhiteKey(startNote + i) ? whiteKey : blackKey;
            var key = Instantiate(prefab, this.transform);
            var keyTransform = key.GetComponent<RectTransform>();
            keyTransform.position += new Vector3(i * keyWidth, 0.0f, 0.0f);
            keyObjects.Add(key);
        }

        Fingering lowKey = Fingering.GetFingeringByMIDINote(startNote);
        Fingering highKey = Fingering.GetFingeringByMIDINote(startNote + semitoneSpan - 1);

        lowKeyText.text = lowKey.name;
        highKeyText.text = highKey.name;

        lowKeyText.transform.position = keyObjects[0].transform.position + new Vector3(0.0f, keyHeight, 0.0f);
        highKeyText.transform.position = keyObjects[semitoneSpan - 1].transform.position + new Vector3(0.0f, keyHeight, 0.0f);
    }

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
}

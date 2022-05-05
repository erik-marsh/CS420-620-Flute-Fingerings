using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Class that defines an association between fingering keypresses, note names, note frequencies, and MIDI note values.
/// </summary>
public class Fingering
{
    #region Members
    public string name;
    public float frequency;
    public int midiNote;
    public uint keyCombination;
    #endregion

    #region Constructors
    public Fingering(string name, float frequency, int midiNote, uint keyCombination)
    {
        this.name = name;
        this.frequency = frequency;
        this.midiNote = midiNote;
        this.keyCombination = keyCombination;
    }
    #endregion

    #region Static Data
    // defines the bitmap for fingerings
    public static uint LIndex = 1 << 0;
    public static uint LMiddle = 1 << 1;
    public static uint LRing = 1 << 2;
    public static uint LPinky = 1 << 3;
    public static uint LThumb = 1 << 4;
    public static uint RIndex = 1 << 5;
    public static uint RMiddle = 1 << 6;
    public static uint RRing = 1 << 7;
    public static uint RPinky = 1 << 8;
    public static uint RThumb = 1 << 9;

    /// <summary>
    /// List of fingerints known to the application.
    /// Static to ensure equality tests will work.
    /// </summary>
    public static Fingering[] knownFingerings = {
        // first octave
        new Fingering("D4",  293.66f, 62, LThumb | LIndex | LMiddle | LRing | RIndex | RMiddle | RRing),
        new Fingering("Eb4", 311.13f, 63, LThumb | LIndex | LMiddle | LRing | RIndex | RMiddle | RRing | RPinky),
        new Fingering("E4",  329.63f, 64, LThumb | LIndex | LMiddle | LRing | RIndex | RMiddle | RPinky),
        new Fingering("F4",  349.23f, 65, LThumb | LIndex | LMiddle | LRing | RIndex | RPinky),
        new Fingering("Gb4", 369.99f, 66, LThumb | LIndex | LMiddle | LRing | RRing | RPinky),
        new Fingering("G4",  392.00f, 67, LThumb | LIndex | LMiddle | LRing | RPinky),
        new Fingering("Ab4", 415.30f, 68, LThumb | LIndex | LMiddle | LRing | LPinky | RPinky),
        new Fingering("A4",  440.00f, 69, LThumb | LIndex | LMiddle| RPinky),
        new Fingering("Bb4", 466.16f, 70, LThumb | LIndex | RIndex | RPinky),
        new Fingering("B4",  493.88f, 71, LThumb | LIndex | RPinky),

        // second octave
        new Fingering("C5",  523.25f, 72, LIndex | RPinky),
        new Fingering("Db5", 554.37f, 73, RPinky),
        new Fingering("D5",  587.33f, 74, LThumb | LMiddle | LRing | RIndex | RMiddle | RRing),
        new Fingering("Eb5", 622.25f, 75, LThumb | LMiddle | LRing | RIndex | RMiddle | RRing | RPinky),
        new Fingering("E5",  659.26f, 76, LThumb | LIndex | LMiddle | LRing | RIndex | RMiddle | RPinky),
        new Fingering("F5",  698.46f, 77, LThumb | LIndex | LMiddle | LRing | RIndex | RPinky),
        new Fingering("Gb5", 739.99f, 78, LThumb | LIndex | LMiddle | LRing | RRing | RPinky),
        new Fingering("G5",  783.99f, 79, LThumb | LIndex | LMiddle | LRing | RPinky),
        new Fingering("Ab5", 830.61f, 80, LThumb | LIndex | LMiddle | LRing | LPinky | RPinky),
        new Fingering("A5",  880.00f, 81, LThumb | LIndex | LMiddle | RPinky),
        new Fingering("Bb5", 932.33f, 82, LThumb | LIndex | RIndex | RPinky),
        new Fingering("B5",  987.77f, 83, LThumb | LIndex | RPinky),

        // third octave
        new Fingering("C6",  1046.50f, 84, LIndex | RPinky),
        new Fingering("Db6", 1108.73f, 85, RPinky),
        new Fingering("D6",  1174.66f, 86, LThumb | LMiddle | LRing | RPinky),

        // second octave alternates
        new Fingering("Db5", 554.37f, 73, RRing | RPinky),
        new Fingering("Db5", 554.37f, 73, RMiddle | RRing | RPinky),
        new Fingering("Db5", 554.37f, 73, RIndex | RMiddle | RRing | RPinky),

        // third octave alternates
        new Fingering("Db6", 1108.73f, 85, RRing | RPinky),
        new Fingering("Db6", 1108.73f, 85, RMiddle | RRing | RPinky),
        new Fingering("Db6", 1108.73f, 85, RIndex | RMiddle | RRing | RPinky)
    };

    public static Fingering nullFingering = new Fingering("", 0.0f, 0, 0);
    #endregion

    #region Static Helpers
    /// <summary>
    /// Returns the first known fingering with a specified key combination.
    /// </summary>
    /// <param name="keyCombination"></param>
    /// <returns></returns>
    public static Fingering GetFingeringByCombination(uint keyCombination)
    {
        // correction to ignore the right thumb, since it doesn't actually press keys
        if ((keyCombination & RThumb) != 0)
        {
            keyCombination &= ~RThumb;
        }

        Fingering res = nullFingering;
        foreach (var f in knownFingerings)
        {
            if (f.keyCombination == keyCombination)
                res = f;
        }

        return res;
    }

    /// <summary>
    /// Returns the first known fingering with the specified name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Fingering GetFingeringByName(string name)
    {
        Fingering res = nullFingering;
        foreach (var f in knownFingerings)
        {
            if (f.name == name)
                res = f;
        }

        return res;
    }

    /// <summary>
    /// Returns the first known fingering with the specified name.
    /// </summary>
    /// <param name="midiNote"></param>
    /// <returns></returns>
    public static Fingering GetFingeringByMIDINote(int midiNote)
    {
        Fingering res = nullFingering;
        foreach (var f in knownFingerings)
        {
            if (f.midiNote == midiNote)
                res = f;
        }

        return res;
    }
    #endregion
}
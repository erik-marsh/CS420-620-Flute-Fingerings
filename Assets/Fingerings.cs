using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Fingering
{
    public string name;
    public float frequency;
    public int midiNote;
    public uint keyCombination;

    public Fingering(string name, float frequency, int midiNote, uint keyCombination)
    {
        this.name = name;
        this.frequency = frequency;
        this.midiNote = midiNote;
        this.keyCombination = keyCombination;
    }

    #region Static Data
    // could have been an enum but it just made the fingering assignment way too verbose
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


    public static Fingering[] knownFingerings = {
        new Fingering("C5",  523.25f, 72, LIndex | RPinky),
        new Fingering("Db5", 554.37f, 73, RPinky),
        new Fingering("D5",  587.33f, 74, LThumb | LMiddle | LRing | RIndex | RMiddle | RRing),
        new Fingering("Eb5", 622.25f, 75, LThumb | LIndex | LMiddle | LRing | RIndex | RMiddle | RRing | RPinky),
        new Fingering("E5",  659.26f, 76, LThumb | LIndex | LMiddle | LRing | RIndex | RMiddle | RPinky),
        new Fingering("F5",  698.46f, 77, LThumb | LIndex | LMiddle | LRing | RIndex | RPinky),
        new Fingering("Gb5", 739.99f, 78, LThumb | LIndex | LMiddle | LRing | RRing | RPinky),
        new Fingering("G5",  783.99f, 79, LThumb | LIndex | LMiddle | LRing | RPinky),
        new Fingering("Ab5", 830.61f, 80, LThumb | LIndex | LMiddle | LRing | LPinky | RPinky),
        new Fingering("A5",  880.00f, 81, LThumb | LIndex | LMiddle | RPinky),
        new Fingering("Bb5", 932.33f, 82, LThumb | LIndex | RIndex | RPinky),
        new Fingering("B5",  987.77f, 83, LThumb | LIndex | RPinky)
    };

    public static Fingering nullFingering = new Fingering("", 0.0f, 0, 0);
    #endregion

    #region Static Helpers
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
    #endregion
}
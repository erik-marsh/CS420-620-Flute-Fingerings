using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Fingering
{
    //public enum Keys : uint
    //{
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
    //}

    public static Fingering[] knownFingerings = {
    /*public static Fingering C5  = */new Fingering("C5", LIndex | RPinky),
    /*public static Fingering Db5 = */new Fingering("Db5", RPinky),
    /*public static Fingering D5  = */new Fingering("D5", LThumb | LMiddle | LRing | RIndex | RMiddle | RRing),
    /*public static Fingering Eb5 = */new Fingering("Eb5", LThumb | LIndex | LMiddle | LRing | RIndex | RMiddle | RRing | RPinky),
    /*public static Fingering E5  = */new Fingering("E5", LThumb | LIndex | LMiddle | LRing | RIndex | RMiddle | RPinky),
    /*public static Fingering F5  = */new Fingering("F5", LThumb | LIndex | LMiddle | LRing | RIndex | RPinky),
    /*public static Fingering Gb5 = */new Fingering("Gb5", LThumb | LIndex | LMiddle | LRing | RRing | RPinky),
    /*public static Fingering G5  = */new Fingering("G5", LThumb | LIndex | LMiddle | LRing | RPinky),
    /*public static Fingering Ab5 = */new Fingering("Ab5", LThumb | LIndex | LMiddle | LRing | LPinky | RPinky),
    /*public static Fingering A5  = */new Fingering("A5", LThumb | LIndex | LMiddle | RPinky),
    /*public static Fingering Bb5 = */new Fingering("Bb5", LThumb | LIndex | RIndex | RPinky),
    /*public static Fingering B5  = */new Fingering("B5", LThumb | LIndex | RPinky)
    };

    public static Fingering nullFingering = new Fingering("", 0);

    public uint keyCombination;
    public string name;

    public Fingering(string name, uint keyCombination)
    {
        this.name = name;
        this.keyCombination = keyCombination;
    }

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
}
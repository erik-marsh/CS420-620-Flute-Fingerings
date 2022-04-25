using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotePlayer
{
    /// <summary>
    /// Helper to contain info needed to construct and play a note
    /// </summary>
    [System.Serializable]
    public class NoteInfo
    {
        public string _Name;
        public AudioClip _AudioClip;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

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

    /// <summary>
    /// Event arg helper
    /// </summary>
    public class NoteEventArgs : System.EventArgs
    {
        NoteInfo _Note;

        public NoteEventArgs(NoteInfo note)
        {
            _Note = note;
        }
    }

    /// <summary>
    /// Preset container for <see cref="NotePlayer"/>
    /// </summary>
    [CreateAssetMenu(fileName ="NotePlayerPreset_", menuName ="Note Player/Note Player Preset")]
    public class PN_NotePlayerPreset : ScriptableObject
    {
        #region Members

        /// <summary>
        /// Simple method to infer our data is safe. Could run into issues if SO is modified / reloaded during runtime.
        /// </summary>
        private bool _initialized = false;

        [Header("Prefab asset with an AudioSource attached")]
        public GameObject AudioSourcePrefab;

        [Header("Input Note audio files + names here")]
        public List<NoteInfo> List_Notes = new List<NoteInfo>();

        #endregion

        /// <summary>
        /// expensive, 1-time init to verify no note names are duplicated
        /// </summary>
        private void Initialize()
        {
            if (_initialized == true) return;

            //tests
            if (AudioSourcePrefab == null) throw new System.Exception("Audio Source Prefab reference missing");
            if (AudioSourcePrefab.GetComponent<AudioSource>() == null) throw new System.Exception("Audio Source Prefab AudioSource component missing");

            List<NoteInfo> validNotes = new List<NoteInfo>();
            List<NoteInfo> invalidNotes = new List<NoteInfo>();

            foreach (var n in List_Notes)
            {
                n._Name = n._Name.ToLower(); //only store as lower

                if(validNotes.Find(x => x._Name == n._Name) == null)
                {
                    validNotes.Add(n);
                }
                else
                {
                    invalidNotes.Add(n);
                }
            }

            //clear, then re-add ONLY valid notes
            List_Notes.Clear();
            foreach(var n in validNotes)
            {
                List_Notes.Add(n);
            }

            _initialized = true;
        }

        /// <summary>
        /// Instantiates an audio source based on this SO's AudioSource field
        /// </summary>
        /// <returns></returns>
        private GameObject CreateAudioSourceInstance(NoteInfo info)
        {
            GameObject go = Instantiate(AudioSourcePrefab);

            AudioSource audioSrc = go.GetComponent<AudioSource>();

            audioSrc.clip = info._AudioClip;

            return go;
        }

        /// <summary>
        /// Creates a new note player using supplied arg
        /// </summary>
        /// <param name="noteName">name of the note to play</param>
        public PN_NotePlayer CreateNotePlayer(string noteName)
        {
            Initialize();

            PN_NotePlayer notePlayer = new PN_NotePlayer();

            notePlayer._Preset = this;



            return notePlayer;
        }
    }
}
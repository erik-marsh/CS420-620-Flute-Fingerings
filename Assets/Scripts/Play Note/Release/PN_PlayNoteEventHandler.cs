using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotePlayer
{
    /// <summary>
    /// Container data structure that represents a recording of played notes
    /// </summary>
    public class PN_RecordingSession
    {
        [field: SerializeField]
        public List<RecordingEntry> List_RecordingEntries { get; private set; } = new List<RecordingEntry>();

        #region Helpers

        public class RecordingEntry
        {
            public float _NoteStartTime = -1f;
            public float _NoteEndTime = -1f;

            public NoteInfo _NoteInfo;
        }

        #endregion

        private bool ValidateEntry(RecordingEntry entry)
        {
            return (
                entry._NoteStartTime > 0 
                && entry._NoteEndTime > 0
                && entry._NoteInfo != null
                );
        }

        /// <summary>
        /// Initializes a new list of recording entries
        /// </summary>
        /// <returns></returns>
        public bool StartNewRecording()
        {
            List_RecordingEntries = new List<RecordingEntry>();
            return true;
        }

        /// <summary>
        /// Adds a new note entry to the active recording session
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool AddEntry(RecordingEntry entry)
        {
            if (!ValidateEntry(entry)) return false;

            List_RecordingEntries.Add(entry);
            return true;
        }


    }

    /// <summary>
    /// GAME LOGIC script that implements event handling to play notes
    /// </summary>
    /// <remarks>
    ///     Currently only accepts ONE NOTE AT A TIME! If you can somehow play chords on a flute, this does not work lol
    /// </remarks>
    public class PN_PlayNoteEventHandler : MonoBehaviour
    {
        #region Members

        public PN_NotePlayerPreset _Preset;
        public bool FLAG_Debug = false;

        public List<PN_RecordingSession> List_RecordingSessions = new List<PN_RecordingSession>();

        [SerializeField] private PN_RecordingSession.RecordingEntry CurrentEntry;

        #endregion



        #region Initialization

        void Awake()
        {
            //tests
            if (_Preset == null)
            {
                throw new System.Exception(ToString() + ": Preset not set");
            }
        }

        #endregion



        // add more subscriptions if you want to hook up a different dispatcher (i.e., a dispatcher that uses fingering events)
        // you can use the same event handlers as long as the EventArgs match   ~Jared
        #region Event Subscriptions

        private void OnEnable()
        {
            PN_TestEventDispatcher.OnNoteStart += PN_TestEventDispatcher_OnNoteStart; 
            PN_TestEventDispatcher.OnNoteEnd += PN_TestEventDispatcher_OnNoteEnd;
        }

        private void OnDisable()
        {
            PN_TestEventDispatcher.OnNoteStart -= PN_TestEventDispatcher_OnNoteStart;
            PN_TestEventDispatcher.OnNoteEnd -= PN_TestEventDispatcher_OnNoteEnd;
        }

        #endregion



        #region Event Handlers

        /// <summary>
        /// Creates a new <see cref="PN_RecordingSession.RecordingEntry"/> and holds it in this Component's field buffer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PN_TestEventDispatcher_OnNoteEnd(object sender, Utils.StringEventArgs e)
        {
            if (FLAG_Debug) Debug.Log("Note Start: " + e._Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PN_TestEventDispatcher_OnNoteStart(object sender, Utils.StringEventArgs e)
        {
            if (FLAG_Debug) Debug.Log("Note End: " + e._Name);
        }

        #endregion
    }


}
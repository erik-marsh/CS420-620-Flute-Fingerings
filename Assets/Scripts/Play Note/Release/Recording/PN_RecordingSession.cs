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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotePlayer
{
    /// <summary>
    /// Container data structure that represents a recording of played notes
    /// </summary>
    /// <remarks>
    /// Timestamp data must be manually relativized by a manager class, 
    ///  otherwise timestamps in <see cref="RecordingEntry"/>'s will be in absolute time from whenever they were recorded.
    /// </remarks>
    public class PN_RecordingSession
    {
        [field: SerializeField]
        public List<RecordingEntry> List_RecordingEntries { get; private set; } = new List<RecordingEntry>();

        #region Helpers

        [System.Serializable]
        public class RecordingEntry
        {
            public float _NoteStartTime = -1f;
            public float _NoteEndTime = -1f;

            public NoteInfo _NoteInfo;

            public bool IsValid { 
                get => (
                _NoteStartTime >= 0
                && _NoteEndTime >= _NoteStartTime
                && _NoteInfo != null
                );
            }
        }

        #endregion

        private bool ValidateEntry(RecordingEntry entry)
        {
            return entry.IsValid;
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
            if (!ValidateEntry(entry))
            {
                Debug.LogWarning(entry.ToString() + " Entry Invalid!" + "\n start " + entry._NoteStartTime + "\n end " + entry._NoteEndTime + "\n info " + entry._NoteInfo);
                return false;
            }

            List_RecordingEntries.Add(entry);
            return true;
        }

        /// <summary>
        /// Relativizes recording timestamp data such that the first note is played at t=0 and subsequent timestamps are expressed as an offset to that time.
        /// </summary>
        public void RelativizeRecordingTimestampData()
        {
            if (List_RecordingEntries.Count < 1) return;

            float startTimestamp = List_RecordingEntries[0]._NoteStartTime;
            //List_RecordingEntries[0]._NoteStartTime = 0f;

            foreach (RecordingEntry e in List_RecordingEntries)
            {
                e._NoteStartTime = e._NoteStartTime - startTimestamp;
                e._NoteEndTime = e._NoteEndTime - startTimestamp;

                if(e._NoteStartTime < 0 || e._NoteEndTime < 0)
                {
                    throw new System.Exception("Invalid Recording timestamp data \n" + JsonUtility.ToJson(e, true)); //no idea how this would get through but this makes this method safe
                }
            }
        }
    }
}
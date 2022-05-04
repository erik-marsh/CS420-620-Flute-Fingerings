using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotePlayer
{
    /// <summary>
    /// GAME LOGIC script that implements event handling to record notes to data strucure
    /// </summary>
    /// <remarks>
    /// <para>
    ///     Currently only accepts ONE NOTE AT A TIME! If you can somehow play chords on a flute, this does not work lol
    /// </para>
    /// <para>
    ///     Contains the list of <see cref="PN_RecordingSession"/> objects for use in MIDI playback or export
    /// </para>
    /// </remarks>
    public class PN_NoteRecorder_EventHandler : MonoBehaviour
    {
        #region Members

        public bool FLAG_Debug = false;

        /// <summary>
        /// The preset specifying our library of notes we can record
        /// </summary>
        public PN_NotePlayerPreset _Preset;

        /// <summary>
        /// Previous recording sessions.
        /// </summary>
        [SerializeField]
        private List<PN_RecordingSession> _List_InactiveRecordingSessions = new List<PN_RecordingSession>();

        /// <summary>
        /// The current note being recorded. Contains <see cref="NoteInfo"/> data alongside timestamps for playback.
        /// </summary>
        //[field: SerializeField] 
        public PN_RecordingSession.RecordingEntry CurrentActiveNote { get; private set; }

        /// <summary>
        /// The currently active recording session.
        /// </summary>
        [field: SerializeField]
        public PN_RecordingSession ActiveRecordingSession { get; private set; }

        #region Properties

        /// <summary>
        /// Previous recording sessions.
        /// </summary>
        public IReadOnlyCollection<PN_RecordingSession> PreviousRecordingSessions
        {
            get { return _List_InactiveRecordingSessions.AsReadOnly(); }
        }

        /// <summary>
        /// Is there an active recording session?
        /// </summary>
        public bool IsRecording
        {
            get
            {
                return (ActiveRecordingSession != null);
            }
        }

        #endregion

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

        #region Events

        // add more subscriptions if you want to hook up a different dispatcher (i.e., a dispatcher that uses fingering events)
        // you can use the same event handlers as long as the EventArgs match   ~Jared
        #region Event Subscriptions

        private void OnEnable()
        {
            PN_TestEventDispatcher.OnNoteStart += PN_TestEventDispatcher_OnNoteStart; 
            PN_TestEventDispatcher.OnNoteEnd += PN_TestEventDispatcher_OnNoteEnd;

            HandData.OnNoteStart += PN_TestEventDispatcher_OnNoteStart;
            HandData.OnNoteEnd += PN_TestEventDispatcher_OnNoteEnd;
        }

        private void OnDisable()
        {
            PN_TestEventDispatcher.OnNoteStart -= PN_TestEventDispatcher_OnNoteStart;
            PN_TestEventDispatcher.OnNoteEnd -= PN_TestEventDispatcher_OnNoteEnd;

            HandData.OnNoteStart -= PN_TestEventDispatcher_OnNoteStart;
            HandData.OnNoteEnd -= PN_TestEventDispatcher_OnNoteEnd;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Stores the current <see cref="PN_RecordingSession.RecordingEntry"/> to the <see cref="ActiveRecordingSession"/>
        /// </summary>
        private void PN_TestEventDispatcher_OnNoteEnd(object sender, Utils.StringEventArgs e)
        {
            if (FLAG_Debug)
                Debug.Log("Invoking " + nameof(PN_TestEventDispatcher_OnNoteEnd) + " with arg " + e._Name + " (sent by " + sender.GetType() + ")");

            if (CurrentActiveNote == null) return;

            CurrentActiveNote._NoteEndTime = Time.time;
            StoreActiveNote();
        }

        /// <summary>
        /// Creates a new <see cref="PN_RecordingSession.RecordingEntry"/> and holds it in this Component's field buffer.
        /// </summary>
        private void PN_TestEventDispatcher_OnNoteStart(object sender, Utils.StringEventArgs e)
        {
            if (FLAG_Debug)
                Debug.Log("Invoking " + nameof(PN_TestEventDispatcher_OnNoteStart) + " with arg " + e._Name + " (sent by " + sender.GetType() + ")");

            // store prev note if one exists
            StoreActiveNote();

            CurrentActiveNote = new PN_RecordingSession.RecordingEntry();
            CurrentActiveNote._NoteStartTime = Time.time;
            CurrentActiveNote._NoteInfo = _Preset.RetrieveNoteInfo(e._Name);

            //if the note info doesnt exist in our preset discard this active note.
            if (CurrentActiveNote._NoteInfo == null) CurrentActiveNote = null;
        }

        #endregion

        #endregion

        #region Internal Methods

        private void StoreActiveNote()
        {
            //discard if the active not is not valid or it doesnt exist
            if (ActiveRecordingSession == null || CurrentActiveNote == null || !CurrentActiveNote.IsValid) return;

            ActiveRecordingSession.AddEntry(CurrentActiveNote);
            CurrentActiveNote = null;
        }
        private void StoreCurrentSession()
        {
            //nullify absolute-time from this recording session. (instead, it will be expressed as base-offset time where startTime is t=0)
            ActiveRecordingSession.RelativizeRecordingTimestampData();

            _List_InactiveRecordingSessions.Add(ActiveRecordingSession);
            ActiveRecordingSession = null;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start a new recording session.
        /// </summary>
        /// <remarks>
        ///     Stops the active recording session if one exists.
        /// </remarks>
        public void StartRecording()
        {
            StopRecording();

            ActiveRecordingSession = new PN_RecordingSession();
        }

        /// <summary>
        /// Stop the current recording session.
        /// </summary>
        public void StopRecording()
        {
            if (ActiveRecordingSession == null) return;

            StoreCurrentSession();
        }

        #endregion
    }
}
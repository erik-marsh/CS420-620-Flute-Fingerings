using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;

using System.Threading.Tasks;

namespace NotePlayer
{
    /// <summary>
    /// Schedules and manages the playback of recording sessions
    /// </summary>
    /// <remarks>
    /// <para>
    ///     Contains public methods to invoke playback from external scripts
    /// </para>
    /// <para>
    ///     Invoking a new playback will stop any current playback
    /// </para>
    /// </remarks>
    public class PN_NotePlaybackManager : MonoBehaviour, System.IDisposable
    {
        #region Members

        public bool FLAG_Debug = false;

        /// <summary>
        /// Preset to look up <see cref="NoteInfo"/> information
        /// </summary>
        public PN_NotePlayerPreset _Preset;

        /// <summary>
        /// internal cancellation token source for multithreading
        /// </summary>
        private System.Threading.CancellationTokenSource _CancellationTokenSource = new System.Threading.CancellationTokenSource();

        /// <summary>
        /// The MIDI note player for playback.
        /// </summary>
        public MidiStreamPlayer _MIDIPlayer;

        #endregion

        #region Events
        public static event System.EventHandler OnPlaybackStopped;
        public static event System.EventHandler<int> OnNotePlayed;
        #endregion

        #region Initialization

        private void Awake()
        {
            //tests
            if (_Preset == null)
            {
                throw new System.Exception("Preset reference not found");
            }
            if (_CancellationTokenSource == null)
            {
                throw new System.Exception("Reference missing on init");
            }
        }

        #endregion

        #region Destroy

        private void OnDestroy()
        {
            Dispose();
        }

        /// <summary>
        /// Free multithreading resources and cancel running asynchronous tasks
        /// </summary>
        public void Dispose()
        {
            //cleanup multithreading
            _CancellationTokenSource.Cancel();
            _CancellationTokenSource.Dispose();
        }

        #endregion

        #region Multithreading

        protected async System.Threading.Tasks.Task Async_PlaybackRecording(System.Threading.CancellationToken cancelToken, PN_RecordingSession session)
        {
            if (FLAG_Debug) Debug.Log("Playback started on session data: \n" + JsonUtility.ToJson(session));

            string debugOutput = "Notes added { Name, RelativeStartTime }:\n";

            System.Threading.CancellationTokenSource cts = System.Threading.CancellationTokenSource.CreateLinkedTokenSource(cancelToken);

            try
            {
                if (FLAG_Debug) Debug.Log("Entries count for session: " + session.List_RecordingEntries.Count.ToString());

                //loop variables
                int curNoteIndex = 0;
                float nextDelay_Seconds;
                while (!cancelToken.IsCancellationRequested && curNoteIndex < session.List_RecordingEntries.Count)
                {
                    if (_MIDIPlayer == null)
                    {
                        Debug.LogWarning("Cannot play note: no MIDI player is set for " + gameObject.name);
                    }
                    else
                    {
                        float noteDuration = Mathf.Abs(session.List_RecordingEntries[curNoteIndex]._NoteStartTime - session.List_RecordingEntries[curNoteIndex]._NoteEndTime);

                        // use the MIDI player library to create sound
                        _MIDIPlayer.MPTK_PlayEvent(new MPTKEvent {
                            Command = MPTKCommand.NoteOn,
                            Value = session.List_RecordingEntries[curNoteIndex]._MIDINote,
                            Channel = 0,
                            Duration = (int)(noteDuration * 1000.0f), // MIDI durations are in milliseconds (and as an integer, which is weird)
                            Velocity = 100,
                            Delay = 0
                        });
                        OnNotePlayed?.Invoke(this, session.List_RecordingEntries[curNoteIndex]._MIDINote);
                    }

                    //compute delay offset to next note
                    curNoteIndex++;
                    nextDelay_Seconds = session.List_RecordingEntries[curNoteIndex]._NoteStartTime - session.List_RecordingEntries[Mathf.Max(0, (curNoteIndex - 1))]._NoteStartTime;

                    if (FLAG_Debug) Debug.Log("Delay to next: " + nextDelay_Seconds.ToString());

                    // wait until next note
                    // TODO: investigate the 1ms delay
                    // 1 added ms delay for some reason helps...
                    await System.Threading.Tasks.Task.Delay((int)(1000 * nextDelay_Seconds) + 1, cts.Token); //this function has time arg in ms
                }

                if (FLAG_Debug)
                {
                    Debug.Log("note indices utilized for playback: " + curNoteIndex.ToString());
                    Debug.Log("cancel requested: " + cancelToken.IsCancellationRequested.ToString());
                    Debug.Log(debugOutput);
                }
            }
            catch (System.OperationCanceledException)
            {
                //how do we handle cancel?
                //OnCooldownAvailable?.Invoke(this, new CooldownTrackerEventArgs(this));
                if (FLAG_Debug) Debug.Log(ToString() + ": Task Canceled early!");
            }
            finally
            {
                if (FLAG_Debug) Debug.Log(ToString() + ": Disposal of Cancellation Token Source!");
                cts.Dispose();

                // invoke event
                OnPlaybackStopped?.Invoke(this, new System.EventArgs());
            }
        }

        #endregion

        #region Public Methods

        public bool PlayRecordingSession(PN_RecordingSession session)
        {
            if (session == null) return false;

            //cancel any active playback thread(s)
            Dispose();

            _CancellationTokenSource = new System.Threading.CancellationTokenSource();

            System.Threading.Tasks.Task t = Async_PlaybackRecording(_CancellationTokenSource.Token, session);


            return true;
        }

        /// <summary>
        /// Stops active playback
        /// </summary>
        /// <remarks>
        /// Any notes that were created before this call will still play their full duration.
        /// </remarks>
        /// <returns>Whether playback was canceled successfully</returns>
        public bool StopActivePlayback()
        {
            Dispose();
            //_CancellationTokenSource = new System.Threading.CancellationTokenSource();

            // invoke event
            OnPlaybackStopped?.Invoke(this, new System.EventArgs());

            return true;
        }

        #endregion
    }
}
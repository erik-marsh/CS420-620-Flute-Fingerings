using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                int curNoteIndex = 0;
                if (FLAG_Debug) Debug.Log("Entries count for session: " + session.List_RecordingEntries.Count.ToString());
                while (!cancelToken.IsCancellationRequested && curNoteIndex < session.List_RecordingEntries.Count)
                {
                    //play current note
                    PN_NotePlayer np = _Preset.CreateNotePlayer(session.List_RecordingEntries[curNoteIndex]._NoteInfo._Name);
                    np._NoteDuration = Mathf.Abs(session.List_RecordingEntries[curNoteIndex]._NoteStartTime - session.List_RecordingEntries[curNoteIndex]._NoteEndTime);
                    
                    //append to debug log
                    if(FLAG_Debug)
                    {
                        debugOutput += "{ " + session.List_RecordingEntries[curNoteIndex]._NoteInfo._Name + ", " + session.List_RecordingEntries[curNoteIndex]._NoteStartTime + " }, ";
                    }

                    //wait until next note
                    curNoteIndex++;
                    //TODO: investigate the 1ms delay
                    //1 added ms delay for some reason helps...
                    await System.Threading.Tasks.Task.Delay((int)(1000 * session.List_RecordingEntries[curNoteIndex]._NoteStartTime) + 1, cts.Token); 
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

        #endregion
    }
}
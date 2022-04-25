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
    public class PN_NotePlaybackManager : MonoBehaviour
    {
        #region Members

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

        #region Multithreading

        protected async System.Threading.Tasks.Task NotePlayback(System.Threading.CancellationToken ct)
        {

        }

        #endregion

        #region Public Methods

        public bool PlayRecordingSession()
        {


            return true;
        }

        #endregion
    }
}
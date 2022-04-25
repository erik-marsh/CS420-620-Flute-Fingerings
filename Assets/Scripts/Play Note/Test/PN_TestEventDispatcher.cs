using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotePlayer
{
    /// <summary>
    /// GAME LOGIC test event publisher to verify event handling works
    /// </summary>
    public class PN_TestEventDispatcher : MonoBehaviour
    {
        #region Events

        public static event System.EventHandler<Utils.StringEventArgs> OnNoteStart;
        public static event System.EventHandler<Utils.StringEventArgs> OnNoteEnd;

        #endregion

        public bool ContinueTesting { get; set; } = true;

        public PN_NotePlayerPreset _Preset;

        private void Awake()
        {
            //tests
            if (_Preset == null)
            {
                throw new System.Exception(ToString() + ": Preset not set");
            }
        }

        /// <summary>
        /// Periodically sends a note event
        /// </summary>
        /// <returns></returns>
        protected IEnumerator I_Test1()
        {
            while(ContinueTesting)
            {
                OnNoteStart?.Invoke(this, new Utils.StringEventArgs("c1"));
                yield return new WaitForSeconds(1f);

                OnNoteEnd?.Invoke(this, new Utils.StringEventArgs("c1"));
                yield return new WaitForSeconds(1f);
            }
        }
    }


}
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

        public bool FLAG_Debug = false;

        public PN_NotePlayerPreset _Preset;

        private void Awake()
        {
            //tests
            if (_Preset == null)
            {
                throw new System.Exception(ToString() + ": Preset not set");
            }
        }

        private void Start()
        {
            StartCoroutine(I_Test1());
        }

        /// <summary>
        /// Periodically sends a note event
        /// </summary>
        /// <returns></returns>
        protected IEnumerator I_Test1()
        {
            while(ContinueTesting)
            {
                if (FLAG_Debug) Debug.Log("Invoking Note Events!");

                OnNoteStart?.Invoke(this, new Utils.StringEventArgs("c1"));
                yield return new WaitForSeconds(1f);

                OnNoteEnd?.Invoke(this, new Utils.StringEventArgs("c1"));
                yield return new WaitForSeconds(1f);
            }
        }
    }


}
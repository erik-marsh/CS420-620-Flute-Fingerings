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

        #region Helpers

        public enum TestChoice { test1, test2 }

        #endregion

        public bool ContinueTesting { get; set; } = true;

        public bool FLAG_Debug = false;

        public TestChoice TestToRun;

        //may not actually need the preset ref on this component
        public PN_NotePlayerPreset _Preset;

        #region Initialization

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
            switch(TestToRun)
            {
                case TestChoice.test1: StartCoroutine(I_Test1()); break;
                case TestChoice.test2: StartCoroutine(I_Test2(5f)); break;
                default: Debug.LogError("Test not found!"); break;
            }            
        }

        #endregion

        /// <summary>
        /// Periodically sends a note event
        /// </summary>
        protected IEnumerator I_Test1()
        {
            while (ContinueTesting)
            {
                if (FLAG_Debug) Debug.Log("Invoking Note Events!");

                OnNoteStart?.Invoke(this, new Utils.StringEventArgs("c1"));
                yield return new WaitForSeconds(1f);

                OnNoteEnd?.Invoke(this, new Utils.StringEventArgs("c1"));
                yield return new WaitForSeconds(1f);
            }
        }
        /// <summary>
        /// Periodically sends a note event with a set duration
        /// </summary>
        protected IEnumerator I_Test2(float duration)
        {
            float startTime = Time.time;
            while (Mathf.Abs(Time.time - startTime) < duration)
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NotePlayer
{
    /// <summary>
    /// Note Player instance to play audio in worldspace
    /// </summary>
    public class PN_NotePlayer : MonoBehaviour
    {
        private float s_RoutineUpdateInterval = 1f;

        public PN_NotePlayerPreset _Preset;

        public NoteInfo _DesiredNote;

        private AudioSource _AudioSource;

        private void Awake()
        {
            _AudioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Guarantees Preset was input during init (awake-time). Plays the desired note
        /// </summary>
        /// <exception cref="System.Exception"></exception>
        protected void Start()
        {
            //tests
            if (_Preset == null)
            {
                throw new System.Exception(ToString() + ": Preset not set");
            }
            if (_DesiredNote == null)
            {
                throw new System.Exception(ToString() + ": Preset not set");
            }
            if (_AudioSource == null)
            {
                throw new System.Exception(ToString() + ": Preset not set");
            }

            //play note then destroy this gameobject
            StartCoroutine(I_ContinuePlayNote());
        }

        public IEnumerator I_ContinuePlayNote()
        {
            _AudioSource.Play();
            while(_AudioSource.isPlaying)
            {
                yield return new WaitForSeconds(s_RoutineUpdateInterval);
            }
            Destroy(gameObject);
        }
    }
}
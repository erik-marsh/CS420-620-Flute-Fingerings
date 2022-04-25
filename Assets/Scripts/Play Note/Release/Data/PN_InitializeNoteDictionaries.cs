using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotePlayer
{
    /// <summary>
    /// Reinitializes SO init upon new instance loading. 
    /// </summary>
    /// <remarks>
    /// useful for re-init on scene load
    /// </remarks>
    public class PN_InitializeNoteDictionaries : MonoBehaviour
    {
        public bool FLAG_Debug = false;

        /// <summary>
        /// Resource folder paths to acquire SO assets from.
        /// </summary>
        public string[] ResourcePaths;

        void Awake()
        {
            List<PN_NotePlayerPreset> listPresets = new List<PN_NotePlayerPreset>();

            //load all resources into our list
            foreach (var s in ResourcePaths)
            {
                var addPresets = Resources.LoadAll<PN_NotePlayerPreset>(s);

                foreach( var p in addPresets)
                {
                    listPresets.Add(p);
                }
            }

            //invoke reinitialization onto all resources
            foreach(var p in listPresets)
            {
                p.ReInitialize();
                if (FLAG_Debug) Debug.Log(p.ToString() + " successfully reinitialized.");
            }
        }
    }

}
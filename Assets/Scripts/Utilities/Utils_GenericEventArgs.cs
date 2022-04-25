using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Event arg helper
    /// </summary>
    public class StringEventArgs : System.EventArgs
    {
        public string _Name;

        public StringEventArgs(string name)
        {
            _Name = name;
        }
    }
}
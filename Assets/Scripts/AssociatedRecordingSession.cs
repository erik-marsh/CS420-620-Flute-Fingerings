using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Allows Button objects to be aware of which recording index they correspond to.
/// </summary>
public class AssociatedRecordingSession : MonoBehaviour
{
    public RecordingManager recordingManager;

    public int sessionID;

    /// <summary>
    /// Callback function used to set the current recording in the recording manager.
    /// </summary>
    public void SelectRecording()
    {
        recordingManager.SelectRecording(sessionID);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssociatedRecordingSession : MonoBehaviour
{
    public RecordingManager recordingManager;

    public int sessionID;

    public void SelectRecording()
    {
        recordingManager.SelectRecording(sessionID);
    }
}

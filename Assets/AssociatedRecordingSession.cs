using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssociatedRecordingSession : MonoBehaviour
{
    public RecordingManager recordingManager;

    public int sessionID;

    public void SelectRecording()
    {
        recordingManager.sessionToPlay = sessionID;
        recordingManager.playButton.interactable = true;
    }
}

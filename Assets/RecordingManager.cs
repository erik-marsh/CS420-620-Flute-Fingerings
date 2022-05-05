using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using NotePlayer;

public class RecordingManager : MonoBehaviour
{
    public PN_NoteRecorder_EventHandler eventHandler;
    public PN_NotePlaybackManager playbackManger;
    public RectTransform scrollViewContent;
    public Button playButton;

    private Text playButtonText;
    private string playButtonInactiveText = "Play selected recording";
    private string playButtonActiveText = "Playing...";

    public GameObject buttonPrefab;

    // -1 if there are no sessions to play
    public int sessionToPlay = -1;

    private List<GameObject> scrollViewContentItems = new List<GameObject>();
    private int lastRepopulationCount = 0;

    #region Unity Messages
    private void Awake()
    {
        playButtonText = playButton.GetComponentInChildren<Text>();
    }

    private void OnEnable()
    {
        PN_NotePlaybackManager.OnPlaybackStopped += OnPlaybackStopped;
    }

    private void OnDisable()
    {
        PN_NotePlaybackManager.OnPlaybackStopped -= OnPlaybackStopped;
    }
    #endregion

    private void OnPlaybackStopped(object sender, System.EventArgs e)
    {
        playButtonText.text = playButtonInactiveText;
    }

    #region Button Callbacks
    public void ToggleRecording(Toggle toggle)
    {
        if (toggle.isOn)
        {
            eventHandler.StartRecording();
        }
        else
        {
            eventHandler.StopRecording();
            PopulateRecordingList();
        }
    }

    public void PlaySelectedRecording()
    {
        if (sessionToPlay < 0) return;

        var session = eventHandler.PreviousRecordingSessions.ElementAt(sessionToPlay);

        if (session.List_RecordingEntries.Count > 0)
        {
            playbackManger.PlayRecordingSession(session);
            playButtonText.text = playButtonActiveText;
        }
    }
    #endregion

    public void PopulateRecordingList()
    {
        for (int i = lastRepopulationCount; i < eventHandler.PreviousRecordingSessions.Count; i++)
        {
            PN_RecordingSession session = eventHandler.PreviousRecordingSessions.ElementAt(i);
            var buttonObj = Instantiate(buttonPrefab, scrollViewContent);
            scrollViewContentItems.Add(buttonObj);

            var associatedSession = buttonObj.GetComponent<AssociatedRecordingSession>();
            associatedSession.recordingManager = this;
            associatedSession.sessionID = i;

            var text = buttonObj.GetComponentInChildren<Text>();
            // sorry about this
            text.text = "Recording " + session.recordingID + " (" + session.List_RecordingEntries.Count + " notes, " + (session.List_RecordingEntries.Count == 0 ? "0sec)" : session.List_RecordingEntries[session.List_RecordingEntries.Count - 1]._NoteEndTime + "sec)");
        }

        lastRepopulationCount = eventHandler.PreviousRecordingSessions.Count;
    }

    public void ClearRecordingList()
    {
        foreach (var obj in scrollViewContentItems)
        {
            Destroy(obj);
        }

        scrollViewContentItems.Clear();
        lastRepopulationCount = 0;
        sessionToPlay = -1;
        playButton.interactable = false;
    }
}

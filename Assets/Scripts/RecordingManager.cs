using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using NotePlayer;

public class RecordingManager : MonoBehaviour
{
    #region Component References
    // for data retrieval
    public PN_NoteRecorder_EventHandler eventHandler;
    public PN_NotePlaybackManager playbackManger;
    public HandData handData;

    // for modifying UI state
    public RectTransform scrollViewContent;
    public Button exportButton;
    public Text totalPlaybackLengthText;
    public Text currentPlaybackTimeText;

    public Button playButton;
    private Text playButtonText;
    private string playButtonInactiveText = "Play selected recording";
    private string playButtonActiveText = "Playing...";
    #endregion

    #region Prefab References
    // prefab used to instantiate buttons that let you select recordings for playback
    public GameObject buttonPrefab;
    #endregion

    #region Script Parameters
    [Header("Recording Button Default")]
    public ColorBlock recordingButtonDefault;
    [Header("Recording Button Active")]
    public ColorBlock recordingButtonActive;
    #endregion

    #region Session Playback
    // timer to determine when to show blue visualizations (recording playback) or green visualizations (general-purpose)
    private bool isPlaying = false;
    private float recordingTimer = 0.0f;

    // -1 if there are no sessions to play
    public int sessionToPlay = -1;

    private List<GameObject> scrollViewContentItems = new List<GameObject>();
    private int lastRepopulationCount = 0;
    #endregion

    #region Unity Messages
    private void Awake()
    {
        playButtonText = playButton.GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if (!isPlaying) return;

        recordingTimer += Time.deltaTime;

        if (recordingTimer <= eventHandler.PreviousRecordingSessions.ElementAt(sessionToPlay).GetRecordingDuration())
        {
            // extra space is for formatting
            currentPlaybackTimeText.text = recordingTimer.ToString("0.00") + "sec ";
        }
        else
        {
            isPlaying = false;
            handData.isPlayingRecording = false;
            handData.SetRecordingNote(-1); // and disable any remaining recording visualizations
        }
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
        playButton.colors = recordingButtonDefault;
        //isPlaying = false;
    }

    #region Button Callbacks
    /// <summary>
    /// Toggles recording. If toggled off, the list of recordings is updated.
    /// </summary>
    /// <param name="toggle"></param>
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

    /// <summary>
    /// If a valid recording session is selected, begin playback of that session in another thread.
    /// </summary>
    public void PlaySelectedRecording()
    {
        if (sessionToPlay < 0) return;

        var session = eventHandler.PreviousRecordingSessions.ElementAt(sessionToPlay);

        if (session.List_RecordingEntries.Count > 0)
        {
            playbackManger.PlayRecordingSession(session);
            isPlaying = true;
            recordingTimer = 0.0f;
            playButtonText.text = playButtonActiveText;
            playButton.colors = recordingButtonActive;
        }
    }
    #endregion

    #region Helpers
    /// <summary>
    /// Fills out the list of recordings with recordings that have been recorded since the last recording.
    /// </summary>
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
            text.text = $"Recording {session.recordingID} ({session.List_RecordingEntries.Count} notes, {session.GetRecordingDuration().ToString("0.00")} sec)";
        }

        lastRepopulationCount = eventHandler.PreviousRecordingSessions.Count;
    }

    /// <summary>
    /// Clears the recording list.
    /// </summary>
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
        exportButton.interactable = false;
    }

    /// <summary>
    /// Sets a recoring session as the session that will be played when the play button is pressed.
    /// </summary>
    /// <param name="sessionID"></param>
    public void SelectRecording(int sessionID)
    {
        // reset last button's color
        if (sessionToPlay >= 0)
            scrollViewContentItems[sessionToPlay].GetComponent<Button>().colors = recordingButtonDefault;

        sessionToPlay = sessionID;
        scrollViewContentItems[sessionToPlay].GetComponent<Button>().colors = recordingButtonActive;

        playButton.interactable = true;
        exportButton.interactable = true;

        PN_RecordingSession session = eventHandler.PreviousRecordingSessions.ElementAt(sessionToPlay);
        currentPlaybackTimeText.text = "0.00sec ";
        totalPlaybackLengthText.text = $"/ {session.GetRecordingDuration().ToString("0.00")}sec";
    }
    #endregion
}

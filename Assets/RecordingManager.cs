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
    public HandData handData;

    public RectTransform scrollViewContent;
    public Button playButton;
    public Button exportButton;
    public Text totalPlaybackLengthText;
    public Text currentPlaybackTimeText;

    private bool isPlaying = false;
    private float recordingTimer = 0.0f;

    private Text playButtonText;
    private string playButtonInactiveText = "Play selected recording";
    private string playButtonActiveText = "Playing...";

    public GameObject buttonPrefab;
    [Header("Recording Button Default")]
    public ColorBlock recordingButtonDefault;
    [Header("Recording Button Active")]
    public ColorBlock recordingButtonActive;
    public Color _recordingButtonActive = new Color(0.447f, 0.522f, 1.0f, 1.0f);

    // -1 if there are no sessions to play
    public int sessionToPlay = -1;

    private List<GameObject> scrollViewContentItems = new List<GameObject>();
    private int lastRepopulationCount = 0;

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
            isPlaying = true;
            handData.StartPlayingRecording();
            recordingTimer = 0.0f;
            playButtonText.text = playButtonActiveText;
            playButton.colors = recordingButtonActive;
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
            text.text = $"Recording {session.recordingID} ({session.List_RecordingEntries.Count} notes, {session.GetRecordingDuration().ToString("0.00")} sec)";
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
        exportButton.interactable = false;
    }

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
}

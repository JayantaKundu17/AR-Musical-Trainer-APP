using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private DrumKitManager drumKitManager;

    [Header("Instruments")]
    [SerializeField] private GameObject piano;

    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject instrumentSelectionPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Instrument UI")]
    [SerializeField] private GameObject pianoStop;

    private void Start()
    {
        ShowMainMenu();
    }

    /// <summary>
    /// Called when the Play button is pressed.
    /// Opens the currently selected instrument.
    /// </summary>
    public void PlaySelectedInstrument()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found.");
            return;
        }

        switch (GameManager.Instance.SelectedInstrument)
        {
            case "Drum":

                // Hide Piano STOP
                if (pianoStop != null)
                    pianoStop.SetActive(false);

                if (drumKitManager != null)
                {
                    drumKitManager.Show();
                }
                else
                {
                    Debug.LogError("DrumKitManager is not assigned.");
                }

                break;

            case "Piano":
                OpenPiano();
                break;

            case "Guitar":
                OpenGuitar();
                break;

            default:
                Debug.LogWarning("Please select an instrument first.");
                break;
        }
    }

    private void OpenPiano()
    {
        // Hide all menu panels
        mainMenuPanel.SetActive(false);
        instrumentSelectionPanel.SetActive(false);
        settingsPanel.SetActive(false);

        // Show piano
        if (piano != null)
        {
            piano.SetActive(true);
        }
        else
        {
            Debug.LogError("Piano is not assigned.");
        }

        // Show Piano STOP button
        if (pianoStop != null)
        {
            pianoStop.SetActive(true);
        }
        else
        {
            Debug.LogError("Piano STOP button is not assigned.");
        }
    }

    private void OpenGuitar()
    {
        // Hide Piano STOP if Guitar is opened
        if (pianoStop != null)
            pianoStop.SetActive(false);

        Debug.Log("Guitar is not implemented yet.");
    }

    public void OpenInstrumentSelection()
    {
        // Hide Piano STOP
        if (pianoStop != null)
            pianoStop.SetActive(false);

        mainMenuPanel.SetActive(false);
        instrumentSelectionPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        // Hide Piano STOP
        if (pianoStop != null)
            pianoStop.SetActive(false);

        mainMenuPanel.SetActive(false);
        instrumentSelectionPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void ShowMainMenu()
    {
        Debug.Log($"MainMenuController on: {gameObject.name}");
        Debug.Log($"mainMenuPanel = {(mainMenuPanel == null ? "NULL" : mainMenuPanel.name)}");

        // Hide Piano STOP
        if (pianoStop != null)
            pianoStop.SetActive(false);

        // Show main menu
        mainMenuPanel.SetActive(true);
        instrumentSelectionPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
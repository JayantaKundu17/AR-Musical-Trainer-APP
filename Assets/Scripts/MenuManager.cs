using UnityEngine;

/// <summary>
/// Central controller that shows/hides the three menu panels
/// (Main Menu, Instrument Selection, Settings). Only one panel is
/// active at a time.
///
/// Attach to an empty GameObject named "MenuManager" and assign the
/// three panels in the Inspector.
/// </summary>
public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject instrumentSelectionPanel;
    [SerializeField] private GameObject settingsPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu() => SetActivePanel(mainMenuPanel);

    public void ShowInstrumentSelection() => SetActivePanel(instrumentSelectionPanel);

    public void ShowSettings() => SetActivePanel(settingsPanel);

    private void SetActivePanel(GameObject panelToShow)
    {
        mainMenuPanel.SetActive(panelToShow == mainMenuPanel);
        instrumentSelectionPanel.SetActive(panelToShow == instrumentSelectionPanel);
        settingsPanel.SetActive(panelToShow == settingsPanel);
    }

    public void ExitApplication()
    {
        Debug.Log("[MenuManager] Exit requested.");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif WINDOWS_UWP
        Windows.ApplicationModel.Core.CoreApplication.Exit();
#else
        Application.Quit();
#endif
    }
}

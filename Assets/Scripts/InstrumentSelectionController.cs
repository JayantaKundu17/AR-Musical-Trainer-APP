using UnityEngine;
using UnityEngine.UI;

public class InstrumentSelectionController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button pianoButton;
    [SerializeField] private Button guitarButton;
    [SerializeField] private Button drumButton;
    [SerializeField] private Button backButton;

    [Header("Controller")]
    [SerializeField] private MainMenuController mainMenuController;

    private void Start()
    {
        if (pianoButton != null)
            pianoButton.onClick.AddListener(() => SelectInstrument("Piano"));

        if (guitarButton != null)
            guitarButton.onClick.AddListener(() => SelectInstrument("Guitar"));

        if (drumButton != null)
            drumButton.onClick.AddListener(() => SelectInstrument("Drum"));

        if (backButton != null)
            backButton.onClick.AddListener(ReturnToMainMenu);
    }

    private void SelectInstrument(string instrumentName)
    {
        Debug.Log($"[InstrumentSelection] Selected: {instrumentName}");

        if (GameManager.Instance != null)
            GameManager.Instance.SetSelectedInstrument(instrumentName);

        ReturnToMainMenu();
    }

    private void ReturnToMainMenu()
    {
        if (mainMenuController != null)
        {
            mainMenuController.ShowMainMenu();
        }
        else
        {
            Debug.LogError("MainMenuController is not assigned in InstrumentSelectionController.");
        }
    }
}
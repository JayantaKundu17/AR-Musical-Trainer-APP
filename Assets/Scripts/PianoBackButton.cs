using UnityEngine;

public class PianoBackButton : MonoBehaviour
{
    [Header("Piano")]
    [SerializeField] private GameObject piano;

    [Header("Main Menu")]
    [SerializeField] private GameObject mainMenuPanel;

    [Header("Piano UI")]
    [SerializeField] private GameObject pianoStop;

    public void BackToMenu()
    {
        // Hide the piano
        if (piano != null)
        {
            piano.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Piano is not assigned to PianoBackButton.");
        }

        // Hide the Piano STOP button
        if (pianoStop != null)
        {
            pianoStop.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Piano STOP button is not assigned to PianoBackButton.");
        }

        // Show the main menu
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Main Menu Panel is not assigned to PianoBackButton.");
        }
    }

    private void OnMouseDown()
    {
        BackToMenu();
    }
}
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

/// <summary>
/// Handles the Settings submenu: Volume control and Interaction sensitivity
/// selection, plus Back. Attach to the SettingsPanel GameObject.
/// </summary>
public class SettingsController : MonoBehaviour
{
    [Header("Volume Control")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Text volumeValueText;
    [SerializeField] private AudioMixer audioMixer;
    private const string MixerVolumeParam = "MasterVolume";

    [Header("Interaction Sensitivity")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private InteractionSensitivityController sensitivityController;

    [Header("Navigation")]
    [SerializeField] private Button backButton;
    [SerializeField] private MainMenuController mainMenuController;

    private void Start()
    {
        float savedVolume = GameManager.Instance ? GameManager.Instance.GetSavedVolume() : 0.75f;
        float savedSensitivity = GameManager.Instance ? GameManager.Instance.GetSavedSensitivity() : 0.5f;

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
            OnVolumeChanged(savedVolume);
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = savedSensitivity;
            sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
            OnSensitivityChanged(savedSensitivity);
        }

        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();

            if (mainMenuController != null)
                backButton.onClick.AddListener(mainMenuController.ShowMainMenu);
            else
                Debug.LogError("MainMenuController is not assigned in SettingsController.");
        }
    }

    public void OnVolumeChanged(float linearValue)
    {
        linearValue = Mathf.Clamp(linearValue, 0.0001f, 1f);

        if (volumeValueText != null)
            volumeValueText.text = Mathf.RoundToInt(linearValue * 100) + "%";

        if (audioMixer != null)
        {
            float dB = Mathf.Log10(linearValue) * 20f;
            audioMixer.SetFloat(MixerVolumeParam, dB);
        }

        AudioListener.volume = linearValue;

        GameManager.Instance?.SaveVolume(linearValue);
    }

    public void OnSensitivityChanged(float value)
    {
        value = Mathf.Clamp01(value);

        if (sensitivityController != null)
            sensitivityController.SetSensitivity(value);

        GameManager.Instance?.SaveSensitivity(value);
    }
}
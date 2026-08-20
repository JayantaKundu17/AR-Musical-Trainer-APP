using UnityEngine;

/// <summary>
/// Persistent singleton that survives scene loads. Stores the state that the
/// menus read/write: selected instrument, volume, interaction sensitivity.
/// Attach to an empty GameObject named "GameManager" in your first/boot scene.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Runtime state")]
    public string SelectedInstrument = "";

    private const string VolumeKey = "MasterVolume";
    private const string SensitivityKey = "InteractionSensitivity";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public float GetSavedVolume() => PlayerPrefs.GetFloat(VolumeKey, 0.75f);
    public void SaveVolume(float value) => PlayerPrefs.SetFloat(VolumeKey, value);

    public float GetSavedSensitivity() => PlayerPrefs.GetFloat(SensitivityKey, 0.5f);
    public void SaveSensitivity(float value) => PlayerPrefs.SetFloat(SensitivityKey, value);

    public void SetSelectedInstrument(string instrumentName)
    {
        SelectedInstrument = instrumentName;
        Debug.Log($"[GameManager] Instrument selected: {instrumentName}");
    }
}

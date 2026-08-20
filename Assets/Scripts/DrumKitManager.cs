using UnityEngine;

public class DrumKitManager : MonoBehaviour
{
    public static DrumKitManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject drumKitRoot;

    [SerializeField] private GameObject menuCanvas;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (drumKitRoot != null)
            drumKitRoot.SetActive(false);

        if (menuCanvas != null)
            menuCanvas.SetActive(true);
    }

    public void Show()
    {
        drumKitRoot.SetActive(true);
        menuCanvas.SetActive(false);
    }

    public void Hide()
    {
        drumKitRoot.SetActive(false);
        menuCanvas.SetActive(true);
    }

    public void Toggle()
    {
        if (drumKitRoot.activeSelf)
            Hide();
        else
            Show();
    }

    public bool IsDrumKitVisible()
    {
        return drumKitRoot != null && drumKitRoot.activeSelf;
    }
}
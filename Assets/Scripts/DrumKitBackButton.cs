using UnityEngine;

/// <summary>
/// Handles returning from the drum kit back to the main menu.
/// Supports both mouse clicks and HoloLens interaction.
/// </summary>
[RequireComponent(typeof(Collider))]
public class DrumKitBackButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        ReturnToMenu();
    }

    /// <summary>
    /// Called from HoloLens or mouse.
    /// </summary>
    public void ReturnToMenu()
    {
        if (DrumKitManager.Instance != null)
        {
            DrumKitManager.Instance.Hide();
        }
        else
        {
            Debug.LogError("DrumKitManager instance not found.");
        }
    }
}
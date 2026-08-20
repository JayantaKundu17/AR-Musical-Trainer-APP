using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// <summary>
/// Small helper so click/tap detection works no matter which input backend
/// your project uses (Edit → Project Settings → Player → Active Input
/// Handling: Input System Package (New), Input Manager (Old), or Both).
/// This avoids relying on Unity's OnMouseDown message, which silently does
/// nothing if only the new Input System is active.
/// </summary>
public static class PointerInputUtil
{
    /// <summary>
    /// True exactly on the frame the primary pointer button (left mouse
    /// button on desktop) went down. Outputs its screen position.
    /// </summary>
    public static bool PrimaryButtonDownThisFrame(out Vector2 screenPosition)
    {
        screenPosition = default;

#if ENABLE_INPUT_SYSTEM
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            screenPosition = Mouse.current.position.ReadValue();
            return true;
        }
        return false;
#elif ENABLE_LEGACY_INPUT_MANAGER
        if (Input.GetMouseButtonDown(0))
        {
            screenPosition = Input.mousePosition;
            return true;
        }
        return false;
#else
        return false;
#endif
    }
}

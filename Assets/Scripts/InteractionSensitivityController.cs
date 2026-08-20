using UnityEngine;

/// <summary>
/// Applies the "interaction sensitivity" setting to HoloLens hand/gaze input.
/// Works standalone (stores + logs the value), with a commented MRTK
/// integration block you can enable if your project uses the Mixed Reality
/// Toolkit.
/// </summary>
public class InteractionSensitivityController : MonoBehaviour
{
    [Range(0f, 1f)]
    public float CurrentSensitivity = 0.5f;

    [Header("Example mapped range (tune to taste)")]
    [SerializeField] private float minPointerSpeed = 0.5f;
    [SerializeField] private float maxPointerSpeed = 2.5f;

    public float MappedPointerSpeed { get; private set; }

    public void SetSensitivity(float value)
    {
        CurrentSensitivity = Mathf.Clamp01(value);
        MappedPointerSpeed = Mathf.Lerp(minPointerSpeed, maxPointerSpeed, CurrentSensitivity);

        Debug.Log($"[InteractionSensitivity] value={CurrentSensitivity:F2}, mappedPointerSpeed={MappedPointerSpeed:F2}");

        // ---- Optional MRTK integration (requires Mixed Reality Toolkit package) ----
        // foreach (var pointer in Microsoft.MixedReality.Toolkit.Input.PointerUtils
        //              .GetPointers<Microsoft.MixedReality.Toolkit.Input.IMixedRealityPointer>())
        // {
        //     if (pointer is Microsoft.MixedReality.Toolkit.Input.LinePointer linePointer)
        //     {
        //         linePointer.PointerExtent = MappedPointerSpeed;
        //     }
        // }
    }
}

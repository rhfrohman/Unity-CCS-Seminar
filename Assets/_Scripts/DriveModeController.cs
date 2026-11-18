using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum DriveMode { Park, Drive, Sport }

public class DriveModeController : MonoBehaviour
{
    [SerializeField] DriveMode mode = DriveMode.Park;

    [Header("Scene")]
    public Light[] accentLights;
    public Animator animator;

    [Header("Accent colours (one per accent light, repeats if fewer)")]
    public Color[] parkColors = { new Color(0.85f, 0.88f, 0.92f) };
    public Color[] driveColors = { new Color(0f, 0.85f, 1f) };
    public Color[] sportColors = { new Color(1f, 0.12f, 0.05f), new Color(1f, 0.55f, 0.1f) };

    [Header("HMI indicator (optional)")]
    public Graphic[] modeIndicators = new Graphic[3]; // P, D, S
    public Color indicatorActive = new Color(0f, 0.85f, 1f);
    public Color indicatorInactive = new Color(1f, 1f, 1f, 0.25f);
    public Graphic[] modeBars = new Graphic[0]; // P, D, S: shown only on the active mode

    [Tooltip("Animated by the Timeline: 0 = Park, 1 = Drive, 2 = Sport. Applied only when it changes, so keys still work.")]
    public float timelineMode = -1f;
    float lastTimelineMode = -1f;

    DriveMode applied = (DriveMode)(-1);

    public DriveMode Mode
    {
        get => mode;
        set { mode = value; Apply(); }
    }

    void OnEnable() => Apply();

    void Update()
    {
        var kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.digit1Key.wasPressedThisFrame || kb.numpad1Key.wasPressedThisFrame) Mode = DriveMode.Park;
            if (kb.digit2Key.wasPressedThisFrame || kb.numpad2Key.wasPressedThisFrame) Mode = DriveMode.Drive;
            if (kb.digit3Key.wasPressedThisFrame || kb.numpad3Key.wasPressedThisFrame) Mode = DriveMode.Sport;
        }

        int tm = Mathf.RoundToInt(timelineMode);
        if (tm >= 0 && tm != Mathf.RoundToInt(lastTimelineMode)) mode = (DriveMode)Mathf.Min(tm, 2);
        lastTimelineMode = timelineMode;

        if (mode != applied) Apply();
    }

    void OnValidate() { if (isActiveAndEnabled) Apply(); }

    // For UI buttons (e.g. the round controller).
    public void SetPark() => Mode = DriveMode.Park;
    public void SetDrive() => Mode = DriveMode.Drive;
    public void SetSport() => Mode = DriveMode.Sport;

    void Apply()
    {
        if (modeIndicators != null)
            for (int i = 0; i < modeIndicators.Length; i++)
                if (modeIndicators[i]) modeIndicators[i].color = i == (int)mode ? indicatorActive : indicatorInactive;
        if (modeBars != null)
            for (int i = 0; i < modeBars.Length; i++)
                if (modeBars[i]) modeBars[i].enabled = i == (int)mode;

        // Lights keep their authored colours in Edit mode.
        if (!Application.isPlaying) return;

        Color[] colors = mode == DriveMode.Park ? parkColors : mode == DriveMode.Drive ? driveColors : sportColors;
        if (accentLights != null && colors != null && colors.Length > 0)
            for (int i = 0; i < accentLights.Length; i++)
                if (accentLights[i]) accentLights[i].color = colors[i % colors.Length];

        if (animator && animator.runtimeAnimatorController && applied != mode)
            animator.SetTrigger(mode.ToString());

        applied = mode;
    }
}

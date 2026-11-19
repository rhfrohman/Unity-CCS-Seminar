using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteAlways]
public class FuelGauge : MonoBehaviour
{
    public RectTransform fill;          // child of the bar track, stretched by anchorMax.x
    public TMP_Text rangeReadout;
    public Graphic[] tintWhenLow;       // fill, pump icon, etc.
    [Range(0, 1)] public float level = 0.62f;
    public float tankLitres = 60f;
    public float litresPer100km = 8.5f;
    [Range(0, 1)] public float lowThreshold = 0.15f;
    public Color normalColor = new Color32(0x44, 0xD1, 0xFC, 0xFF);
    public Color iconColor = new Color32(0x98, 0x98, 0x98, 0xFF);
    public Color lowColor = new Color32(0xFC, 0x60, 0x44, 0xFF);

    public bool IsLow => level < lowThreshold;
    public int RangeKm => Mathf.RoundToInt(level * tankLitres / litresPer100km * 100f);

    void Update()
    {
        if (fill) fill.anchorMax = new Vector2(Mathf.Clamp01(level), fill.anchorMax.y);
        if (rangeReadout) rangeReadout.text = RangeKm + " km";
        if (tintWhenLow != null)
            for (int i = 0; i < tintWhenLow.Length; i++)
                if (tintWhenLow[i]) tintWhenLow[i].color = IsLow ? lowColor : i == 0 ? normalColor : iconColor;
    }
}

using UnityEngine;
using TMPro;
public class SpeedGauge : MonoBehaviour
{
    public Transform needle;
    public TMP_Text readout;
    public float maxSpeed = 200f;
    public float minAngle = 135f, maxAngle = -135f;
    [Range(0, 200)] public float speed;
    void Update()
    {
        float t = Mathf.Clamp01(speed / maxSpeed);
        needle.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(minAngle, maxAngle, t));
        if (readout) readout.text = Mathf.RoundToInt(speed).ToString();
    }
}

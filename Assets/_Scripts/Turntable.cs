using UnityEngine;

public class Turntable : MonoBehaviour
{
    public float degreesPerSecond = 10f;

    void Update()
    {
        transform.Rotate(0f, degreesPerSecond * Time.deltaTime, 0f, Space.World);
    }
}

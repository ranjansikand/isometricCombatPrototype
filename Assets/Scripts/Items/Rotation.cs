// Rotates the cubes

using UnityEngine;

public class Rotation : MonoBehaviour
{
    void Update() {
        transform.Rotate(Time.deltaTime * 35f, Time.deltaTime * 65f, Time.deltaTime * 35f, Space.Self);
    }
}

// Simple rotation script to spin objects

using System.Collections;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    float originalY;

    [SerializeField] float rotationX = 35f;
    [SerializeField] float rotationY = 65f;
    [SerializeField] float rotationZ = 35f;

    [SerializeField] float floatStrength = 0;

    void Start() {
        Invoke(nameof(SaveYPos), .25f);
    }

    void SaveYPos() {
        this.originalY = this.transform.position.y;
    }

    void Update() {
        // Floating Script
        if (floatStrength > 0) {
            transform.position = new Vector3(transform.position.x,
            originalY + ((float)Mathf.Sin(Time.time) * floatStrength),
            transform.position.z);
        }
        
        // Rotation Script
        transform.Rotate(
            Time.deltaTime * rotationX, 
            Time.deltaTime * rotationY, 
            Time.deltaTime * rotationZ, 
            Space.Self
        );
    }
}

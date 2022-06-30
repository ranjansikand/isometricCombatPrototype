
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] int numberOfItems = 1;

    void Start() {
        for (int i = 0; i < numberOfItems; i++) {
            ItemGenerator.instance.PopOutObject(transform.position);
        }
    }
}


using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] Item weapon;

    void Start() {
        ItemGenerator.instance.SpawnObject(transform.position, weapon);
        ItemGenerator.instance.SpawnGold(transform.position, 10);
    }
}

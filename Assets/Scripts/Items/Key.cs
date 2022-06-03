// Script attached to key prefab
using System.Collections;
using UnityEngine;

public class Key : MonoBehaviour
{
    void Awake() {
        gameObject.layer = 10;
        StartCoroutine(DestroyOnTimeout());
    }

    IEnumerator DestroyOnTimeout() {
        yield return new WaitForSeconds(60);
        Destroy(gameObject);
    }
}


using UnityEngine;
using UnityEngine.UI;

public class KeyCount : MonoBehaviour
{
    [SerializeField] Text _keyCounter;
    int _numberOfKeys;

    void Start() {
        PlayerController.keyCount += UpdateKeyCount;

        UpdateKeyCount();
    }

    void UpdateKeyCount (int count = 0) {
        _numberOfKeys = count;

        _keyCounter.text = "x" + _numberOfKeys.ToString();
    }
}

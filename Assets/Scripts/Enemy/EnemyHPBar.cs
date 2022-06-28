using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour {
    [SerializeField] private Image _bar;
    private int _maxAmount;
    private static GameObject mainCamera;

    public void InitializeBar(int value) {
        _maxAmount = value;
        _bar.fillAmount = 1;
    }

    public void UpdateBar(int value) {
        _bar.fillAmount = 1.0f * value / _maxAmount;
    }

    private void Awake() {
        if (mainCamera == null) mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void LateUpdate() {
        transform.LookAt(transform.position + mainCamera.transform.forward);
    }
}
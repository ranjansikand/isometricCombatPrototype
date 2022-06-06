using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] PlayerHealth _player;

    private int _health;
    private float _lerpTimer;

    [SerializeField] float _chipSpeed = 3f;
    [SerializeField] Image _frontBar, _backBar; 

    void Awake()
    {
        if (_player == null) {
            _player = GameObject.Find("Player").GetComponent<PlayerHealth>();
        }

        PlayerHealth.onHealthUpdate += OnHealthUpdate;
    }

    void Start() 
    {
        _health = _player.Health;
    }

    void OnHealthUpdate() 
    {
        StopCoroutine(UpdateBackBar());
        // Update Frame to reflect max health changes
        _frontBar.fillAmount = 1.0f * _player.Health / _player.MaxHealth;
        
        if (_health != _player.Health) {
            if (_health > _player.Health) StartCoroutine(UpdateBackBar());
            else _backBar.fillAmount = _frontBar.fillAmount;

            _health = _player.Health;
        }
    }

    IEnumerator UpdateBackBar() {
        float startingFill = _backBar.fillAmount;
        float goalFill = _frontBar.fillAmount;
        _lerpTimer = 0;

        while (_backBar.fillAmount > goalFill) {
            // update timing
            _lerpTimer += Time.deltaTime;

            float percentComplete = _lerpTimer / _chipSpeed;
            percentComplete = percentComplete * percentComplete;

            // update bar
            _backBar.fillAmount = Mathf.Lerp(startingFill, goalFill, percentComplete);
            yield return null;
        }
    }
}

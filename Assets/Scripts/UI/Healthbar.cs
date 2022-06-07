using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] PlayerHealth _player;
    [SerializeField] Image _frontBar, _backBar; 
    
    private int _health;

    private float _chipSpeed = 0.1f;
    private WaitForSeconds _startDelay = new WaitForSeconds(1f);


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
        yield return _startDelay;
        
        while (_backBar.fillAmount > _frontBar.fillAmount) {
            _backBar.fillAmount = Mathf.MoveTowards(
                _backBar.fillAmount, 
                _frontBar.fillAmount, 
                _chipSpeed * Time.deltaTime);
            yield return null;
        }
    }
}

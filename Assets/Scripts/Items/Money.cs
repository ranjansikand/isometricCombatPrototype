using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Money : MonoBehaviour
{
    int _goldAmount;

    Transform _destination;
    bool _destroying = false, _hasDestination = false;

    static WaitForSeconds _destroyDelay = new WaitForSeconds(60);
    static WaitForSeconds _moveDelay = new WaitForSeconds(1f);
    static float _speed = 4.0f;


    // Helper functions
    
    public void SetValue(int value) {
        _goldAmount = value;
    }
    IEnumerator DestroyOnTimeout() {
        yield return _destroyDelay;

        if (!_hasDestination) {
            DestroyThis();
        }
    }

    void DestroyThis() {
        Destroy(gameObject);
    }

    IEnumerator GoToPlayer(Transform other) {
        yield return _moveDelay;

        _destination = other;
        _hasDestination = true;
    }


    // Unity Functions

    void Awake()
    {
        StartCoroutine(DestroyOnTimeout());
    }

    private void OnTriggerEnter(Collider other) {
        // Check if player detected
        if (other.gameObject.layer == 11) {
            StartCoroutine(GoToPlayer(other.transform));
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.layer == 11) {
            _destroying = true;
            PlayerInventory.instance.AddGold(_goldAmount);
            Invoke(nameof(DestroyThis), 0.05f);
        }
    }

    void Update() {
        if (_hasDestination && !_destroying) {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                _destination.position, 
                _speed * Time.deltaTime);
        }
    }
}

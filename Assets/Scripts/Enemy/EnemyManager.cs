// Script that regulates enemy attacks
// Prevents enemies from attacking all at once

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    int _enemiesInCombat = 0;

    List<EnemyBase> _enemies = new List<EnemyBase>();

    [SerializeField] List<Transform> _targetPoints;


    void Awake() {
        instance = this;
        StartCoroutine(ManageCombat());

        if (_targetPoints.Count == 0) {
            GameObject[] temp =  GameObject.FindGameObjectsWithTag("Target");
            for (int i = 0; i < temp.Length; i++) {
                _targetPoints.Add(temp[i].transform);
            }
        }
    }

    public Transform GetPoint() {
        Transform buffer = _targetPoints[0];
        _targetPoints.RemoveAt(0);
        _targetPoints.Add(buffer);
        return buffer;
    }

    IEnumerator ManageCombat() {
        while (true) {
            if (_enemiesInCombat > 0) {
                EnemyBase enemy = _enemies[0];
                _enemies.RemoveAt(0);
                _enemies.Add(enemy);
                enemy.LaunchAttack();
            }

            // Decrease time between attacks with larger groups of enemies
            yield return new WaitForSeconds(
                Random.Range(0.5f, 3f) *
                Mathf.Max(0.1f, (25-(1.0f*_enemiesInCombat)) / 25)
            );
        }
    }

    public void AddToEnemyList(EnemyBase enemy) {
        if (!_enemies.Contains(enemy)) {
            _enemies.Add(enemy); 
            _enemiesInCombat++;
        }
    }

    public void RemoveFromCombat(EnemyBase enemy) {
        if (_enemies.Contains(enemy)) {
            _enemies.Remove(enemy); 
            _enemiesInCombat--;
        }
    }
}

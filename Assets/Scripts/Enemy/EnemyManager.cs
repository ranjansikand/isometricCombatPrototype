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

    WaitForSeconds _delay = new WaitForSeconds(1.5f);

    void Awake() {
        instance = this;
        StartCoroutine(ManageCombat());
    }

    IEnumerator ManageCombat() {
        while (true) {
            if (_enemiesInCombat != 0) {
                EnemyBase selectedEnemy = _enemies[Random.Range(0, _enemies.Count)];
                // Try to pick an enemy that isn't moving
                for (int i = 0; i < _enemiesInCombat; i++) {
                    selectedEnemy = _enemies[Random.Range(0, _enemies.Count)];
                    if (!selectedEnemy.Agent.hasPath) break;
                }
                
                selectedEnemy.LaunchAttack();
            }

            // Decrease time between attacks with larger groups of enemies
            yield return new WaitForSeconds(
                Random.Range(0.5f, 3f)*
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

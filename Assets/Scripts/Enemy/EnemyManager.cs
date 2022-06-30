// Script that regulates enemy attacks
// Prevents enemies from attacking all at once

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField] List<Transform> _targetPoints;


    void Awake() {
        instance = this;

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
}

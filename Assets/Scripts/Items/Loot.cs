// Script for physical item prefabs
// All dropped items use this script

using System.Collections;
using UnityEngine;

public class Loot : MonoBehaviour
{
    Item _item;

    public GameObject _selectedHighlight;
    public Item Item { get { return _item; } set { _item = value; }}

    static WaitForSeconds _delay = new WaitForSeconds(60);


// Functioning functions
    void Helper(bool value) {
        switch (value) {
            case (true): StopCoroutine(DestroyOnTimeout()); break;
            case (false): StartCoroutine(DestroyOnTimeout()); break;
        }
        _selectedHighlight.SetActive(value);
    }

    IEnumerator DestroyOnTimeout() {
        yield return _delay;
        Destroy(gameObject);
    }


    // Controller functions
    void Awake() { Helper(false); }

    public Loot SelectObject() {
        Helper(true);
        return this;
    }

    public void DeselectObject() { Helper(false); }
}

// Script to provide dropped loot for enemies

using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    #region singleton
    public static ItemGenerator instance;
    void Awake() {
        if (instance != null) {
            Debug.LogWarning("Multiple inventories found!");
            return;
        }
        instance = this;
    }
    #endregion

    [SerializeField] List<Item> _allItems = new List<Item>();
    [SerializeField] GameObject _itemPrefab;

    #region private functions

    Item GetRandomItem() {
        return _allItems[Random.Range(0, _allItems.Count)];
    }
    #endregion

    // Script called from fallen enemies
    public GameObject SpawnObject (Vector3 location, Item item = null) {
        if (item == null) {
            item = GetRandomItem();
        }
        GameObject newItem = Instantiate(_itemPrefab, location + Vector3.up, Quaternion.identity);
        newItem.GetComponent<Loot>().Item = item;
        return newItem;
    }
}

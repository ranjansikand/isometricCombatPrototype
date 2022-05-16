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
    public void SpawnObject (Vector3 location) {
        GameObject newItem = Instantiate(_itemPrefab, location, Quaternion.identity);
        newItem.GetComponent<ItemScript>().Item = GetRandomItem();
    }
}

using UnityEngine;
using UnityEngine.UI;

public class GoldCount : MonoBehaviour {
    [SerializeField] Text _goldCounter;
    int _goldAmount;

    void Start() {
        PlayerInventory.updateGoldCount += UpdateGoldCount;

        UpdateGoldCount();
    }

    void UpdateGoldCount (int amount = 0) {
        _goldAmount = amount;

        _goldCounter.text = "$" + _goldAmount.ToString();
    }
}
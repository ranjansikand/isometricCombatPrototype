// Script attached to item prefab
// Item assigned by Generator

using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ItemScript : MonoBehaviour
{
    Item item; 
    public Item Item { set { item = value; }}

    void Update() {
        transform.Rotate(Time.deltaTime * 35f, Time.deltaTime * 65f, Time.deltaTime * 35f, Space.Self);
    }

    void OnTriggerEnter(Collider collider) {
        var itemReceiver = collider.GetComponent<IReceivable>();

        if (itemReceiver != null && itemReceiver.AddItem(item)) {
            Destroy(gameObject);
        }
    }
}

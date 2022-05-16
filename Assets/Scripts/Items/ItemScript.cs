// 

using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ItemScript : MonoBehaviour
{
    Item item; 
    public Item Item { set { item = value; }}

    SphereCollider _collider;

    void Awake() {
        _collider = GetComponent<SphereCollider>();
    }

    void Start() {
        _collider.isTrigger = true;
        _collider.radius = 1.0f;
    }

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

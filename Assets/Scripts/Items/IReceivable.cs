// Interface attached to item's beneficiary called by the ItemScript on Trigger
// Used to add items to inventory

public interface IReceivable
{
    void SelectItem (Item newItem = null);
}

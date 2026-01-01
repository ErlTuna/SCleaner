public interface IInventoryAddable
{
    bool CanBeAdded(PlayerInventoryManager inventory);
    void AddToInventory(PlayerInventoryManager inventory);
}


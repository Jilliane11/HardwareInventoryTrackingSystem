namespace HardwareInventoryTrackingSystem.Interfaces;

public interface IPlaceFormAction
{
    IDictionary<int, Models.Item> FindItemsByIds(IEnumerable<int> itemIds);
    List<Models.Item> GetAvailableItems();
    void Add(Models.BorrowingForm newForm);
    Models.Item FindItemById(int itemId);
}
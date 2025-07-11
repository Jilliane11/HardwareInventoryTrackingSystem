namespace HardwareInventoryTrackingSystem.Interfaces;

public class PlaceFormAction : IPlaceFormAction
{
    private readonly AppDbContext _context;

    public PlaceFormAction(AppDbContext context)
    {
        _context = context;
    }

    public IDictionary<int, Models.Item> FindItemsByIds(IEnumerable<int> itemIds)
    {
        return _context.Items.Where(x => itemIds.Contains(x.ItemId))
            // .Include(r => r.QuantityInStock)
            .ToDictionary(key => key.ItemId);


    }
    public List<Models.Item> GetAvailableItems()
    {

        var items = _context.Items.Where(item => item.QuantityInStock > 0).ToList();
        return items ?? new List<Models.Item>();
    }

    public void Add(Models.BorrowingForm newForm)
    {
        _context.Add(newForm);
    }

    public Models.Item FindItemById(int itemId)
    {
        var item = _context.Items
            .Where(i => i.ItemId == itemId)
            .FirstOrDefault();

        return item;
    }
}
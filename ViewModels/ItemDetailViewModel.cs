using HardwareInventoryTrackingSystem.Models;

namespace HardwareInventoryTrackingSystem.ViewModels;

public class ItemDetailViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Specification { get; set; }
    public string Code { get; set; }
    public string ItemType { get; set; }
    public string Status { get; set; }
    public string Image { get; set; }
    public int QuantityInStock { get; set; }

    public ItemDetailViewModel(Item item)
    {
        Id = item.ItemId;
        Name = item.ItemName;
        Specification = item.Specification;
        Code = item.Code;
        ItemType = item.ItemType;
        Status = item.Status;
        QuantityInStock = item.QuantityInStock;
        Image=item.ImageUrl;
    }
}
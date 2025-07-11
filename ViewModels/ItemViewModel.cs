using HardwareInventoryTrackingSystem.Models;

namespace HardwareInventoryTrackingSystem.ViewModels;

public class ItemViewModel
{
    public int Id { get; set; }
    public string ItemCode { get; set; }
    public string Name { get; set; }
    public string Specification { get; set; }
    public int Quantity { get; set; }
    public string Picture { get; set; }
    public ItemViewModel(Item item)
    {
        Id = item.ItemId;
        ItemCode = item.Code;
        Name = item.ItemName;
        Specification = item.Specification;
        Quantity = item.QuantityInStock;
        Picture = item.ImageUrl;
       
    }

    public ItemViewModel()
    {
    }
}
using HardwareInventoryTrackingSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HardwareInventoryTrackingSystem.Commands;
public class ItemCommandBase
{
    [Required, StringLength(100), DisplayName("Component Name")]
    public string Name { get; set; }
    [Required, StringLength(100), DisplayName("Specification")]
    public string Specification { get; set; }
    [Required, StringLength(100), DisplayName("Code")]
    public string Code { get; set; }
    [Required, StringLength(100), DisplayName("Type")]
    public string Type { get; set; }
    [Required, StringLength(100), DisplayName("Status")]
    public string Status { get; set; }
    [Required, DisplayName("Stock")]
    public int Stock { get; set; }
    [DisplayName("Image")]
    public string Image { get; set; }

}
public class CreateItemCommand : ItemCommandBase
{
    public Item ToItem()
    {

        return new Item
        {
            ItemName = Name,
            Specification = Specification,
            Code = Code,
            ItemType = Type,
            Status = Status,
            QuantityInStock = Stock,
            ImageUrl = Image
        };
    }

}

public class UpdateItemCommand:ItemCommandBase
{
    public int Id { get; set; }
    public Item ToItem()
    {

        return new Item
        {
            ItemName = Name,
            Specification = Specification,
            Code = Code,
            ItemType = Type,
            Status = Status,
            QuantityInStock = Stock,
            ImageUrl = Image
        };
    }
    public void UpdateItem(Item item)
    {
        item.ItemName = Name;
        item.Specification = Specification;
        item.Code = Code;
        item.ItemType = Type;
        item.Status = Status;
        item.QuantityInStock = Stock;
        item.ImageUrl = Image;
    }
}





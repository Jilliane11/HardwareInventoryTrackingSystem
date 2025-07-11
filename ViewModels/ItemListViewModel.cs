using HardwareInventoryTrackingSystem.Models;

namespace HardwareInventoryTrackingSystem.ViewModel;

public class ItemListViewModel
{
    public required int Quantity { get; set; }
    public string Remarks { get; set; }
    public DateTime ReturnDate { get; set; }
    public bool Claimed { get; set; }
    public bool Returned { get; set; }
    public string ItemName { get; set; }
    public string Image { get; set; }
    public int ItemId { get; set; }
      
    public int FormId { get; set; }
    public int? AdminReceiverId { get; set; }

    public ItemListViewModel(ItemList il)
    {
        Quantity = il.Quantity;
        Remarks = il.Remarks;
        Image = il.Item.ImageUrl;
        ReturnDate = il.ReturnDate;
        Claimed = il.Claimed;
        ItemName = il.Item.ItemName;
        Returned = il.Returned;
        ItemId = il.ItemId;
        FormId = il.BorrowingFormId;
        AdminReceiverId = il.AdminReceiverId;
    }

    public ItemListViewModel()
    {
            
    }
}
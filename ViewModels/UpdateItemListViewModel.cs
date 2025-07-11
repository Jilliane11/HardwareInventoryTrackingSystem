using HardwareInventoryTrackingSystem.Models;

public class UpdateItemListViewModel
{
    public int ItemListId { get; set; }
    public int FormId { get; set; }
    public int? ReceiverId { get; set; }
    public int Quantity { get; set; }
    public bool Claimed { get; set; }
    public bool Returned { get; set; }

    public UpdateItemListViewModel(ItemList il)
    {
        ItemListId = il.ItemListId;
        FormId = il.BorrowingFormId;
        ReceiverId = il.AdminReceiverId;
        Quantity = il.Quantity;
        Claimed = il.Claimed;
        Returned = il.Returned;
    }

    public UpdateItemListViewModel()
    {

    }
}
namespace HardwareInventoryTrackingSystem.Models
{
    public class ItemList
    {
        public int ItemListId { get; set; }
        public required int Quantity { get; set; }
        public string? Remarks { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool Claimed { get; set; }
        public bool Returned { get; set; }

        public bool IsDeleted { get; set; }
        public Item Item { get; set; }
        public int ItemId { get; set; }
        public BorrowingForm BorrowingForm { get; set; }
        public int BorrowingFormId { get; set; }
        public int? AdminReceiverId { get; set; }
        
    }

}

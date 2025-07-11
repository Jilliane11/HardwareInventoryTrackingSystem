namespace HardwareInventoryTrackingSystem.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Specification { get; set; }
        public string Code { get; set; }
        public string ItemType { get; set; }
        public string Status { get; set; }
        public string ImageUrl { get; set; }
        public int QuantityInStock { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<ItemList> ItemLists { get; set; } = new List<ItemList>();


    }
}
   
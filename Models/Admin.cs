using System.ComponentModel.DataAnnotations;
using HardwareInventoryTrackingSystem.Models;

namespace HardwareInventoryTrackingSystem
{
    public class Admin
    {
        public int AdminId { get; set; }
        public required string UserName { get; set; }
        public required string Name { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ImageUrl { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<BorrowingForm>? BorrowingForms { get; set; }
        public ICollection<ItemList>? ReturnedItems { get; set; }
        
    }
}

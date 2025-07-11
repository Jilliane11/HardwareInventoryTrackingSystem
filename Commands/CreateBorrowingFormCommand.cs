using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HardwareInventoryTrackingSystem.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static System.Net.Mime.MediaTypeNames;

namespace HardwareInventoryTrackingSystem.Commands
{
    public class BorrowingFormCommandBase
    {
        public int FormId { get; set; }
        [Required, StringLength(50), DisplayName("Purpose")]
        public string Purpose { get; set; }
        [Required, StringLength(50), DisplayName("Room No")]
        public string RoomNo { get; set; }
        [Required, StringLength(50), DisplayName("Subject Code")]
        public string Code { get; set; }
        [Required,StringLength(50),DisplayName("Professor In Charge")]
        public string Professor { get; set; }
        [StringLength(50), DisplayName("Remarks")]
        public string? Remarks { get; set; }
        [DisplayName("Time Borrowed")]
        public DateTime TimeBorrowed { get; set; }
        [DisplayName("Date & Time Released")]
        public DateTime DateTimeReleased { get; set; }
        [DisplayName("Expected Return Date")]
        public DateTime ExpectedReturnDate { get; set; }
        [DisplayName("Actual Date Returned")]
        public DateTime ActualReturnDate { get; set; }
    }

    public class CreateBorrowingFormCommand : BorrowingFormCommandBase
    {
        public List<CreateItemListCommand> ItemList { get; set; } = new List<CreateItemListCommand>();

        public BorrowingForm ToBorrowingForm()
        {
            return new BorrowingForm
            {
                BorrowingFormId = FormId,
                Timestamp = DateTime.Now,
                Purpose = Purpose,
                RoomNo = RoomNo,
                Remarks = Remarks,
                SubjectCode = Code,
                DateTimeReleased = DateTimeReleased,
                ExpectedReturnDate = ExpectedReturnDate,
                ActualReturnDate = ActualReturnDate,
                ProfessorInCharge = Professor,
                ItemLists = ItemList.Select(c=>c.ToItemList()).ToList()
            };
        }
    }

    public class ItemListCommandBase
    {
        public int ItemListId { get; set; }
        [Required]
        public int FormId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Range(1, 50)]
        public int Quantity { get; set; }
        public int? AdminReceiverId { get; set; }
        public bool Claim { get; set; }
        public bool Return { get; set; }
    }

    public class UpdateFormItemListCommand:ItemListCommandBase
    {
        public ItemList ToItemList()
        {
            return new ItemList
            {
                ItemListId = ItemListId,
                Quantity = Quantity,
                Claimed = Claim,
                Returned = Return,
                ItemId = ItemId,
                BorrowingFormId = FormId,
                AdminReceiverId = AdminReceiverId
            };
        }
        public void UpdateItemList(ItemList il)
        {
            il.BorrowingFormId = FormId;
            il.AdminReceiverId = AdminReceiverId;
            il.Quantity=Quantity;
            il.Claimed = Claim;
            il.Returned = Return;
            il.ItemId = ItemId;
        }
    }
    
    public class CreateItemListCommand
    {
        [Required]
        public int FormId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Range(1,50)]
        public int Quantity { get; set; }
        [StringLength(50)]
        public string? Remarks { get; set; }
        [Required]
        public DateTime ReturnDate { get; set; }
        public int? AdminReceiverId { get; set; }

        public ItemList ToItemList()
        {
            return new ItemList
            {
                BorrowingFormId = FormId,
                ItemId = ItemId,
                Quantity = Quantity,
                Remarks = Remarks,
                ReturnDate = ReturnDate,
                AdminReceiverId = AdminReceiverId
            };
        }

    }
}

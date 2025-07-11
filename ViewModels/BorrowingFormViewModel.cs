using HardwareInventoryTrackingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HardwareInventoryTrackingSystem.ViewModel
{
    public class BorrowItemsViewModel
    {
        public BorrowingFormViewModel Form { get; set; }  = new BorrowingFormViewModel();
        public List<ItemListViewModel> BorrowedItems { get; set; } = new List<ItemListViewModel>();

        // Parameterless constructor for model binding
        public BorrowItemsViewModel()
        {
            // Initialize properties with default values
            Form = new BorrowingFormViewModel(); // Or set any default values as needed
            BorrowedItems = new List<ItemListViewModel>();
        }

        // Constructor that accepts the parameters
        public BorrowItemsViewModel(BorrowingFormViewModel form, List<ItemListViewModel> borrowedItems)
        {
            Form = form;
            BorrowedItems = borrowedItems;
        }
    }
    public class BorrowingFormViewModel
    {
        public int FormId { get; set; }
        public int AdminId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public DateTime Timestamp { get; set; }
        [Required(ErrorMessage = "Date & Time to be borrowed is required.")]
        [ValidBorrowingTime(ErrorMessage = "Date & Time to be borrowed must be in the future and between 8:00 AM and 5:00 PM.")]
        public DateTime DateToBorrow { get; set; }
        [Required(ErrorMessage = "Purpose is required.")]
        public  string Purpose { get; set; }
        [Required(ErrorMessage = "Professor Name is required.")]
        public string Professor { get; set; }
        [Required(ErrorMessage = "Room No is required.")]
        public string RoomNo { get; set; }
        [Required(ErrorMessage = "Subject Code is required.")]
        public string SubjectCode { get; set; }
        public bool IsApproved { get; set; }
        public List<ItemListViewModel>? BorrowedItems { get; set; } = new List<ItemListViewModel>();

        public BorrowingFormViewModel(BorrowingForm bf)
        {
            Professor = bf.ProfessorInCharge;
            Timestamp = bf.Timestamp;
            Purpose = bf.Purpose;
            RoomNo = bf.RoomNo;
            DateToBorrow = bf.TimeBorrowed;
            SubjectCode = bf.SubjectCode;
            AdminId=bf.AdminId;
            IsApproved =bf.IsApproved;
            StudentName = bf.Student.StudentName;
            StudentId=bf.StudentId;
            FormId = bf.BorrowingFormId;
            BorrowedItems = bf.ItemLists.Select(itemList => new ItemListViewModel
            {
                ItemId = itemList.ItemId,  
                Quantity = itemList.Quantity,
                Remarks = itemList.Remarks,
                ItemName = itemList.Item.ItemName,
                ReturnDate = itemList.ReturnDate,
                Claimed = itemList.Claimed,
                Returned = itemList.Returned,
                FormId = itemList.BorrowingFormId,
                AdminReceiverId = itemList.AdminReceiverId
            }).ToList();
        }

        public BorrowingFormViewModel()
        {
            
        }
    }

    public class ValidBorrowingTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateToBorrow)
            {
                DateTime now = DateTime.Now;

                if (dateToBorrow <= now)
                {
                    return new ValidationResult("The date and time to borrow must be in the future.");
                }

                TimeSpan startTime = new TimeSpan(8, 0, 0);  // 8:00 AM
                TimeSpan endTime = new TimeSpan(17, 0, 0);   // 5:00 PM
                TimeSpan borrowTime = dateToBorrow.TimeOfDay;

                if (borrowTime < startTime || borrowTime > endTime)
                {
                    return new ValidationResult("The borrowing time must be between 8:00 AM and 5:00 PM.");
                }
            }

            // If all checks pass, return Success
            return ValidationResult.Success;
        }
    }
}

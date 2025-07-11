using HardwareInventoryTrackingSystem.ViewModel;
using HardwareInventoryTrackingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

namespace HardwareInventoryTrackingSystem.Pages.AdminView.BorrowingForm
{
    [Authorize(Roles = "Admin")]
    public class AdminFormHistoryViewModel : PageModel
    {
        private readonly AppDbContext _context;

        public AdminFormHistoryViewModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<HistoryAdminViewModel> History { get; set; }
        public void OnGet()
        {
            LoadCompletedForms();
        }

        private void LoadCompletedForms()
        {
            var borrowingForms = _context.BorrowingForms
                .Where(bf => bf.IsApproved && bf.IsComplete)
                .Include(bf => bf.Student)
                .Include(bf => bf.ItemLists)
                .ThenInclude(il => il.Item)
                .ToList();

            History = borrowingForms
                .Select(bf => new HistoryAdminViewModel(bf)
                {
                    BorrowedItems = bf.ItemLists
                        .Where(il => il.Claimed)
                        .Select(il => new ItemListViewModel
                        {
                            ItemId = il.ItemId,
                            Quantity = il.Quantity,
                            Remarks = il.Remarks,
                            ItemName = il.Item.ItemName,
                            ReturnDate = il.ReturnDate,
                            Claimed = il.Claimed,
                            FormId = il.BorrowingFormId,
                            AdminReceiverId = il.AdminReceiverId
                        })
                        .ToList(),
                })
                .ToList();
        }
        
    }

    public class HistoryAdminViewModel
    {
        public int FormId { get; set; }
        public int AdminId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime DateToBorrow { get; set; }
        public string Purpose { get; set; }
        public string Professor { get; set; }
        public string RoomNo { get; set; }
        public DateTime ReturnedDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public bool IsComplete { get; set; }
        public string SubjectCode { get; set; }
        public List<ItemListViewModel>? BorrowedItems { get; set; } = new List<ItemListViewModel>();

        public HistoryAdminViewModel(Models.BorrowingForm bf)
        {
            Professor = bf.ProfessorInCharge;
            Timestamp = bf.Timestamp;
            Purpose = bf.Purpose;
            ExpectedReturnDate = bf.ExpectedReturnDate;
            ReturnedDate = bf.ActualReturnDate;
            RoomNo = bf.RoomNo;
            IsComplete = bf.IsComplete;
            DateToBorrow = bf.TimeBorrowed;
            SubjectCode = bf.SubjectCode;
            AdminId = bf.AdminId;
            StudentName = bf.Student.StudentName;
            StudentId = bf.StudentId;
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

        public HistoryAdminViewModel()
        {

        }
    }
}

using HardwareInventoryTrackingSystem.Models;
using HardwareInventoryTrackingSystem.ViewModel;
using HardwareInventoryTrackingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HardwareInventoryTrackingSystem.Pages.AdminView.BorrowingForm
{
    [Authorize(Roles = "Admin")]
    public class BorrowerInformationModel : PageModel
    {
        private readonly AppDbContext _context;

        public BorrowerInformationModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty] public StudentViewModel Borrower { get; set; }

        private string GetItemNameById(int itemId)
        {
            var item = _context.Items.AsNoTracking().FirstOrDefault(i => i.ItemId == itemId);
            return item?.ItemName;
        }

        private string GetImageById(int itemId)
        {
            var item = _context.Items.AsNoTracking().FirstOrDefault(i => i.ItemId == itemId);
            return item?.ImageUrl;
        }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            var student = await _context.Students
                .Include(s => s.BorrowingForms)
                .ThenInclude(bf => bf.ItemLists)
                .FirstOrDefaultAsync(s => s.StudentId == id && !s.IsDeleted);

            if (student == null)
            {
                return NotFound();
            }

            Borrower = new StudentViewModel(
                student,
                student.BorrowingForms?.Select(bf => new BorrowingFormViewModel
                {
                    FormId = bf.BorrowingFormId,
                    DateToBorrow = bf.TimeBorrowed,
                    Purpose = bf.Purpose,
                    RoomNo = bf.RoomNo,
                    IsApproved = bf.IsApproved,
                    SubjectCode = bf.SubjectCode,
                    Professor = bf.ProfessorInCharge,
                    BorrowedItems = bf.ItemLists.Select(itemList => new ItemListViewModel
                    {
                        ItemId = itemList.ItemId,
                        ItemName = GetItemNameById(itemList.ItemId),
                        Image = GetImageById(itemList.ItemId),
                        Quantity = itemList.Quantity,
                        Remarks = itemList.Remarks,
                        ReturnDate = itemList.ReturnDate,
                        Claimed = itemList.Claimed,
                        Returned = itemList.Returned,
                        FormId = itemList.BorrowingFormId,
                        AdminReceiverId = itemList.AdminReceiverId
                    }).ToList()
                }).ToList()
            );

            return Page();
        }

        public IActionResult OnPostClaimAndApprove(int[] selectedItems, int formId)
        {
            var form = _context.BorrowingForms
                .Include(f => f.ItemLists)
                .FirstOrDefault(f => f.BorrowingFormId == formId);

            if (form != null)
            {
                if (selectedItems != null && selectedItems.Length > 0)
                {
                    foreach (var itemId in selectedItems)
                    {
                        var itemList = _context.ItemLists
                            .FirstOrDefault(bi => bi.ItemId == itemId && bi.BorrowingFormId == formId);

                        if (itemList != null)
                        {
                            var item = _context.Items.FirstOrDefault(i => i.ItemId == itemList.ItemId);
                            if (item != null)
                            {
                                // Decrease the stock
                                if (item.QuantityInStock >= itemList.Quantity)
                                {
                                    item.QuantityInStock -= itemList.Quantity;
                                }
                                else
                                {
                                    // Handle insufficient stock
                                    ModelState.AddModelError("", $"Insufficient stock for item {item.ItemName}");
                                    return Page();
                                }
                            }

                            // Mark the item as claimed
                            itemList.Claimed = true;
                        }
                    }
                }

                // Mark the form as approved
                form.IsApproved = true;

                _context.SaveChanges();
            }

            return RedirectToPage();
        }

    }
}
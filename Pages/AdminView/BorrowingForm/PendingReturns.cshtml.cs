using HardwareInventoryTrackingSystem.ViewModel;
using HardwareInventoryTrackingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using HardwareInventoryTrackingSystem.Models;

namespace HardwareInventoryTrackingSystem.Pages.AdminView.BorrowingForm
{
    [Authorize(Roles = "Admin")]
    public class PendingReturnsModel : PageModel
    {
        private readonly AppDbContext _context;

        public PendingReturnsModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<PendingReturnsViewModel> PendingReturns { get; set; }

        [BindProperty]
        public PendingReturnsViewModel? SelectedPendingReturn { get; set; }

        public void OnGet()
        {
            LoadPendingReturns();
        }

        public IActionResult OnPostSelectForm(int formId)
        {
            LoadPendingReturns();
            SelectedPendingReturn = PendingReturns.FirstOrDefault(pr => pr.FormId == formId);
            return Page();
        }

        private void LoadPendingReturns()
        {
            PendingReturns = _context.BorrowingForms
                .Where(bf => bf.IsApproved && !bf.IsComplete)
                .Include(bf => bf.Student)
                .Include(bf => bf.ItemLists)
                .ThenInclude(il => il.Item)
                .ToList()
                .Select(bf => new PendingReturnsViewModel(bf)
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
                            Returned = il.Returned, // Ensure Returned status is fetched
                            FormId = il.BorrowingFormId,
                            AdminReceiverId = il.AdminReceiverId
                        })
                        .ToList(),
                })
                .ToList();
        }

        public IActionResult OnPostCompleteForm(int formId)
        {
            Console.WriteLine($"Attempting to complete form with ID: {formId}");

            var form = _context.BorrowingForms
                .Include(bf => bf.ItemLists)
                .FirstOrDefault(bf => bf.BorrowingFormId == formId);

            if (form != null && CanBeCompleted(formId))
            {
                form.IsComplete = true;
                form.ActualReturnDate = DateTime.Now;
                _context.SaveChanges();
                Console.WriteLine($"Form {formId} marked as complete.");
                TempData["SuccessMessage"] = $"Form {formId} completed successfully.";
            }
            else
            {
                Console.WriteLine($"Form {formId} cannot be completed or does not exist.");
                TempData["ErrorMessage"] = $"Form {formId} cannot be completed or does not exist.";
            }

            LoadPendingReturns();
            return RedirectToPage();
        }

        public bool CanBeCompleted(int formId)
        {
            var form = _context.BorrowingForms
                .Include(bf => bf.ItemLists)
                .FirstOrDefault(bf => bf.BorrowingFormId == formId);

            if (form == null) return false;

            return form.ItemLists.All(il => !il.Claimed || (il.Claimed && il.Returned));
        }
        public IActionResult OnPostToggleReturnStatus(int itemId, int formId)
        {
            var itemList = _context.ItemLists
                .Include(il => il.Item)
                .FirstOrDefault(il => il.ItemId == itemId && il.BorrowingFormId == formId);

            if (itemList != null && itemList.Claimed && !itemList.Returned)
            {
             
                itemList.Returned = true;
                itemList.ReturnDate = DateTime.Now;
               
                var item = itemList.Item;
                if (item != null)
                {
                    item.QuantityInStock+= itemList.Quantity; 
                }

                _context.SaveChanges();
                TempData["SuccessMessage"] = $"Item {item.ItemName} (ID: {itemId}) marked as returned and stock updated.";
            }
            else
            {
                TempData["ErrorMessage"] = $"Error toggling return status for item ID {itemId}.";
                return NotFound();
            }

            LoadPendingReturns();
            SelectedPendingReturn = PendingReturns.FirstOrDefault(pr => pr.FormId == formId);
            return Page();
        }

    }
}
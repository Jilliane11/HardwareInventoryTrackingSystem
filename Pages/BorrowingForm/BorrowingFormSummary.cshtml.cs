using HardwareInventoryTrackingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HardwareInventoryTrackingSystem.Pages.BorrowingForm
{
    [Authorize(Roles = "Student,Admin")]
    public class BorrowingFormSummaryModel : PageModel
    {
        private readonly AppDbContext _context;

        public BorrowingFormSummaryModel(AppDbContext context)
        {
            _context = context;
        }
        public Models.BorrowingForm BorrowingForm { get; set; }
        public List<ItemList> ItemList { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            BorrowingForm = await _context.BorrowingForms
                .Include(b => b.ItemLists)
                .ThenInclude(i => i.Item)
                .FirstOrDefaultAsync(b => b.BorrowingFormId == id);
            if (BorrowingForm == null)
            {
                return NotFound();
            }

            ItemList = BorrowingForm.ItemLists.ToList();

            return Page();
        }



    }
}

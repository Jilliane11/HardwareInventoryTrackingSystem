using HardwareInventoryTrackingSystem.ViewModel;
using HardwareInventoryTrackingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HardwareInventoryTrackingSystem.Pages.AdminView.BorrowingForm
{
    [Authorize(Roles = "Admin")]
    public class BorrowingFormDetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        public BorrowingFormDetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public BorrowingFormViewModel BorrowingForm { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var borrowingForm = await _context.BorrowingForms
                .Include(c => c.Student)
                .Include(bf => bf.ItemLists)
                .ThenInclude(il => il.Item)
                .FirstOrDefaultAsync(bf => bf.BorrowingFormId == id);

            if (borrowingForm == null)
            {
                return NotFound();
            }
            BorrowingForm = new BorrowingFormViewModel(borrowingForm);
            return Page();
        }
    }


}
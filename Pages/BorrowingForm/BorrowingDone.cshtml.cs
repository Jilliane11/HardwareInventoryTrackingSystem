using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HardwareInventoryTrackingSystem.Pages.BorrowingForm
{
    [Authorize(Roles = "Student,Admin")]
    public class BorrowingDoneModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}

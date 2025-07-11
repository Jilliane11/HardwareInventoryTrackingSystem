using HardwareInventoryTrackingSystem.Commands;
using HardwareInventoryTrackingSystem.Models;
using HardwareInventoryTrackingSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace HardwareInventoryTrackingSystem.Pages.BorrowingForm
{
    [Authorize(Roles = "Student,Admin")]
    public class BorrowingFormModel : PageModel
    {
        private readonly AppDbContext _context;
        public DropDownOptions Options { get; set; }

        public BorrowingFormModel(AppDbContext context)
        {
            _context = context;
            Options = new DropDownOptions();
        }
        
        [BindProperty] public BorrowingFormViewModel Input { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //var adminId = HttpContext.Session.GetInt32("UserId");
            var studentIdClaim = User.FindFirstValue("UserId");
            var adminIdClaim = User.FindFirstValue("UserId");
            var studentId = int.Parse(studentIdClaim);
            var adminId = int.Parse(adminIdClaim);
            // Input.AdminId = adminId.Value;
            var form = new Models.BorrowingForm
            {
                Timestamp = DateTime.Today.Date,
                TimeBorrowed = Input.DateToBorrow,
                Purpose = Input.Purpose,
                RoomNo = Input.RoomNo,
                SubjectCode = Input.SubjectCode,
                ProfessorInCharge = Input.Professor,
                StudentId = studentId,
                AdminId = 1,
            };
            
            try
            {
                var borrowedDate = form.TimeBorrowed;
                form.ExpectedReturnDate = new DateTime(
                borrowedDate.Year,
                borrowedDate.Month,
                borrowedDate.Day,
                17, // Hour: 5 PM
                0, // Minute: 0
                0);// Second: 0

                _context.BorrowingForms.AddAsync(form);
                await _context.SaveChangesAsync();
                
                return RedirectToPage("/BorrowingForm/BFWithItemList", new { id = form.BorrowingFormId });

            }
            catch (DbUpdateException dbEx)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return Page();
            }
        }
    }

    public class DropDownOptions
    {
        public ICollection<SelectListItem> Purposes = new List<SelectListItem>
        {
            new SelectListItem{Value = "Testing", Text="Testing"},
            new SelectListItem{Value = "Experimentation", Text="Experimentation"},
            new SelectListItem{Value = "Research", Text="Research"},
            new SelectListItem{Value = "Other", Text="Other"}
        };
        public ICollection<SelectListItem> Subjects = new List<SelectListItem>
        {
            new SelectListItem{Value = "CPE 123", Text="CPE 123"},
            new SelectListItem{Value = "CPE 234", Text="CPE 234"},
            new SelectListItem{Value = "CPE 345", Text="CPE 345"},
            new SelectListItem{Value = "ECE 123", Text="ECE 123"},
        };
        public ICollection<SelectListItem> Professors = new List<SelectListItem>
        {
            new SelectListItem{Value = "Dr. Eleonor V. Palconit", Text="Dr. Eleonor V. Palconit"},
            new SelectListItem{Value = "Sir Dexielito Badilles", Text="Sir Dexielito Badilles"},
            new SelectListItem{Value = "Sir Pidor", Text="Sir Pidor"},
            new SelectListItem{Value = "Sir Jason", Text="Sir Jason"},
        };
        public ICollection<SelectListItem> Rooms = new List<SelectListItem>
        {
            new SelectListItem{Value = "F313A", Text="F313A"},
            new SelectListItem{Value = "F313B", Text="F313B"},
            new SelectListItem{Value = "F313C", Text="F313C"},
            new SelectListItem{Value = "F313D", Text="F313D"},
        };
        public ICollection<SelectListItem> Courses = new List<SelectListItem>
        {
            new SelectListItem{Value = "Aerospace Engineering", Text="Aerospace Engineering"},
            new SelectListItem{Value = "Civil Engineering", Text="Civil Engineering"},
            new SelectListItem{Value = "Chemical Engineering", Text="Chemical Engineering"},
            new SelectListItem{Value = "Computer Engineering", Text="Computer Engineering"},
            new SelectListItem{Value = "Electronics Engineering", Text="Electronics Engineering"},
            new SelectListItem{Value = "Electrical Engineering", Text="Electrical Engineering"},
            new SelectListItem{Value = "Industrial Engineering", Text="Industrial Engineering"},
            new SelectListItem{Value = "Mechanical Engineering", Text="Mechanical Engineering"},
            new SelectListItem{Value = "Robotics Engineering", Text="Robotics Engineering"},
            new SelectListItem{Value = "Other", Text="Other"}
        };
    }


}

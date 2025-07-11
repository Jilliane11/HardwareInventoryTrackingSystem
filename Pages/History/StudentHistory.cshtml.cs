using HardwareInventoryTrackingSystem.Models;
using HardwareInventoryTrackingSystem.ViewModel;
using HardwareInventoryTrackingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HardwareInventoryTrackingSystem.Pages.History
{
    public class StudentHistoryModel : PageModel
    {
        private readonly AppDbContext _context;
        public StudentHistoryModel(AppDbContext context)
        {
            _context = context;
        }

        public StudentViewModel StudentHistory{ get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var studentIdClaim = User.FindFirstValue("UserId"); 

            if (int.TryParse(studentIdClaim, out int studentId))
            {
                var student = await _context.Students
                    .Include(c => c.BorrowingForms)
                    .ThenInclude(c => c.ItemLists)
                    .ThenInclude(c=>c.Item)
                    .FirstOrDefaultAsync(s => s.StudentId == studentId);

                if (student != null)
                {
                    StudentHistory = new StudentViewModel(student, student.BorrowingForms.Select(b => new BorrowingFormViewModel(b)).ToList());
                }
            }
            else
            {
                return NotFound(); // Or handle the error as appropriate
            }

            return Page();
        }
    }
    }
    


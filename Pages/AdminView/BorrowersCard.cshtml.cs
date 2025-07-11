using System.Runtime.CompilerServices;
using HardwareInventoryTrackingSystem.Models;
using HardwareInventoryTrackingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HardwareInventoryTrackingSystem.Pages.AdminView
{
    [Authorize(Roles = "Admin")]
    public class BorrowersCardModel : PageModel
    {
        private readonly AppDbContext _context;

        public BorrowersCardModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty] public List<BorrowerViewModel> Borrowers { get; set; }
        [BindProperty] public List<BorrowedComponentsViewModel> Components { get; set; }
        [BindProperty] public List<NotificationsViewModel> TodayNotifications { get; set; }
        [BindProperty] public List<NotificationsViewModel> UpcomingNotifications { get; set; }

        public async Task OnGet()
        {
            Borrowers = await _context.Students
                .Include(s => s.BorrowingForms)
                .Where(s => !s.IsDeleted && s.BorrowingForms.Any(bf => !bf.IsApproved))
                .Select(student => new BorrowerViewModel(student))
                .ToListAsync();
            Components = await _context.ItemLists
                .Include(s => s.BorrowingForm)
                .Include(s => s.Item)
               .Where(s => s.Claimed)
                .Where(s => !s.Returned)
                .Select(s => new BorrowedComponentsViewModel(s))
                .ToListAsync();
            // Fetch Today's Notifications
            TodayNotifications = await _context.BorrowingForms
                .Where(bf => bf.TimeBorrowed.Date == DateTime.Today)
                .Where(bf => !bf.IsApproved)
                .Select(bf => new NotificationsViewModel(bf))
                .ToListAsync();

            // Fetch Upcoming Notifications
            UpcomingNotifications = await _context.BorrowingForms
                .Where(bf => bf.TimeBorrowed.Date > DateTime.Today)
                .OrderBy(bf => bf.TimeBorrowed) // Sort by date
                .Where(bf => !bf.IsApproved)
                .Select(bf => new NotificationsViewModel(bf))
                .ToListAsync();



        }
    }

    public class BorrowedComponentsViewModel
    {
        public string Particular { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        public DateTime TimeBorrowed { get; set; }

        public BorrowedComponentsViewModel(ItemList il)
        {
            Particular = il.Item.ItemName;
            Quantity = il.Quantity;
            Image = il.Item.ImageUrl;
            TimeBorrowed = il.BorrowingForm.TimeBorrowed;
        }
    }

    public class BorrowerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Course { get; set; }
        public string Image { get; set; }

        public BorrowerViewModel(Student student)
        {
            Id = student.StudentId;
            Name = student.StudentName;
            Course = student.Course;
            Image = student.ImageUrl;
        }
    }
}

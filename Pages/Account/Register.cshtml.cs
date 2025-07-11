using HardwareInventoryTrackingSystem.Account;
using HardwareInventoryTrackingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using HardwareInventoryTrackingSystem.Pages.BorrowingForm;
using static System.Net.Mime.MediaTypeNames;

namespace HardwareInventoryTrackingSystem.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly StudentRegistrationService _studentRegistration;
        public DropDownOptions Options { get; set; }
        [BindProperty]
        public StudentRegistrationInput Input { get; set; }

        public RegisterModel(StudentRegistrationService studentRegistration)
        {
            _studentRegistration = studentRegistration; 
            Options = new DropDownOptions();
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check if the username or email already exists using the service
            if (await _studentRegistration.CheckIfUserExistsAsync(Input.UserName, Input.Email))
            {
                ModelState.AddModelError(string.Empty, "Username or Email already taken.");
                return Page();
            }

            // Register the student using the service
            await _studentRegistration.RegisterStudentAsync(Input);

            // Optionally, add a success message or redirect
            return RedirectToPage("/Account/Login"); // Redirect to login page
        }
    }

    public class StudentRegistrationService
    {
        private readonly AppDbContext _context;

        public StudentRegistrationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckIfUserExistsAsync(string userName, string email)
        {
            return await _context.Students.AnyAsync(s => s.UserName == userName || s.Email == email);
        }

        public async Task RegisterStudentAsync(StudentRegistrationInput input)
        {
            var student = new Student
            {
                UserName = input.UserName,
                Password = HashGenerator.GenerateSHA256Hash(input.Password), 
                StudentName = input.StudentName,
                Email = input.Email,
                Course = input.Course,
                LastEnrolled = input.LastEnrolled,
                Semester = input.Semester,
                Birthdate = input.Birthdate,
                IsDeleted = false,


            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            var userRole = new UserRole
            {
                UserId = student.StudentId, // Assuming you have a primary key StudentId
                Role = "Student"
            };

            // Add the role to the UserRoles table (assuming such a table exists)
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
        }
    }

    public class CourseOptions
    {
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
    public class StudentRegistrationInput
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Student Name")]
        public string StudentName { get; set; }

        [Required]
        [Display(Name = "Course")]
        public string Course { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        public int LastEnrolled { get; set; }
        public int Semester { get; set; }
        public DateOnly Birthdate { get; set; }

    }
}

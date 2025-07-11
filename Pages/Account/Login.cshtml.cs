using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HardwareInventoryTrackingSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HardwareInventoryTrackingSystem.Account
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _context;
        public LoginModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Credential Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var hashedPassword = HashGenerator.GenerateSHA256Hash(Input.Password);

            // Check for student and admin
            var student = await _context.Students
                .FirstOrDefaultAsync(c => c.UserName == Input.UserName && c.Password == hashedPassword);

            var admin = await _context.Admins
                .FirstOrDefaultAsync(c => c.UserName == Input.UserName && c.Password == hashedPassword);

            if (student == null && admin == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return Page();
            }

            if (student != null)
            {
                // Always redirect students to the Index page
                await SignInUser(student.StudentId, student.UserName, "Student");
                return RedirectToPage("/BorrowingForm/BorrowingForm");
            }

            if (admin != null)
            {
                // Redirect admins to the admin-specific page
                await SignInUser(admin.AdminId, admin.UserName, "Admin");
                return RedirectToPage("/AdminView/BorrowersCard");
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return Page();
        }

        private async Task SignInUser(int userId, string username, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim("UserId", userId.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true // Remember the user
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }
    }

    public class Credential
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class HashGenerator
    {
        public static string GenerateSHA256Hash(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input), "Input cannot be null or empty.");
            }

            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
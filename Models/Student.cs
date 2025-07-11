using System.ComponentModel.DataAnnotations;

namespace HardwareInventoryTrackingSystem.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string Course { get; set; }
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[\w\.-]+@addu\.edu\.ph$", ErrorMessage = "Email must end with @addu.edu.ph")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string UserName { get; set; }
        public string? ImageUrl {get; set; }
        public int LastEnrolled { get; set; }
        public int Semester { get; set; }
        public DateOnly Birthdate { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<BorrowingForm>? BorrowingForms { get; set; }
        

    }
}

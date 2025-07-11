using HardwareInventoryTrackingSystem.Models;
using HardwareInventoryTrackingSystem.ViewModel;

namespace HardwareInventoryTrackingSystem.ViewModels;

public class StudentViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Course { get; set; }
    public DateOnly Birthdate { get; set; }
    public int SchoolYear { get; set; }
    public int Semester { get; set; }
    public string Image { get; set; }
    public List<BorrowingFormViewModel> BorrowingForms { get; set; } = new List<BorrowingFormViewModel>();

    public StudentViewModel()
    {
        
    }
    public StudentViewModel(Student student, List<BorrowingFormViewModel> borrowingForms)
    {
        Id = student.StudentId;
        Name = student.StudentName;
        Course = student.Course;
        SchoolYear = student.LastEnrolled;
        Semester = student.Semester;
        Birthdate = student.Birthdate;
        Image = student.ImageUrl;
        BorrowingForms = borrowingForms;
    }
}
using HardwareInventoryTrackingSystem.Models;

namespace HardwareInventoryTrackingSystem.ViewModels;

public class StudentDetailViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Course { get; set; }
    public string Email { get; set; }
    public int Semester { get; set; }
    public string Image { get; set; }
    public int LastEnrolled { get; set; }

    public StudentDetailViewModel(Student student)
    {
        Id = student.StudentId;
        Semester =student.Semester;
        Name = student.StudentName;
        Course = student.Course;
        Email = student.Email;
        Image = student.ImageUrl;
        LastEnrolled = student.LastEnrolled;
    }
}
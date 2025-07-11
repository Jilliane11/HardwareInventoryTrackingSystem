using HardwareInventoryTrackingSystem.Models;
using HardwareInventoryTrackingSystem.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HardwareInventoryTrackingSystem.Commands;
public class StudentCommandBase
{
    [Required, StringLength(100), DisplayName("Name")]
    public string Name { get; set; }
    [Required, StringLength(100), DisplayName("Course")]
    public string Course { get; set; }
    [StringLength(100), DisplayName("Email")]
    public string Email { get; set; }
    public string Image { get; set; }
}

public class UpdateStudentCommand : StudentCommandBase
{

    public Student ToStudent()
    {
        return new Student
        {
            StudentName = Name,
            Course = Course,
            Email = Email,
            ImageUrl = Image,
        };
    }

}
public class CreateStudentCommand : StudentCommandBase
{
    public Student ToStudent()
    {
        return new Student
        {
            StudentName = Name,
            Course = Course,
            Email = Email,
            ImageUrl = Image,
        };
    }

}
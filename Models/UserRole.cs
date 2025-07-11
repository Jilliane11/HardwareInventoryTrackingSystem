namespace HardwareInventoryTrackingSystem.Models;

public class UserRole
{
    public int UserRoleId { get; set; }  // Primary Key
    public int UserId { get; set; }      // Foreign Key to Users table (e.g., Students, Admins)
    public string Role { get; set; }     // Role name (e.g., "Student", "Admin")
}
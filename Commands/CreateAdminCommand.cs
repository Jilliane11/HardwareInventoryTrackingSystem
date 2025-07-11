using HardwareInventoryTrackingSystem.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HardwareInventoryTrackingSystem.Commands;
public class AdminCommandBase
{
    [Required, StringLength(100), DisplayName("Name")]
    public string Name { get; set; }
    [Required, StringLength(100), DisplayName("Username")]
    public string Username { get; set; }
    [Required, StringLength(100), DisplayName("Position")]
    public string Position { get; set; }
    [Required, StringLength(100), DisplayName("Password")]
    public string Password { get; set; }

}
public class UpdateAdminCommand : AdminCommandBase
{
    public Admin ToAdmin()
    {
        return new Admin
        {
            Name = Name,
            UserName = Username,

        };
    }
}


public class CreateAdminCommand : AdminCommandBase
{
    public Admin ToAdmin()
    {
        return new Admin
        {
            Name = Name,
            UserName = Username,
            Password = Password

        };
    }

}
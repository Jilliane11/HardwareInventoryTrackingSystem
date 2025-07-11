namespace HardwareInventoryTrackingSystem.ViewModels;

public class AdminDetailViewModel
{
    public string Name { get; set; }
    public string UserName { get; set; }

    public AdminDetailViewModel(Admin admin)
    {
        Name = admin.Name;
        UserName = admin.UserName;
    }
}
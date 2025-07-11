using HardwareInventoryTrackingSystem;

public class AdminViewModel
{
        public string Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }

    public AdminViewModel(Admin admin)
    {
            Id = admin.Name;
            Name = admin.Name;
            Picture = admin.ImageUrl;
    }
}
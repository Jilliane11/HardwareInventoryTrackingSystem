namespace HardwareInventoryTrackingSystem.ViewModels;

public class NotificationsViewModel
{
    public int FormId { get; set; }
    public DateTime DateToBorrow { get; set; }
    public string Message { get; set; }
    public NotificationsViewModel(Models.BorrowingForm bf)
    {
        FormId = bf.BorrowingFormId;
        DateToBorrow = bf.TimeBorrowed;
        Message = $"Scheduled borrowing on {DateToBorrow:MMMM dd, yyyy hh:mm tt}";
    }
}
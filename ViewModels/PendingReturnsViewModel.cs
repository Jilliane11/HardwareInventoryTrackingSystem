using HardwareInventoryTrackingSystem.ViewModel;

namespace HardwareInventoryTrackingSystem.ViewModels;

public class PendingReturnsViewModel
{
    public int FormId { get; set; }
    public int AdminId { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; }
    public DateTime Timestamp { get; set; }
    public DateTime DateToBorrow { get; set; }
    //datetoborrow plus until the end sa day
    public DateTime ExpectedReturnDate { get; set; }
    //kung kanus a jud nabalik - input
    public DateTime ActualReturnDateTime { get; set; }
    public bool IsApproved { get; set; }
    public bool IsComplete { get; set; }
    public List<ItemListViewModel>? BorrowedItems { get; set; } = new List<ItemListViewModel>();

    public PendingReturnsViewModel()
    {

    }
    public PendingReturnsViewModel(Models.BorrowingForm bf)
    {
        Timestamp = bf.Timestamp;
        DateToBorrow = bf.TimeBorrowed;
        ExpectedReturnDate = bf.ExpectedReturnDate;
        ActualReturnDateTime = bf.ActualReturnDate;
        AdminId = bf.AdminId;
        IsComplete = bf.IsComplete;
        IsApproved = bf.IsApproved;
        StudentName = bf.Student.StudentName;
        StudentId = bf.StudentId;
        FormId = bf.BorrowingFormId;
        BorrowedItems = bf.ItemLists.Select(itemList => new ItemListViewModel
        {
            ItemId = itemList.ItemId,
            Quantity = itemList.Quantity,
            Remarks = itemList.Remarks,
            ItemName = itemList.Item.ItemName,
            ReturnDate = itemList.ReturnDate,
            Claimed = itemList.Claimed,
            FormId = itemList.BorrowingFormId,
            AdminReceiverId = itemList.AdminReceiverId
        }).ToList();
    }
}
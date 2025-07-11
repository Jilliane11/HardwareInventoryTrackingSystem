using HardwareInventoryTrackingSystem.Models;
using HardwareInventoryTrackingSystem.ViewModel;

namespace HardwareInventoryTrackingSystem.ViewModels
{
    public class BorrowerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Course { get; set; }
        public string Image { get; set; }
        public List<BorrowingFormViewModel> BorrowingForms { get; set; }

        public BorrowerViewModel(Student student)
        {
            Id = student.StudentId;
            Name = student.StudentName;
            Course = student.Course;
            Image = student.ImageUrl;
            BorrowingForms = student.BorrowingForms
                .Where(bf => !bf.IsDeleted)
                .Select(bf => new BorrowingFormViewModel(bf))
                .ToList();
        }
    }

    public class BorrowedItemViewModel
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
    }

    public class BorrowerInformationViewModel
    {
        public int BorrowingFormId { get; set; }
        public string StudentName { get; set; }
        public List<BorrowedItemViewModel> BorrowedItems { get; set; }

        public BorrowerInformationViewModel(int borrowingFormId, string studentName, List<BorrowedItemViewModel> borrowedItems)
        {
            BorrowingFormId = borrowingFormId;
            StudentName = studentName;
            BorrowedItems = borrowedItems;
        }
    }
}

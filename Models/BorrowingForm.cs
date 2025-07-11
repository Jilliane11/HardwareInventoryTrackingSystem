using HardwareInventoryTrackingSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace HardwareInventoryTrackingSystem.Models
{
    public class BorrowingForm
    {
        public int BorrowingFormId { get; set; }
        //kung kanus a nag request (DateTime.Now perme)
        public DateTime Timestamp { get; set; }
        public required string Purpose { get; set; }
        public required string RoomNo { get; set; }
        public required string SubjectCode { get; set; }
        public required string ProfessorInCharge { get; set; }
        public string? Remarks { get; set; }
        //nabalik na tanan
        public bool IsComplete { get; set; }
        //approved - pede  an kuhaon
        public bool IsApproved { get; set; }
        public DateTime DateTimeReleased { get; set; }
        //si ya jomax ra maka edit
        //kung kanus a gikuha 
        [TimeBetween(8, 0, 17, 0, ErrorMessage = "TimeBorrowed must be between 8:00 AM and 5:00 PM.")]
        public DateTime TimeBorrowed { get; set; }
        //kung kanusa dapat mabalik
        public DateTime ExpectedReturnDate { get; set; }
        //si ya jomax ray maka edit
        //kung kanus a nabalik jud
        public DateTime ActualReturnDate { get; set; }

        public bool IsDeleted { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int AdminId { get; set; }
        public Admin Admin { get; set; }
        public List<ItemList> ItemLists { get; set; } = new List<ItemList>();
        
    }

    public class TimeBetweenAttribute : ValidationAttribute
    {
        private readonly TimeSpan _start;
        private readonly TimeSpan _end;

        public TimeBetweenAttribute(int startHour, int startMinute, int endHour, int endMinute)
        {
            _start = new TimeSpan(startHour, startMinute, 0);
            _end = new TimeSpan(endHour, endMinute, 0);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dtValue)
            {
                TimeSpan time = dtValue.TimeOfDay;

                if (time < _start || time > _end)
                {
                    return new ValidationResult($"Time must be between {_start.Hours:D2}:{_start.Minutes:D2} and {_end.Hours:D2}:{_end.Minutes:D2}.");
                }
            }
            return ValidationResult.Success;
        }
    }
}

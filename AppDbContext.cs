using HardwareInventoryTrackingSystem;
using HardwareInventoryTrackingSystem.Account;
using HardwareInventoryTrackingSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> op):base(op)
    {
        
    }
    
    public DbSet<Item> Items { get; set; }
    public DbSet<BorrowingForm> BorrowingForms { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<ItemList> ItemLists { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var hashedPassword = HashGenerator.GenerateSHA256Hash("password");
        var studPass = HashGenerator.GenerateSHA256Hash("pass");
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().HasData(
            new User { UserId = 1, UserName = "Jill", PasswordHash = hashedPassword }


        );
        modelBuilder.Entity<Item>().HasData(
            new Item { ItemId = 1, ItemName = "Resistor", Specification = "10k Ohm", Code = "R-10K", ItemType = "Electronic", Status = "Available", QuantityInStock = 100, ImageUrl = "/images/R10.jpg" },
            new Item { ItemId = 2, ItemName = "Capacitor", Specification = "100uF", Code = "C-100", ItemType = "Electronic", Status = "Available", QuantityInStock = 50, ImageUrl = "/images/C10mH.jpg" },
            new Item { ItemId = 3, ItemName = "Inductor", Specification = "10mH", Code = "L-10M", ItemType = "Electronic", Status = "Available", QuantityInStock = 75, ImageUrl = "/images/I10mh.jpg" },
            new Item { ItemId = 4, ItemName = "Resistor", Specification = "10k Ohm", Code = "R-10K", ItemType = "Electronic", Status = "Available", QuantityInStock = 100, ImageUrl = "/images/R10.jpg" },
            new Item { ItemId = 5, ItemName = "Capacitor", Specification = "100uF", Code = "C-100", ItemType = "Electronic", Status = "Available", QuantityInStock = 50, ImageUrl = "/images/C10mH.jpg" },
            new Item { ItemId = 6, ItemName = "Inductor", Specification = "10mH", Code = "L-10M", ItemType = "Electronic", Status = "Available", QuantityInStock = 75, ImageUrl = "/images/I10mh.jpg" }
        );
        modelBuilder.Entity<Admin>().HasData(
            new Admin { AdminId = 1, Name = "John Doe", UserName = "John", Password = studPass, ImageUrl = "a.jpg" }
        );
        modelBuilder.Entity<Student>().HasData(
            new Student { StudentId = 1, StudentName = "Jilliane", Course = "CpE", Email = "j@gmail.com", LastEnrolled = 2024, ImageUrl = "/images/febiar.jpg", Password = studPass, UserName = "jrcfebiar" },
            new Student { StudentId = 2, StudentName = "Nicole", Course = "CpE", Email = "j@gmail.com", LastEnrolled = 2024, ImageUrl = "/images/febiar.jpg", Password = studPass, UserName = "nbhganzon" }
            );
       
        modelBuilder.Entity<BorrowingForm>().HasData(
            new BorrowingForm { StudentId = 1, AdminId = 1, BorrowingFormId = 1, Timestamp = DateTime.Now, RoomNo = "F313", Purpose = "Experimentation", SubjectCode = "CpE345", DateTimeReleased = new DateTime(2024, 11, 05), ExpectedReturnDate = DateTime.Now.AddDays(7), ProfessorInCharge = "Ma'am Neng" },
            new BorrowingForm
            {
                StudentId = 1,
                AdminId = 1,
                BorrowingFormId = 2,
                Timestamp = DateTime.Now,
                RoomNo = "F313B",
                Purpose = "Test",
                SubjectCode = "CpE123",
                DateTimeReleased = new DateTime(2024, 11, 05),
                ExpectedReturnDate = DateTime.Now.AddDays(7),
                ProfessorInCharge = "Ma'am Neng"
            },
        new BorrowingForm
        {
            StudentId = 2,
            AdminId = 1,
            BorrowingFormId = 3,
            Timestamp = DateTime.Now,
            RoomNo = "F313B",
            Purpose = "Test",
            SubjectCode = "CpE123",
            DateTimeReleased = new DateTime(2024, 11, 05),
            ExpectedReturnDate = DateTime.Now.AddDays(7),
            ProfessorInCharge = "Ma'am Neng"
        }
            );
        modelBuilder.Entity<ItemList>().HasData(
            new ItemList
            {
                ItemListId = 1,
                BorrowingFormId = 1, // Foreign key to BorrowingForm
                ItemId = 1,          // Foreign key to Item
                Quantity = 1,
                Remarks = "nothing"
            },
            new ItemList
            {
                ItemListId = 2,
                BorrowingFormId = 1, // Foreign key to BorrowingForm
                ItemId = 2,          // Foreign key to Item
                Quantity = 1,
                Remarks = "its all i can do to hold on to you"
            },
        new ItemList
        {
            ItemListId = 3,
            BorrowingFormId = 2, // Foreign key to BorrowingForm
            ItemId = 1,          // Foreign key to Item
            Quantity = 1,
            Remarks = "hello"
        },
        new ItemList
        {
            ItemListId = 4,
            BorrowingFormId = 2, // Foreign key to BorrowingForm
            ItemId = 2,          // Foreign key to Item
            Quantity = 1,
            Remarks = "hello"
        }


            );


    }
}


using HardwareInventoryTrackingSystem.Commands;
using HardwareInventoryTrackingSystem.Models;
using HardwareInventoryTrackingSystem.ViewModel;
using HardwareInventoryTrackingSystem.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace HardwareInventoryTrackingSystem.Services
{
    public class BorrowingFormService
    {
        private readonly AppDbContext _context;

        public BorrowingFormService(AppDbContext context)
        {
            _context = context;
        }
        //GET BORROWING FORMS
        public async Task<List<BorrowingFormViewModel>> GetForms()
        {
            var forms = await _context.BorrowingForms
                .Where(c => !c.IsDeleted)
                .Select(c => new BorrowingFormViewModel(c))
                .ToListAsync();

            return forms;

        }
        //GET SPECIFIC BORROWING FORM
        public async Task<BorrowingFormViewModel?> GetForm(int id)
        {
            return await _context.BorrowingForms
                .Where(c => c.BorrowingFormId == id)
                .Where(c => !c.IsDeleted)
                .Select(c => new BorrowingFormViewModel(c))
                .SingleOrDefaultAsync();
        }
        //POST BORROWING FORM
        public async Task<int> CreateForm(CreateBorrowingFormCommand cmd)
        {
            if (cmd.ItemList == null || !cmd.ItemList.Any())
            {
                throw new Exception("No items");
            }

            // Validate that all items have valid ItemIds
            foreach (var item in cmd.ItemList)
            {
                var existingItem = await _context.Items.FindAsync(item.ItemId);
                if (existingItem == null)
                {
                    throw new Exception($"Item with ID {item.ItemId} does not exist");
                }
            }
            var form = cmd.ToBorrowingForm();


            _context.BorrowingForms.Add(form);
            await _context.SaveChangesAsync();

            return form.BorrowingFormId;
        }
        //PUT BORROWING FORM
        //DELETE BORROWING FORM


    }

}

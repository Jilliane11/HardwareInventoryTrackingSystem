using HardwareInventoryTrackingSystem.Commands;
using HardwareInventoryTrackingSystem.Models;
using HardwareInventoryTrackingSystem.Services;
using HardwareInventoryTrackingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HardwareInventoryTrackingSystem.Pages.Item
{
    public class CreateItemModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ItemService _itemService;
        private readonly ILogger<CreateItemModel> _logger;

        public CreateItemModel(AppDbContext context, ItemService itemService, ILogger<CreateItemModel> logger)
        {
            _context = context;
            _itemService = itemService;
            _logger = logger;
        }
        [BindProperty]
        public CreateItemCommand Input { get; set; } = new CreateItemCommand();
        public void OnGet()
        {

        }
        //CREATE ITEM
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newItem = Input.ToItem();

            try
            {
                _context.Items.Add(newItem);
                await _context.SaveChangesAsync();
                return RedirectToPage("ItemView");
            }
            catch (DbUpdateException dbEx)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return Page();
            }
        }





    }
}

using HardwareInventoryTrackingSystem.Commands;
using HardwareInventoryTrackingSystem.Models;
using HardwareInventoryTrackingSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HardwareInventoryTrackingSystem.Pages.Item
{
    public class UpdateItemModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ItemService _itemService;
        private readonly ILogger<CreateItemModel> _logger;

        public UpdateItemModel(AppDbContext context, ItemService itemService, ILogger<CreateItemModel> logger)
        {
            _context = context;
            _itemService = itemService;
            _logger = logger;
        }

        [BindProperty] public UpdateItemCommand Input { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            Input = new UpdateItemCommand
            {
                Name = item.ItemName,
                Specification = item.Specification,
                Code = item.Code,
                Type = item.ItemType,
                Status = item.Status,
                Stock = item.QuantityInStock,
                Image = item.ImageUrl
            };


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var item = await _context.Items.FirstOrDefaultAsync(c => c.ItemId == id);

            if (item == null)
            {
                return NotFound();
            }
            var updateCommand = new UpdateItemCommand
            {
                Id = id,
                Name = Input.Name,
                Specification = Input.Specification,
                Code = Input.Code,
                Type = Input.Type,
                Status = Input.Status,
                Stock = Input.Stock,
                Image = Input.Image // Ensure `Input.Image` is available if ImageUrl is used
            };
            updateCommand.UpdateItem(item);
            await _context.SaveChangesAsync();

            return RedirectToPage("ItemView"); // Redirect to the page that lists items
        }
    }

}

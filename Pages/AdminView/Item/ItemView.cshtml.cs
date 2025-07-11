using HardwareInventoryTrackingSystem.Commands;
using HardwareInventoryTrackingSystem.Models;
using HardwareInventoryTrackingSystem.Services;
using HardwareInventoryTrackingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HardwareInventoryTrackingSystem.Pages.Item
{
    [Authorize(Roles = "Student,Admin")]
    public class ItemView : PageModel
    {
        private readonly AppDbContext _context;

        public ItemView(AppDbContext context)
        {
            _context = context;
        }

        public IList<ItemDetailViewModel> Items { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public async Task OnGetAsync(int id)
        {
            var itemsQuery = _context.Items.AsQueryable();

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                itemsQuery = itemsQuery.Where(i => i.ItemName.Contains(SearchTerm) ||
                                                   i.Code.Contains(SearchTerm) ||
                                                   i.Specification.Contains(SearchTerm) ||
                                                   i.ItemType.Contains(SearchTerm));
            }

            Items = await itemsQuery
                .Select(i => new ItemDetailViewModel(i))
                .ToListAsync();
        }
    }
}

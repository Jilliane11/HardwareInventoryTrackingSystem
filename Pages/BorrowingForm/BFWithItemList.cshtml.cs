using HardwareInventoryTrackingSystem.Interfaces;
using HardwareInventoryTrackingSystem.Models;
using HardwareInventoryTrackingSystem.Services;
using HardwareInventoryTrackingSystem.ViewModel;
using HardwareInventoryTrackingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HardwareInventoryTrackingSystem.Pages.BorrowingForm
{
    [Authorize(Roles = "Student, Admin")]
    public class BFWithItemListModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IPlaceFormAction _placeFormAction;

        public BFWithItemListModel(AppDbContext context, IPlaceFormAction placeFormAction)
        {
            _context = context;
            _placeFormAction = placeFormAction;
        }

        [BindProperty]
        public List<ItemListViewModel> SelectedItems { get; set; }

        [BindProperty]
        public BorrowItemsViewModel FormItemsInput { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchQuery { get; set; }

        public List<ItemViewModel> AvailableItems { get; set; }

        public Models.BorrowingForm? BorrowingForm { get; set; }

        private List<ItemListViewModel> FormItemList(IEnumerable<ItemListViewModel> itemList, IDictionary<int, Models.Item> itemsDict)
        {
            return itemList
                .Select(item => new ItemListViewModel
                {
                    Quantity = item.Quantity,
                    ItemId = item.ItemId,
                    Remarks = item.Remarks
                })
                .ToList();
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            BorrowingForm = await _context.BorrowingForms
                .FirstOrDefaultAsync(b => b.BorrowingFormId == id);

            if (BorrowingForm == null)
            {
                return NotFound();
            }

            FormItemsInput = new BorrowItemsViewModel
            {
                Form = new BorrowingFormViewModel
                {
                    AdminId = 1,
                    StudentId = 1,
                    Timestamp = DateTime.Today.Date,
                    Purpose = null,
                    Professor = null,
                    RoomNo = null,
                    SubjectCode = null,
                    BorrowedItems = null
                },
                BorrowedItems = new List<ItemListViewModel>()
            };

            var allItems = _placeFormAction.GetAvailableItems();

            // Apply search query filter
            var filteredItems = string.IsNullOrEmpty(SearchQuery)
                ? allItems
                : allItems.Where(item =>
                    item.ItemName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    item.Code.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();

            AvailableItems = filteredItems.Select(item => new ItemViewModel
            {
                Id = item.ItemId,
                ItemCode = item.Code,
                Name = item.ItemName,
                Specification = item.Specification,
                Quantity = item.QuantityInStock,
                Picture = item.ImageUrl,
            }).ToList();

            FormItemsInput.BorrowedItems = AvailableItems.Select(item => new ItemListViewModel
            {
                ItemId = item.Id,
                Quantity = 0,
                ItemName = item.Name,
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            var borrowingForm = await _context.BorrowingForms
                .FirstOrDefaultAsync(b => b.BorrowingFormId == id);

            if (borrowingForm == null)
            {
                return NotFound();
            }

            if (SelectedItems == null || !SelectedItems.Any())
            {
                ModelState.AddModelError(string.Empty, "No items were selected.");
                return Page();
            }

            var SelectedItemIds = SelectedItems.Select(i => i.ItemId).ToList();
            if (!SelectedItemIds.Any())
            {
                ModelState.AddModelError(string.Empty, "Selected item IDs did not match any available items.");
                return Page();
            }

            var itemsDict = _placeFormAction.GetAvailableItems().ToDictionary(item => item.ItemId, item => item);
            var SelectedList = FormItemList(SelectedItems, itemsDict);
            var itemList = new List<ItemList>();

            foreach (var list in SelectedList)
            {
                if (list.Quantity <= 0)
                {
                    ModelState.AddModelError(string.Empty, $"Quantity for item with ID {list.ItemId} must be greater than 0.");
                    continue;
                }

                if (!itemsDict.ContainsKey(list.ItemId))
                {
                    ModelState.AddModelError(string.Empty, $"Item with ID {list.ItemId} not found in inventory.");
                    continue;
                }

                itemList.Add(new ItemList
                {
                    BorrowingFormId = borrowingForm.BorrowingFormId,
                    ItemId = list.ItemId,
                    Quantity = list.Quantity,
                    Remarks = list.Remarks
                });
            }

            try
            {
                _context.ItemLists.AddRange(itemList);

                borrowingForm.ItemLists = itemList;
                await _context.SaveChangesAsync();

                return RedirectToPage("/BorrowingForm/BorrowingFormSummary", new { id = borrowingForm.BorrowingFormId });
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Please try again later.");
                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return Page();
            }
        }
    }
}
using HardwareInventoryTrackingSystem.Commands;
using HardwareInventoryTrackingSystem.Models;
using HardwareInventoryTrackingSystem.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HardwareInventoryTrackingSystem.Services
{
    public class ItemService
    {
        private readonly AppDbContext _context;

        public ItemService(AppDbContext context)
        {
            _context = context;
        }
        //GET ITEMS
        public async Task<List<ItemViewModel>> GetItems()
        {
            var items = await _context.Items
                .Where(c => !c.IsDeleted)
                .Select(c => new ItemViewModel(c))
                .ToListAsync();
            return items;
        }

        //GET SPECIFIC ITEM
        public async Task<ItemDetailViewModel?> GetItem(int id)
        {
            return await _context.Items
                .Where(c => c.ItemId == id)
                .Where(c => !c.IsDeleted)
                .Select(c => new ItemDetailViewModel(c))
                .SingleOrDefaultAsync();
        }

        //DELETE ITEM
        public async Task<int> DeleteItem(int id)
        {
            var toDelete = await _context.Items.SingleAsync(c => c.ItemId == id);

            toDelete.IsDeleted = true;

            await _context.SaveChangesAsync();
            return toDelete.ItemId;
        }

        //CREATE ITEM
        public async Task<int> CreateItem(CreateItemCommand cmd)
        {
            var item = cmd.ToItem();
            _context.Add(item);
            await _context.SaveChangesAsync();

            return item.ItemId;
        }

        //PUT ITEM
        public async Task<int> UpdateItem(int id, UpdateItemCommand cmd)
        {
            var item = await _context.Items.FirstOrDefaultAsync(c => c.ItemId == id);


            if (item == null)
            {
                return -1;
            }

            item.ItemName = cmd.Name;
            item.Specification = cmd.Specification;
            item.Code = cmd.Code;
            item.ItemType = cmd.Type;
            item.Status = cmd.Status;
            item.QuantityInStock = cmd.Stock;

            _context.Update(item);
            await _context.SaveChangesAsync();

            return item.ItemId;
        }

        public async Task<UpdateItemCommand> GetItemForUpdate(int itemId)
        {
            return await _context.Items
                .Where(x => x.ItemId == itemId)
                .Where(x => !x.IsDeleted)
                .Select(x => new UpdateItemCommand
                {
                    Name = x.ItemName,
                    Specification = x.Specification,
                    Code = x.Code,
                    Type = x.ItemType,
                    Status = x.Status,
                    Stock = x.QuantityInStock,
                    Image = x.ImageUrl,
                })
                .SingleOrDefaultAsync();
        }


        //SEARCH ITEM


    }
}

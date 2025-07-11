using HardwareInventoryTrackingSystem.Commands;
using HardwareInventoryTrackingSystem.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HardwareInventoryTrackingSystem.Models;

namespace HardwareInventoryTrackingSystem.Services
{
    public class AdminService
    {
        private readonly AppDbContext _context;
        public AdminService(AppDbContext context)
        {
            _context = context;
        }

        //LACKING: VALIDATIONS, ROUTE

        //GET ADMINS
        public async Task<List<AdminViewModel>> GetAdmins()
        {
            var admins = await _context.Admins
                .Where(c => !c.IsDeleted)
                .Select(c => new AdminViewModel(c))
                .ToListAsync();
            return admins;
        }

        //GET SPECIFIC STUDENT
        public async Task<AdminViewModel?> GetAdmin(int id)
        {
            return await _context.Admins
                .Where(c => c.AdminId == id)
                .Where(c => !c.IsDeleted)
                .Select(c => new AdminViewModel(c))
                .SingleOrDefaultAsync();
        }

        //DELETE STUDENT
        public async Task<int> DeleteAdmin(int id)
        {
            var toDelete = await _context.Admins.SingleAsync(c => c.AdminId == id);

            toDelete.IsDeleted = true;

            await _context.SaveChangesAsync();
            return toDelete.AdminId;
        }

        //CREATE STUDENT
        public async Task<int> CreateAdmin(CreateAdminCommand cmd)
        {
            var admin = cmd.ToAdmin();
            _context.Add(admin);
            await _context.SaveChangesAsync();

            return admin.AdminId;
        }

        //PUT STUDENT
        public async Task<int> UpdateAdmin(int id, UpdateAdminCommand cmd)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(c => c.AdminId == id);


            // Check if item exists
            if (admin == null)
            {
                return -1;
            }

            admin.Name = cmd.Name;
            admin.Password = cmd.Password;
            admin.UserName = cmd.Username;

            _context.Update(admin);

            await _context.SaveChangesAsync();

            return admin.AdminId;
        }


    }

   


    
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HardwareInventoryTrackingSystem.Commands;
using HardwareInventoryTrackingSystem.Models;
using HardwareInventoryTrackingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace HardwareInventoryTrackingSystem.Services
{
    public class StudentService
    {
        private readonly AppDbContext _context;
        
        public StudentService(AppDbContext context)
        {
            _context = context;
        }
        //LACKING: VALIDATIONS, ROUTE

        //GET STUDENTS
        //public async Task<List<StudentViewModel>> GetStudents()
        //{
        //    var students = await _context.Students
        //        .Where(c => !c.IsDeleted)
        //        .Select(c => new StudentViewModel(c))
        //        .ToListAsync();
        //    return students;
        //}

        //GET SPECIFIC STUDENT
        public async Task<StudentDetailViewModel?> GetStudent(int id)
        {
            return await _context.Students
                .Where(c => c.StudentId == id)
                .Where(c => !c.IsDeleted)
                .Select(c => new StudentDetailViewModel(c))
                .SingleOrDefaultAsync();
        }

        //DELETE STUDENT
        public async Task<int> DeleteStudent(int id)
        {
            var toDelete = await _context.Students.SingleAsync(c => c.StudentId == id);

            toDelete.IsDeleted = true;

            await _context.SaveChangesAsync();
            return toDelete.StudentId;
        }

        //CREATE STUDENT
        public async Task<int> CreateStudent(CreateStudentCommand cmd)
        {
            var student = cmd.ToStudent();
            _context.Add(student);
            await _context.SaveChangesAsync();

            return student.StudentId;
        }

        //PUT STUDENT
        public async Task<int> UpdateStudent(int id, UpdateStudentCommand cmd)
        {
            var student = await _context.Students.FirstOrDefaultAsync(c => c.StudentId == id);


            // Check if item exists
            if (student == null)
            {
                return -1;
            }

            // Update the item's properties with the values from the UpdateItemCommand (cmd)
            student.StudentName= cmd.Name;
            student.Course= cmd.Course;
            student.Email= cmd.Email;

            _context.Update(student);
            await _context.SaveChangesAsync();

            return student.StudentId;
        }


    }


    

    


}

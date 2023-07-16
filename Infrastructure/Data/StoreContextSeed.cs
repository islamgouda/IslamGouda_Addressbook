using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(Context context)
        {
            if (!context.Departments.Any())
            {
                var Departments = File.ReadAllText("../Infrastructure/SeedData/Department.json");
                var DepartmentsData = JsonSerializer.Deserialize<List<Department>>(Departments);
                await context.Departments.AddRangeAsync(DepartmentsData);

            }
            
             if(!context.Jobs.Any())
             {
                 var JobsData = File.ReadAllText("../Infrastructure/SeedData/Jobs.json");
                 var Jobs = JsonSerializer.Deserialize<List<Job>>(JobsData);
              await   context.Jobs.AddRangeAsync(Jobs);

             }
            
            
            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
          
        }

        public static async Task SeedUserAsync(UserManager<AddressBook> user)
        {

            var old = new AddressBook();

            old.PhoneNumber = "01124569877";
            old.FullName ="islam";
            //old.JobId = ;
           // old.DepartmentId = addressBookDto.DepartmentId;
            old.MobileNumber = "01123556466";
            old.DateOfBirth = new DateTime();
            old.Address ="Unknown";

            old.Email = "is@gmail.com";
            old.Password = "123";
            old.Age = 26;
            old.UserName = "islam";
            old.NormalizedUserName = "islam";
            old.EmailConfirmed = true;
            old.PhoneNumber ="01124639868";
            old.PhoneNumberConfirmed = true;
            old.Photo = "aaaa";
            //if (user.Users.Any() != null)
            //{
               await user.CreateAsync(old);
            //}

        }

    }
}

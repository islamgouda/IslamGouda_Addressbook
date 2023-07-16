using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Data
{
    public class Context:IdentityDbContext<AddressBook>
    {
        public Context(DbContextOptions dbContextOptions):base(dbContextOptions)
        {

            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach(var EntityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properitiesEntityTypes = EntityType.ClrType.GetProperties().Where(e=>e.PropertyType==typeof(decimal));
                    foreach(var prop in properitiesEntityTypes)
                    {
                        modelBuilder.Entity(EntityType.Name).Property(prop.Name).HasConversion<double>();
                    }
                }
            }
            modelBuilder.Entity<Department>().HasData(
             new List<Department>()
             {
                   new Department{Id=1,Name="IT"},

             });

            var old = new AddressBook();
            
            old.PhoneNumber = "01124569877";
            old.FullName = "islam";
            //old.JobId = ;
            // old.DepartmentId = addressBookDto.DepartmentId;
            old.MobileNumber = "01123556466";
            old.DateOfBirth = new DateTime();
            old.Address = "Unknown";

            old.Email = "is@gmail.com";
            old.Password = "123";
            old.Age = 26;
            old.UserName = "islam";
            old.NormalizedUserName = "islam";
            old.EmailConfirmed = true;
            old.PhoneNumber = "01124639868";
            old.PhoneNumberConfirmed = true;
            old.Photo = "aaaa";
            modelBuilder.Entity<Job>().HasData(
              new List<Job>()
              {
                   new Job{Id=1,Name="Engineer"},

              });
          /*  modelBuilder.Entity<Department>().HasData(
             new List<Department>()
             {
                   new Department{Id=1,Name="IT"},

             });
            modelBuilder.Entity<AddressBook>().HasData(
               new List<AddressBook>()
               {
                  old
                 
               });/*
            modelBuilder.Entity<Productype>().HasData(
              new List<Productype>()
              {
                   new Productype{Id=1,Name="phon"},

              });
            modelBuilder.Entity<Product>().HasData(
                new List<Product>()
                {
                   new Product{Id=1,Name="Iphon",ProductTypeId=1,ProductBrandId=1},
                   new Product{Id=2,Name="Samsong",ProductTypeId=1,ProductBrandId=1},
                   new Product{Id=3,Name="Oppo",ProductTypeId=1,ProductBrandId=1},
                });*/
        }
       // public DbSet<Product> Products { get; set; }
       // public DbSet<ProductBrand> ProductBrands { get; set; }
        //public DbSet<Productype> ProductTypes { get; set; }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Job> Jobs { get; set; }
    }
}

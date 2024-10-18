using Microsoft.EntityFrameworkCore;
using WorkBuddy.Api.Entities;

namespace WorkBuddy.Api.Data
{
    public class DataContext : DbContext
    {
       public DbSet<AppUser> Users { get; set; }
       public DbSet<Department> Departments { get; set; }

       public DbSet<LeaveRequest> Requests { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        { 
        
        
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optionally seed data here (if you want predefined departments)
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "HR"},
                new Department { Id = 2, Name = "Accounts and Finance"},
                new Department { Id = 3, Name = "Marketing"},
                new Department { Id = 4, Name = "Operations"},
                new Department { Id = 5, Name = "Software Development"}
            );
        }




    }
}

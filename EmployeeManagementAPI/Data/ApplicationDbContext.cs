using EmployeeManagementAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
    }
}

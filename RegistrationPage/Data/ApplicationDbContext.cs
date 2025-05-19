using Microsoft.EntityFrameworkCore;
using RegistrationPage.Models;

namespace RegistrationPage.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}

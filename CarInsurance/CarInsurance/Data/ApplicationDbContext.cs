using Microsoft.EntityFrameworkCore;
using CarInsurance.Models;

namespace CarInsurance.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Insuree> Insurees { get; set; }
    }
}


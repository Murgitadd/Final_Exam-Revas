using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Revas.Models;

namespace Revas.DAL
{
    public class AppDbContext : IdentityDbContext<AppDbContext>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Settings> Settings { get; set; }
    }
}

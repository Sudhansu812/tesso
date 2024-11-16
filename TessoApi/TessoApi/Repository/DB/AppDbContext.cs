using Microsoft.EntityFrameworkCore;
using TessoApi.Models;

namespace TessoApi.Repository.DB
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
    }
}

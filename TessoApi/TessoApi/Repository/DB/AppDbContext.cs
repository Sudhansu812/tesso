using Microsoft.EntityFrameworkCore;
using TessoApi.Models;

namespace TessoApi.Repository.DB
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectOwner> ProjectOwners { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<CollectionUser> CollectionUsers { get; set; }
        public DbSet<CollectionItem> CollectionItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.ProjectOwners)
                .WithOne(p => p.Project)
                .HasForeignKey(p => p.ProjectId);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Collections)
                .WithMany(c => c.Projects)
                .UsingEntity<CollectionItem>();

            modelBuilder.Entity<Collection>()
                .HasMany(c => c.CollectionUsers)
                .WithOne(cu => cu.Collection)
                .HasForeignKey(cu => cu.CollectionId);
        }
    }
}

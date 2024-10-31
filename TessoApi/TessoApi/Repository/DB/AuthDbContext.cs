using Microsoft.EntityFrameworkCore;

namespace TessoApi.Repository.DB
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
    }
}

using Microsoft.EntityFrameworkCore;

namespace TessoApi.Repository.DB
{
    public class ExceptionDbContext : DbContext
    {
        public ExceptionDbContext(DbContextOptions<ExceptionDbContext> options) : base(options) { }
    }
}

﻿using Microsoft.EntityFrameworkCore;

namespace TessoApi.Repository.DB
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}

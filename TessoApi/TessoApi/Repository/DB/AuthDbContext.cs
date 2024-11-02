using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using TessoApi.Models.Identity;

namespace TessoApi.Repository.DB
{
    public class AuthDbContext(DbContextOptions<AuthDbContext> options) : IdentityDbContext<User>(options)
    {
    }
}

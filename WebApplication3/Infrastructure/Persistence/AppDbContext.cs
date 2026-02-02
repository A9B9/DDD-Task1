using Microsoft.EntityFrameworkCore;

namespace WebApplication3.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
}
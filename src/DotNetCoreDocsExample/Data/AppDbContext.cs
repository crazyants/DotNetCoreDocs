using DotNetCoreDocsExample.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreApplicationsExample.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public virtual DbSet<TodoItem> TodoItems { get; set; }
    public virtual DbSet<User> Users { get; set; }
  }
}
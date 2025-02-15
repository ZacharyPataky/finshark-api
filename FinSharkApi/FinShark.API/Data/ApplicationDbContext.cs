using FinShark.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FinShark.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    { }

    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }
}

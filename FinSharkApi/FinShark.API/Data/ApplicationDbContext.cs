using FinShark.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinShark.API.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    { }

    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        List<IdentityRole> roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Id = Guid.Parse("25622371-0be0-48f0-aaf6-a40661b14606").ToString(),
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Id = Guid.Parse("423c81a5-f153-46fe-a4fc-3456024e640d").ToString(),
                Name = "User",
                NormalizedName = "USER"
            }
        };
        builder.Entity<IdentityRole>().HasData(roles);
    }
}

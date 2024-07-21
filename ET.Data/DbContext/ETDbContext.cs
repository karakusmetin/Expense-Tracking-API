using ET.Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace ET.Data;

public class ETDbContext : DbContext
{
    public ETDbContext(DbContextOptions<ETDbContext> options) : base(options) 
    { 

    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}

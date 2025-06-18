using Microsoft.EntityFrameworkCore;
using Requeceptor.Domain;

namespace Requeceptor.Services.Persistence;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }
    public DbSet<RequestRecord> Requests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RequestRecord>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            //entity.Property(e => e.ReceivedAt).HasDefaultValueSql("GETDATE()");
        });
    }
}

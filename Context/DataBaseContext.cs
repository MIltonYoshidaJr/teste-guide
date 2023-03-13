using Microsoft.EntityFrameworkCore;
using GUIDE.Models;

public class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions<DataBaseContext> options)
        : base(options)
    {
    }

    public DbSet<Ativos> Ativos { get; set; }

    public DbSet<Users> Users { get; set; }
}
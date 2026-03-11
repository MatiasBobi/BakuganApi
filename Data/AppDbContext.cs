using BakuganApi.models;
using BakuganAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BakuganApi.Data;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BakuganModel>().Property(b => b.Precio).HasPrecision(10, 2);
    }

    // Definiendo los DbSets para mis entidades

    public DbSet<BakuganModel> Bakugans { get; set; }
    public DbSet<BakuganSkillModel> BakuganSkills { get; set; }
    public DbSet<BakuganCategory> BakuganCategories { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DbaRC.Models;

public class SettingsContext : DbContext
{
    public SettingsContext(DbContextOptions<SettingsContext> options)
        : base(options)
    {
    }

    public DbSet<Setting>? Setting { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Setting>().ToTable("Settings");

    }
}

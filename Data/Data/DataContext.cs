﻿using DocLink.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DocLink.Data.Data;

public class DataContext : IdentityDbContext<AppUser>
{
    public DataContext(DbContextOptions options) : base(options) { }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole() { Name = "member" },
            new IdentityRole() { Name = "doctor" },
            new IdentityRole() { Name = "admin" }
            );
        base.OnModelCreating(builder);
    }
}
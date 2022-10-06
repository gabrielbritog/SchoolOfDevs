﻿using Microsoft.EntityFrameworkCore;
using SchoolOfDevs.Entities;
using SchoolOfDevs.Enums;

namespace SchoolOfDevs.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .Property(e => e.TyperUser)
                .HasConversion(
                   v => v.ToString(),
                   v => (TyperUser)Enum.Parse(typeof(TyperUser),v)); // Converte o Enum para string para o banco receber o nome e não o número
                   
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}
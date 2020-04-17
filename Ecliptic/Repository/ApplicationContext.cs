﻿using Ecliptic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ecliptic.Repository
{
    public class ApplicationContext : DbContext
    {
        private string databaseName;

        public DbSet<Building> Buildings { get; set; }

        public DbSet<User> User { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Worker> Workers { get; set; }


        public static ApplicationContext db = new ApplicationContext();

        public ApplicationContext(string databasePath = "database.db")
        {
            databaseName = databasePath;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // builder.Entity<User>().HasMany(c => c.Notes).WithOne(e => e.User);

            //   builder.Entity<Worker>().HasOne(c => c.Room)
            //                         .WithMany(c => c.Workers)
            //                         .HasForeignKey(c => c.RoomId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            String databasePath =
              Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), databaseName);

            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }
    }
}

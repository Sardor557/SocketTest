using Microsoft.EntityFrameworkCore;
using SocketTest.Database.Extensions;
using SocketTest.Models;
using System;
using System.Linq;
using Toolbelt.ComponentModel.DataAnnotations;

namespace SocketTest.Database
{
    public sealed partial class MyDbContext : DbContext
    {

        #region
        public DbSet<tbUser> tbUsers { get; set; }
        public DbSet<tbMessage> tbMessages { get; set; }
        #endregion
        public MyDbContext(DbContextOptions options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();

            modelBuilder.BuildIndexesFromAnnotations();

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}

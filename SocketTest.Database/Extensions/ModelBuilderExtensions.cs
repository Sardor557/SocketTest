using Microsoft.EntityFrameworkCore;
using SocketTest.Models;
using SocketTest.Shared.Utils;
using System;

namespace SocketTest.Database.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbUser>().HasData(new tbUser()
            {
                Id = 1,
                Login = "admin",
                Password = CHash.EncryptMD5("1"),
                Status = 1,
                CreateDate = DateTime.Now,
                CreateUser = 1
            });
            modelBuilder.Entity<tbUser>().HasData(new tbUser()
            {
                Id = 2,
                Login = "director",
                Password = CHash.EncryptMD5("2"),
                Status = 1,
                CreateDate = DateTime.Now,
                CreateUser = 1,
            });
            modelBuilder.Entity<tbUser>().HasData(new tbUser()
            {
                Id = 3,
                Login = "manager",
                Password = CHash.EncryptMD5("3"),
                Status = 1,
                CreateDate = DateTime.Now,
                CreateUser = 1,
            });
            modelBuilder.Entity<tbUser>().HasData(new tbUser()
            {
                Id = 4,
                Login = "sklad",
                Password = CHash.EncryptMD5("4"),
                Status = 1,
                CreateDate = DateTime.Now,
                CreateUser = 1
            });
        }
    }
}

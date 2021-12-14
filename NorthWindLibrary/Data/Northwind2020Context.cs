﻿
// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using NorthWindLibrary.Data.Configurations;
using NorthWindLibrary.Models;
using System;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace NorthWindLibrary.Data
{
    public partial class Northwind2020Context : DbContext
    {
        public Northwind2020Context()
        {
        }

        public Northwind2020Context(DbContextOptions<Northwind2020Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<ContactDevices> ContactDevices { get; set; }
        public virtual DbSet<ContactType> ContactType { get; set; }
        public virtual DbSet<Contacts> Contacts { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<EmployeeTerritories> EmployeeTerritories { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<PhoneType> PhoneType { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Shippers> Shippers { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NorthWind2020;Integrated Security=True");
            }
        }

        /// <summary>
        /// Read connection string from appsettings.json
        /// </summary>
        /// <param name="builder"></param>
        /// <returns>Connection string</returns>
        /// <remarks>
        /// Next level (which I have coded) is to have three environment which can
        /// come from one or more appsettings files along with setting the environment
        /// via Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") for ASP.NET Core
        /// or a custom variable e.g. Environment.GetEnvironmentVariable("OED_ENVIRONMENT")
        /// </remarks>
        private static IConfigurationRoot ReadAppsettings(out IConfigurationBuilder builder)
        {
            builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfigurationRoot config = builder.Build();


            return config; // connection string
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.ApplyConfiguration(new Configurations.CategoriesConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ContactDevicesConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ContactTypeConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ContactsConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.CountriesConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.CustomersConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.EmployeeTerritoriesConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.EmployeesConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.OrderDetailsConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.OrdersConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PhoneTypeConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ProductsConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.RegionConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ShippersConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.SuppliersConfiguration());
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
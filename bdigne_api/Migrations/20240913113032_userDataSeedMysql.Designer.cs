﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using bdigne_api.Db;

#nullable disable

namespace bdigne_api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240913113032_userDataSeedMysql")]
    partial class userDataSeedMysql
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("bdigne_api.Db.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@example.com",
                            Password = "AdminPass123",
                            Role = 0,
                            UserName = "admin"
                        },
                        new
                        {
                            Id = 2,
                            Email = "dev1@example.com",
                            Password = "DevPass123",
                            Role = 1,
                            UserName = "dev1"
                        },
                        new
                        {
                            Id = 3,
                            Email = "dev2@example.com",
                            Password = "DevPass123",
                            Role = 1,
                            UserName = "dev2"
                        },
                        new
                        {
                            Id = 4,
                            Email = "dev3@example.com",
                            Password = "DevPass123",
                            Role = 1,
                            UserName = "dev3"
                        },
                        new
                        {
                            Id = 5,
                            Email = "dev4@example.com",
                            Password = "DevPass123",
                            Role = 1,
                            UserName = "dev4"
                        },
                        new
                        {
                            Id = 6,
                            Email = "dev5@example.com",
                            Password = "DevPass123",
                            Role = 1,
                            UserName = "dev5"
                        },
                        new
                        {
                            Id = 7,
                            Email = "pm1@example.com",
                            Password = "PmPass123",
                            Role = 2,
                            UserName = "pm1"
                        },
                        new
                        {
                            Id = 8,
                            Email = "pm2@example.com",
                            Password = "PmPass123",
                            Role = 2,
                            UserName = "pm2"
                        },
                        new
                        {
                            Id = 9,
                            Email = "pm3@example.com",
                            Password = "PmPass123",
                            Role = 2,
                            UserName = "pm3"
                        },
                        new
                        {
                            Id = 10,
                            Email = "pm4@example.com",
                            Password = "PmPass123",
                            Role = 2,
                            UserName = "pm4"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}

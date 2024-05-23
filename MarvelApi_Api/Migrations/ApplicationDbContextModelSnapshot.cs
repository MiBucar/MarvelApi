﻿// <auto-generated />
using System;
using MarvelApi_Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MarvelApi_Api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MarvelApi_Api.Models.Hero", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)");

                    b.Property<bool>("IsVillain")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Heroes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DateCreated = new DateTime(2024, 5, 22, 17, 52, 36, 660, DateTimeKind.Utc).AddTicks(2520),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsVillain = false,
                            Name = "Iron man"
                        },
                        new
                        {
                            Id = 2,
                            DateCreated = new DateTime(2024, 5, 22, 17, 52, 36, 660, DateTimeKind.Utc).AddTicks(2520),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsVillain = true,
                            Name = "Thanos"
                        },
                        new
                        {
                            Id = 3,
                            DateCreated = new DateTime(2024, 5, 22, 17, 52, 36, 660, DateTimeKind.Utc).AddTicks(2520),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsVillain = false,
                            Name = "Daredevil"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
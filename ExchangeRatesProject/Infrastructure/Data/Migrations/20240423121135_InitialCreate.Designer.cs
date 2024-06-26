﻿// <auto-generated />
using System;
using ExchangeRatesProject.Infrastructure.Data.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ExchangeRatesProject.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240423121135_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ExchangeRatesProject.Entities.Tbl_Log", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("HATA_ACIKLAMASI")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HATA_FONKSIYON")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ISLEM_TARIHI")
                        .HasColumnType("datetime2");

                    b.Property<string>("KULLANICI_IP")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KUR")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("YUZDESEL_DEGISIM")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Logs");
                });
#pragma warning restore 612, 618
        }
    }
}

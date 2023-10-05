﻿// <auto-generated />
using CoffeMachine.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CoffeMachine.Migrations
{
    [DbContext(typeof(CoffeeContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CoffeMachine.Models.Coffee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("Price")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Coffees");
                });

            modelBuilder.Entity("CoffeMachine.Models.CoffeeBalance", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<long>("Balance")
                        .HasColumnType("bigint");

                    b.HasKey("Name");

                    b.ToTable("CoffeeBalances");
                });

            modelBuilder.Entity("CoffeMachine.Models.MoneyInMachine", b =>
                {
                    b.Property<long>("Nominal")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Nominal"));

                    b.Property<long>("Count")
                        .HasColumnType("bigint");

                    b.HasKey("Nominal");

                    b.ToTable("MoneyInMachines");
                });
#pragma warning restore 612, 618
        }
    }
}

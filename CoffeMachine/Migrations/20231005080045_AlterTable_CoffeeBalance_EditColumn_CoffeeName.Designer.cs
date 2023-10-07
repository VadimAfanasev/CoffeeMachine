﻿// <auto-generated />
using CoffeMachine.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CoffeMachine.Migrations
{
    [DbContext(typeof(CoffeeContext))]
    [Migration("20231005080045_AlterTable_CoffeeBalance_EditColumn_CoffeeName")]
    partial class AlterTable_CoffeeBalance_EditColumn_CoffeeName
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
                    b.Property<string>("CoffeeName")
                        .HasColumnType("text");

                    b.Property<long>("Balance")
                        .HasColumnType("bigint");

                    b.HasKey("CoffeeName");

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
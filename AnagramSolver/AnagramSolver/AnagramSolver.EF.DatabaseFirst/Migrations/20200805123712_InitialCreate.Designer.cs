﻿// <auto-generated />
using System;
using AnagramSolver.EF.DatabaseFirst.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AnagramSolver.EF.DatabaseFirst.Migrations
{
    [DbContext(typeof(AnagramSolverContext))]
    [Migration("20200805123712_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AnagramSolver.EF.DatabaseFirst.Entities.CachedWordEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AnagramsIds")
                        .HasColumnName("Anagrams_ids")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phrase")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("CachedWords");
                });

            modelBuilder.Entity("AnagramSolver.EF.DatabaseFirst.Entities.UserLogEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Ip")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phrase")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("SearchTime")
                        .HasColumnName("Search_time")
                        .HasColumnType("time");

                    b.HasKey("ID");

                    b.ToTable("UserLogs");
                });

            modelBuilder.Entity("AnagramSolver.EF.DatabaseFirst.Entities.WordEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SortedWord")
                        .HasColumnName("Sorted_word")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Word")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Words");
                });
#pragma warning restore 612, 618
        }
    }
}

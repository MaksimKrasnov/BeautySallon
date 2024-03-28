﻿// <auto-generated />
using System;
using BeautySaloon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BeautySaloon.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BeautySaloon.Models.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClientName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MasterServiceId")
                        .HasColumnType("integer");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MasterServiceId")
                        .IsUnique();

                    b.ToTable("Appointment");
                });

            modelBuilder.Entity("BeautySaloon.Models.Master", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Master");
                });

            modelBuilder.Entity("BeautySaloon.Models.MasterService", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("MasterId")
                        .HasColumnType("integer");

                    b.Property<int>("ServiceId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MasterId");

                    b.HasIndex("ServiceId");

                    b.ToTable("MasterService");
                });

            modelBuilder.Entity("BeautySaloon.Models.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.Property<int>("ServicesId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ServicesId")
                        .IsUnique();

                    b.ToTable("Service");
                });

            modelBuilder.Entity("BeautySaloon.Models.Services", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("BeautySaloon.Models.Appointment", b =>
                {
                    b.HasOne("BeautySaloon.Models.MasterService", "MasterService")
                        .WithOne("Appointment")
                        .HasForeignKey("BeautySaloon.Models.Appointment", "MasterServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MasterService");
                });

            modelBuilder.Entity("BeautySaloon.Models.MasterService", b =>
                {
                    b.HasOne("BeautySaloon.Models.Master", "Master")
                        .WithMany("MasterServices")
                        .HasForeignKey("MasterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeautySaloon.Models.Service", "Service")
                        .WithMany("MasterServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Master");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("BeautySaloon.Models.Service", b =>
                {
                    b.HasOne("BeautySaloon.Models.Services", "Services")
                        .WithOne("Service")
                        .HasForeignKey("BeautySaloon.Models.Service", "ServicesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Services");
                });

            modelBuilder.Entity("BeautySaloon.Models.Master", b =>
                {
                    b.Navigation("MasterServices");
                });

            modelBuilder.Entity("BeautySaloon.Models.MasterService", b =>
                {
                    b.Navigation("Appointment")
                        .IsRequired();
                });

            modelBuilder.Entity("BeautySaloon.Models.Service", b =>
                {
                    b.Navigation("MasterServices");
                });

            modelBuilder.Entity("BeautySaloon.Models.Services", b =>
                {
                    b.Navigation("Service")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

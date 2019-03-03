﻿// <auto-generated />
using System;
using LOBCore.DataAccesses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LOBCore.Migrations
{
    [DbContext(typeof(LOBDatabaseContext))]
    [Migration("20190303121703_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity("LOBCore.DataAccesses.Entities.CommUserGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("CommUserGroups");
                });

            modelBuilder.Entity("LOBCore.DataAccesses.Entities.CommUserGroupItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CommUserGroupId");

                    b.Property<string>("Email");

                    b.Property<string>("Mobile");

                    b.Property<string>("Name");

                    b.Property<string>("Phone");

                    b.HasKey("Id");

                    b.HasIndex("CommUserGroupId");

                    b.ToTable("CommUserGroupItems");
                });

            modelBuilder.Entity("LOBCore.DataAccesses.Entities.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("LOBCore.DataAccesses.Entities.ExceptionRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CallStack");

                    b.Property<string>("DeviceModel");

                    b.Property<string>("DeviceName");

                    b.Property<DateTime>("ExceptionTime");

                    b.Property<string>("Message");

                    b.Property<string>("OSType");

                    b.Property<string>("OSVersion");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ExceptionRecords");
                });

            modelBuilder.Entity("LOBCore.DataAccesses.Entities.LeaveForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("BeginTime");

                    b.Property<string>("Description");

                    b.Property<DateTime>("EndTime");

                    b.Property<int?>("LeaveFormTypeId");

                    b.Property<int>("TotalHours");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("LeaveFormTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("LeaveForms");
                });

            modelBuilder.Entity("LOBCore.DataAccesses.Entities.LeaveFormType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("LeaveFormTypes");
                });

            modelBuilder.Entity("LOBCore.DataAccesses.Entities.LobUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Account");

                    b.Property<int?>("DepartmentId");

                    b.Property<string>("Image");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("LobUsers");
                });

            modelBuilder.Entity("LOBCore.DataAccesses.Entities.NotificationToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Invalid");

                    b.Property<int>("OSType");

                    b.Property<DateTime>("RegistrationTime");

                    b.Property<string>("Token");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("NotificationTokens");
                });

            modelBuilder.Entity("LOBCore.DataAccesses.Entities.Suggestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Message");

                    b.Property<string>("Subject");

                    b.Property<DateTime>("SubmitTime");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Suggestions");
                });

            modelBuilder.Entity("LOBCore.DataAccesses.Entities.SystemEnvironment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AndroidUrl");

                    b.Property<string>("AndroidVersion");

                    b.Property<string>("AppName");

                    b.Property<string>("iOSUrl");

                    b.Property<string>("iOSVersion");

                    b.HasKey("Id");

                    b.ToTable("SystemEnvironment");
                });

            modelBuilder.Entity("LOBCore.DataAccesses.Entities.CommUserGroupItem", b =>
                {
                    b.HasOne("LOBCore.DataAccesses.Entities.CommUserGroup", "CommUserGroup")
                        .WithMany()
                        .HasForeignKey("CommUserGroupId");
                });

            modelBuilder.Entity("LOBCore.DataAccesses.Entities.ExceptionRecord", b =>
                {
                    b.HasOne("LOBCore.DataAccesses.Entities.LobUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("LOBCore.DataAccesses.Entities.LeaveForm", b =>
                {
                    b.HasOne("LOBCore.DataAccesses.Entities.LeaveFormType", "LeaveFormType")
                        .WithMany()
                        .HasForeignKey("LeaveFormTypeId");

                    b.HasOne("LOBCore.DataAccesses.Entities.LobUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("LOBCore.DataAccesses.Entities.LobUser", b =>
                {
                    b.HasOne("LOBCore.DataAccesses.Entities.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId");
                });

            modelBuilder.Entity("LOBCore.DataAccesses.Entities.NotificationToken", b =>
                {
                    b.HasOne("LOBCore.DataAccesses.Entities.LobUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("LOBCore.DataAccesses.Entities.Suggestion", b =>
                {
                    b.HasOne("LOBCore.DataAccesses.Entities.LobUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}

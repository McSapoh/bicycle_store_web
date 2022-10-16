﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using bicycle_store_web;

namespace bicycle_store_web.Migrations
{
    [DbContext(typeof(bicycle_storeContext))]
    partial class bicycle_storeContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("bicycle_store_web.Bicycle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<uint>("Price")
                        .HasColumnType("int unsigned");

                    b.Property<int>("ProducerId")
                        .HasColumnType("int");

                    b.Property<uint>("Quantity")
                        .HasColumnType("int unsigned");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.Property<float>("WheelDiameter")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("ProducerId");

                    b.HasIndex("TypeId");

                    b.ToTable("Bicycles");
                });

            modelBuilder.Entity("bicycle_store_web.BicycleOrder", b =>
                {
                    b.Property<int>("BicycleOrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BicyclePrice")
                        .HasColumnType("int");

                    b.Property<int>("BicyclesId")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("BicycleOrderId");

                    b.HasIndex("BicyclesId");

                    b.HasIndex("OrderId");

                    b.ToTable("BicycleOrders");
                });

            modelBuilder.Entity("bicycle_store_web.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("bicycle_store_web.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ClientsId")
                        .HasColumnType("int");

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime(6)");

                    b.HasKey("OrderId");

                    b.HasIndex("ClientsId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("bicycle_store_web.Producer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Producers");
                });

            modelBuilder.Entity("bicycle_store_web.Type", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Types");
                });

            modelBuilder.Entity("bicycle_store_web.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Adress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext");

                    b.Property<string>("Role")
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("bicycle_store_web.Bicycle", b =>
                {
                    b.HasOne("bicycle_store_web.Country", "Country")
                        .WithMany("Bicycles")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("bicycle_store_web.Producer", "Producer")
                        .WithMany("Bicycles")
                        .HasForeignKey("ProducerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("bicycle_store_web.Type", "Type")
                        .WithMany("Bicycles")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");

                    b.Navigation("Producer");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("bicycle_store_web.BicycleOrder", b =>
                {
                    b.HasOne("bicycle_store_web.Bicycle", "Bicycles")
                        .WithMany("BicycleOrders")
                        .HasForeignKey("BicyclesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("bicycle_store_web.Order", "Order")
                        .WithMany("BicycleOrders")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bicycles");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("bicycle_store_web.Order", b =>
                {
                    b.HasOne("bicycle_store_web.User", "Clients")
                        .WithMany("Orders")
                        .HasForeignKey("ClientsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Clients");
                });

            modelBuilder.Entity("bicycle_store_web.Bicycle", b =>
                {
                    b.Navigation("BicycleOrders");
                });

            modelBuilder.Entity("bicycle_store_web.Country", b =>
                {
                    b.Navigation("Bicycles");
                });

            modelBuilder.Entity("bicycle_store_web.Order", b =>
                {
                    b.Navigation("BicycleOrders");
                });

            modelBuilder.Entity("bicycle_store_web.Producer", b =>
                {
                    b.Navigation("Bicycles");
                });

            modelBuilder.Entity("bicycle_store_web.Type", b =>
                {
                    b.Navigation("Bicycles");
                });

            modelBuilder.Entity("bicycle_store_web.User", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}

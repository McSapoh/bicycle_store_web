// <auto-generated />
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
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("longblob");

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

                    b.Property<int>("BicycleCost")
                        .HasColumnType("int");

                    b.Property<int>("BicycleId")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("BicycleOrderId");

                    b.HasIndex("BicycleId");

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

            modelBuilder.Entity("bicycle_store_web.Models.ShoppingCart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ShopingCarts");
                });

            modelBuilder.Entity("bicycle_store_web.Models.ShoppingCartOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BicycleId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("ShoppingCartId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BicycleId");

                    b.HasIndex("ShoppingCartId");

                    b.ToTable("ShoppingCartOrders");
                });

            modelBuilder.Entity("bicycle_store_web.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Status")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("UserId");

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
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("longblob");

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
                    b.HasOne("bicycle_store_web.Bicycle", "Bicycle")
                        .WithMany("BicycleOrders")
                        .HasForeignKey("BicycleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("bicycle_store_web.Order", "Order")
                        .WithMany("BicycleOrders")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bicycle");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("bicycle_store_web.Models.ShoppingCart", b =>
                {
                    b.HasOne("bicycle_store_web.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("bicycle_store_web.Models.ShoppingCartOrder", b =>
                {
                    b.HasOne("bicycle_store_web.Bicycle", "Bicycle")
                        .WithMany("BicycleCartOrders")
                        .HasForeignKey("BicycleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("bicycle_store_web.Models.ShoppingCart", "ShoppingCart")
                        .WithMany("ShoppingCartOrders")
                        .HasForeignKey("ShoppingCartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bicycle");

                    b.Navigation("ShoppingCart");
                });

            modelBuilder.Entity("bicycle_store_web.Order", b =>
                {
                    b.HasOne("bicycle_store_web.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("bicycle_store_web.Bicycle", b =>
                {
                    b.Navigation("BicycleCartOrders");

                    b.Navigation("BicycleOrders");
                });

            modelBuilder.Entity("bicycle_store_web.Country", b =>
                {
                    b.Navigation("Bicycles");
                });

            modelBuilder.Entity("bicycle_store_web.Models.ShoppingCart", b =>
                {
                    b.Navigation("ShoppingCartOrders");
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

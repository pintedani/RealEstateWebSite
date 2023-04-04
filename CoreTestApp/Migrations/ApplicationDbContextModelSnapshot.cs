﻿// <auto-generated />
using System;
using CoreTestApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CoreTestApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CoreTestApp.Models.FavoriteAnuntItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FavoritesID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ImobilId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ImobilId");

                    b.ToTable("FavoriteAnuntItems");
                });

            modelBuilder.Entity("CoreTestApp.Models.Imobil", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("AerConditionat")
                        .HasColumnType("bit");

                    b.Property<bool>("CT")
                        .HasColumnType("bit");

                    b.Property<string>("ContactEmail")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("ContactTelefon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataAdaugare")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataAdaugareInitiala")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataAprobare")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Decomandat")
                        .HasColumnType("bit");

                    b.Property<string>("Descriere")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Etaj")
                        .HasColumnType("int");

                    b.Property<bool>("Finisat")
                        .HasColumnType("bit");

                    b.Property<bool>("Garaj")
                        .HasColumnType("bit");

                    b.Property<double>("GoogleMarkerCoordinateLat")
                        .HasColumnType("float");

                    b.Property<double>("GoogleMarkerCoordinateLong")
                        .HasColumnType("float");

                    b.Property<string>("LinkExtern")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LocInPivnita")
                        .HasColumnType("bit");

                    b.Property<bool>("LocParcare")
                        .HasColumnType("bit");

                    b.Property<bool>("Negociabil")
                        .HasColumnType("bit");

                    b.Property<int?>("NrBai")
                        .HasColumnType("int");

                    b.Property<int?>("NrBalcoane")
                        .HasColumnType("int");

                    b.Property<int?>("NumarCamere")
                        .HasColumnType("int");

                    b.Property<int>("NumarTotalEtaje")
                        .HasColumnType("int");

                    b.Property<int>("NumarVizualizari")
                        .HasColumnType("int");

                    b.Property<string>("ObservatiiAdmin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Poze")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Strada")
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<int>("Suprafata")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Valabilitate")
                        .HasColumnType("int");

                    b.Property<int?>("VechimeImobil")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Imobils");
                });

            modelBuilder.Entity("CoreTestApp.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AddressLine1")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("AddressLine2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("OrderPlaced")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("OrderTotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("State")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("CoreTestApp.Models.OrderDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("ImobilId")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ImobilId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("CoreTestApp.Models.ShoppingCartItem", b =>
                {
                    b.Property<int>("ShoppingCartItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShoppingCartItemId"));

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("ImobilId")
                        .HasColumnType("int");

                    b.Property<string>("ShoppingCartId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ShoppingCartItemId");

                    b.HasIndex("ImobilId");

                    b.ToTable("ShoppingCartItems");
                });

            modelBuilder.Entity("CoreTestApp.Models.FavoriteAnuntItem", b =>
                {
                    b.HasOne("CoreTestApp.Models.Imobil", "Imobil")
                        .WithMany()
                        .HasForeignKey("ImobilId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Imobil");
                });

            modelBuilder.Entity("CoreTestApp.Models.OrderDetail", b =>
                {
                    b.HasOne("CoreTestApp.Models.Imobil", "Imobil")
                        .WithMany()
                        .HasForeignKey("ImobilId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoreTestApp.Models.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Imobil");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("CoreTestApp.Models.ShoppingCartItem", b =>
                {
                    b.HasOne("CoreTestApp.Models.Imobil", "Imobil")
                        .WithMany()
                        .HasForeignKey("ImobilId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Imobil");
                });

            modelBuilder.Entity("CoreTestApp.Models.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });
#pragma warning restore 612, 618
        }
    }
}

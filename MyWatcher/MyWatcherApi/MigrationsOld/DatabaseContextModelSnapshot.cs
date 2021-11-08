﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyWatcher.Services;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyWatcherApi.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0-rc.2.21480.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MyWatcher.Entities.Continent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Continent", (string)null);
                });

            modelBuilder.Entity("MyWatcher.Entities.Country", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<Guid>("ContinentId")
                        .HasColumnType("uuid");

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ContinentId");

                    b.ToTable("Country", (string)null);
                });

            modelBuilder.Entity("MyWatcher.Entities.SecondHandItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastScan")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastWeeklyPriceUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<double>("PriceLastWeek")
                        .HasColumnType("double precision");

                    b.Property<double>("PriceLowestKnown")
                        .HasColumnType("double precision");

                    b.Property<double>("PriceThisWeek")
                        .HasColumnType("double precision");

                    b.Property<int>("Service")
                        .HasColumnType("integer");

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SecondHandItem", (string)null);
                });

            modelBuilder.Entity("MyWatcher.Entities.StockItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastScan")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastWeeklyPriceUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<double>("PriceLastWeek")
                        .HasColumnType("double precision");

                    b.Property<double>("PriceLowestKnown")
                        .HasColumnType("double precision");

                    b.Property<double>("PriceThisWeek")
                        .HasColumnType("double precision");

                    b.Property<int>("Service")
                        .HasColumnType("integer");

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("StockItem", (string)null);
                });

            modelBuilder.Entity("MyWatcher.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("LastLogin")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("MyWatcher.Entities.UserSecondHandItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("SecondHandItemId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SecondHandItemId");

                    b.HasIndex("UserId");

                    b.ToTable("UserSecondHandItem", (string)null);
                });

            modelBuilder.Entity("MyWatcher.Entities.UserStockItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("StockItemId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("StockItemId");

                    b.HasIndex("UserId");

                    b.ToTable("UserStockItem", (string)null);
                });

            modelBuilder.Entity("MyWatcher.Entities.Website", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<string>("BaseUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("CountryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Regexes")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Website", (string)null);
                });

            modelBuilder.Entity("MyWatcher.Entities.WebsiteSecondHandItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.Property<Guid>("SecondHandItemId")
                        .HasColumnType("uuid");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("WebsiteId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SecondHandItemId");

                    b.HasIndex("WebsiteId");

                    b.ToTable("WebsiteSecondHandItem", (string)null);
                });

            modelBuilder.Entity("MyWatcher.Entities.WebsiteStockItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.Property<Guid>("StockItemId")
                        .HasColumnType("uuid");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("WebsiteId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("StockItemId");

                    b.HasIndex("WebsiteId");

                    b.ToTable("WebsiteStockItem", (string)null);
                });

            modelBuilder.Entity("MyWatcher.Entities.Country", b =>
                {
                    b.HasOne("MyWatcher.Entities.Continent", "Continent")
                        .WithMany()
                        .HasForeignKey("ContinentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Continent");
                });

            modelBuilder.Entity("MyWatcher.Entities.UserSecondHandItem", b =>
                {
                    b.HasOne("MyWatcher.Entities.SecondHandItem", "SecondHandItem")
                        .WithMany()
                        .HasForeignKey("SecondHandItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyWatcher.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SecondHandItem");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyWatcher.Entities.UserStockItem", b =>
                {
                    b.HasOne("MyWatcher.Entities.StockItem", "StockItem")
                        .WithMany()
                        .HasForeignKey("StockItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyWatcher.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StockItem");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyWatcher.Entities.Website", b =>
                {
                    b.HasOne("MyWatcher.Entities.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("MyWatcher.Entities.WebsiteSecondHandItem", b =>
                {
                    b.HasOne("MyWatcher.Entities.SecondHandItem", "SecondHandItem")
                        .WithMany()
                        .HasForeignKey("SecondHandItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyWatcher.Entities.Website", "Website")
                        .WithMany()
                        .HasForeignKey("WebsiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SecondHandItem");

                    b.Navigation("Website");
                });

            modelBuilder.Entity("MyWatcher.Entities.WebsiteStockItem", b =>
                {
                    b.HasOne("MyWatcher.Entities.StockItem", "StockItem")
                        .WithMany()
                        .HasForeignKey("StockItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyWatcher.Entities.Website", "Website")
                        .WithMany()
                        .HasForeignKey("WebsiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StockItem");

                    b.Navigation("Website");
                });
#pragma warning restore 612, 618
        }
    }
}

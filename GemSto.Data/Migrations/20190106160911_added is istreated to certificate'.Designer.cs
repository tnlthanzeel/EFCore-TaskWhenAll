﻿// <auto-generated />
using System;
using GemSto.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GemSto.Data.Migrations
{
    [DbContext(typeof(GemStoContext))]
    [Migration("20190106160911_added is istreated to certificate'")]
    partial class addedisistreatedtocertificate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GemSto.Domain.Certificate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CertificateProviderId");

                    b.Property<int>("ColourId");

                    b.Property<int>("GemId");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsTreated");

                    b.Property<string>("Number");

                    b.Property<int?>("OriginId");

                    b.Property<int?>("TransactionId");

                    b.HasKey("Id");

                    b.HasIndex("CertificateProviderId");

                    b.HasIndex("ColourId");

                    b.HasIndex("GemId");

                    b.HasIndex("OriginId");

                    b.HasIndex("TransactionId");

                    b.ToTable("Certificates");
                });

            modelBuilder.Entity("GemSto.Domain.Export", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("ExportDate");

                    b.Property<int>("ExportType");

                    b.HasKey("Id");

                    b.ToTable("Exports");
                });

            modelBuilder.Entity("GemSto.Domain.Gem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("BrokerFee");

                    b.Property<string>("CreatedById");

                    b.Property<double>("Depth");

                    b.Property<string>("EditedById");

                    b.Property<DateTimeOffset?>("EditedOn");

                    b.Property<int?>("GemLotId");

                    b.Property<int>("GemStatus");

                    b.Property<decimal>("InitialCost");

                    b.Property<decimal>("InitialWeight");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsGemLot");

                    b.Property<bool?>("IsTreated");

                    b.Property<double>("Length");

                    b.Property<string>("Note");

                    b.Property<int>("NumberOfPieces");

                    b.Property<int>("PaymentStatusToSeller");

                    b.Property<DateTimeOffset?>("PurchasedDate");

                    b.Property<decimal>("RecutWeight");

                    b.Property<int?>("SellerId");

                    b.Property<string>("SellerName");

                    b.Property<int?>("ShapeId");

                    b.Property<string>("StockNumber");

                    b.Property<decimal>("TotalAmountPaidToSeller");

                    b.Property<decimal>("TotalCost");

                    b.Property<int?>("VarietyId");

                    b.Property<double>("Width");

                    b.HasKey("Id");

                    b.HasIndex("GemLotId");

                    b.HasIndex("SellerId");

                    b.HasIndex("ShapeId");

                    b.HasIndex("VarietyId");

                    b.ToTable("Gems");
                });

            modelBuilder.Entity("GemSto.Domain.GemExport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ExportId");

                    b.Property<int>("GemId");

                    b.HasKey("Id");

                    b.HasIndex("ExportId");

                    b.HasIndex("GemId");

                    b.ToTable("GemExports");
                });

            modelBuilder.Entity("GemSto.Domain.GemLot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("BrokerFee");

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<int>("PaymentStatusToSeller");

                    b.Property<decimal>("TotalAmountPaidToSeller");

                    b.Property<decimal>("TotalCost");

                    b.HasKey("Id");

                    b.ToTable("GemLots");
                });

            modelBuilder.Entity("GemSto.Domain.LookUp.CertificateProvider", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("CertificateProviders");
                });

            modelBuilder.Entity("GemSto.Domain.LookUp.Colour", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Colours");
                });

            modelBuilder.Entity("GemSto.Domain.LookUp.Origin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("Origins");
                });

            modelBuilder.Entity("GemSto.Domain.LookUp.Seller", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PhoneNumber");

                    b.HasKey("Id");

                    b.ToTable("Sellers");
                });

            modelBuilder.Entity("GemSto.Domain.LookUp.Shape", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("ImagePath");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Shapes");
                });

            modelBuilder.Entity("GemSto.Domain.LookUp.Variety", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Varieties");
                });

            modelBuilder.Entity("GemSto.Domain.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedById");

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<string>("EditedById");

                    b.Property<DateTimeOffset?>("EditedOn");

                    b.Property<int>("GemId");

                    b.Property<int?>("GemStatus");

                    b.Property<decimal>("PaidAmount");

                    b.Property<DateTimeOffset?>("PaidOn");

                    b.Property<int>("Remark");

                    b.Property<int>("TransactionType");

                    b.HasKey("Id");

                    b.HasIndex("GemId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("GemSto.Domain.User.StoreUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTimeOffset>("DOB");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("IsAdmin");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("TelephoneLineTwo");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("GemSto.Domain.User.UserAudit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AuditEvent");

                    b.Property<string>("IpAddress");

                    b.Property<DateTimeOffset>("Timestamp");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("UserAudits");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("GemSto.Domain.Certificate", b =>
                {
                    b.HasOne("GemSto.Domain.LookUp.CertificateProvider", "CertificateProvider")
                        .WithMany()
                        .HasForeignKey("CertificateProviderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GemSto.Domain.LookUp.Colour", "Colour")
                        .WithMany()
                        .HasForeignKey("ColourId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GemSto.Domain.Gem", "Gem")
                        .WithMany("Certificates")
                        .HasForeignKey("GemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GemSto.Domain.LookUp.Origin", "Origin")
                        .WithMany()
                        .HasForeignKey("OriginId");

                    b.HasOne("GemSto.Domain.Transaction", "Transaction")
                        .WithMany()
                        .HasForeignKey("TransactionId");
                });

            modelBuilder.Entity("GemSto.Domain.Gem", b =>
                {
                    b.HasOne("GemSto.Domain.GemLot", "GemLot")
                        .WithMany("Gems")
                        .HasForeignKey("GemLotId");

                    b.HasOne("GemSto.Domain.LookUp.Seller", "Seller")
                        .WithMany()
                        .HasForeignKey("SellerId");

                    b.HasOne("GemSto.Domain.LookUp.Shape", "Shape")
                        .WithMany()
                        .HasForeignKey("ShapeId");

                    b.HasOne("GemSto.Domain.LookUp.Variety", "Variety")
                        .WithMany()
                        .HasForeignKey("VarietyId");
                });

            modelBuilder.Entity("GemSto.Domain.GemExport", b =>
                {
                    b.HasOne("GemSto.Domain.Export", "Export")
                        .WithMany("GemExports")
                        .HasForeignKey("ExportId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GemSto.Domain.Gem", "Gem")
                        .WithMany("GemExports")
                        .HasForeignKey("GemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GemSto.Domain.Transaction", b =>
                {
                    b.HasOne("GemSto.Domain.Gem", "Gem")
                        .WithMany("Transactions")
                        .HasForeignKey("GemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("GemSto.Domain.User.StoreUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("GemSto.Domain.User.StoreUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GemSto.Domain.User.StoreUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("GemSto.Domain.User.StoreUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

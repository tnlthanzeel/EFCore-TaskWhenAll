using GemSto.Domain;
using GemSto.Domain.LookUp;
using GemSto.Domain.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Data
{
    public class GemStoContext : IdentityDbContext<StoreUser>
    {
        public GemStoContext(DbContextOptions<GemStoContext> options) : base(options) { }
        public DbSet<Gem> Gems { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Colour> Colours { get; set; }
        public DbSet<CertificateProvider> CertificateProviders { get; set; }
        public DbSet<Variety> Varieties { get; set; }
        public DbSet<Export> Exports { get; set; }
        public DbSet<GemExport> GemExports { get; set; }
        public DbSet<Shape> Shapes { get; set; }
        public DbSet<GemLot> GemLots { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<UserAudit> UserAudits { get; set; }
        public DbSet<Origin> Origins { get; set; }
        public DbSet<Certification> Certifications { get; set; }
        public DbSet<ThirdPartyCertificate> ThirdPartyCertificates { get; set; }
        public DbSet<Approval> Approvals { get; set; }
        public DbSet<GemApproval> GemApprovals { get; set; }
        public DbSet<Approver> Approvers { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SalePayment> SalePayments { get; set; }
        public DbSet<GemSales> GemSales { get; set; }
        public DbSet<GemHistory> GemHistory { get; set; }

        public DbSet<SellerLotPayment> SellerLotPayment { get; set; }

        public DbSet<BuyerLotPayment> BuyerLotPayment { get; set; }

        public DbSet<MiscCategory> MiscCategory { get; set; }

        public DbSet<MiscSubCategory> MiscSubCategories { get; set; }

        public DbSet<Participant> Participants { get; set; }

        public DbSet<MiscPayments> MiscPayments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Gem>()
                .HasIndex(u => u.StockNumber)
                .IsUnique();

            modelBuilder.Entity<Sale>()
                .HasIndex(u => u.Number)
                .IsUnique();

            modelBuilder.Entity<GemSales>()
               .HasIndex(u => u.SaleNumber)
               .IsUnique();
        }
    }
}

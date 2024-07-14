using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace e_commmerce.Entities;

public partial class ShopContext : DbContext
{
    public ShopContext()
    {
    }

    public ShopContext(DbContextOptions<ShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<BillingAddress> BillingAddresses { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-MC950J7\\NVMINH;Database=shop;User Id=sa;Password=Minh1213@;TrustServerCertificate=True;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Uid).HasName("PK__Accounts__C5B69A4AFC105B6D");

            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Pass).HasColumnName("pass");
        });

        modelBuilder.Entity<BillingAddress>(entity =>
        {
            entity.ToTable("BillingAddress");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.City).HasMaxLength(250);
            entity.Property(e => e.CompanyName).HasMaxLength(250);
            entity.Property(e => e.Country).HasMaxLength(250);
            entity.Property(e => e.County).HasMaxLength(250);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(250);
            entity.Property(e => e.LastName).HasMaxLength(250);
            entity.Property(e => e.Phone).HasMaxLength(250);
            entity.Property(e => e.Streetaddress).HasMaxLength(250);

            entity.HasOne(d => d.AccountU).WithMany(p => p.BillingAddresses)
                .HasForeignKey(d => d.AccountUid)
                .HasConstraintName("FK_BillingAddress_Accounts");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Cid).HasName("PK__Categori__C1FFD861C159A1EA");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC07A2793DA3");

            entity.Property(e => e.CateId).HasColumnName("cateID");
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.Cate).WithMany(p => p.Products)
                .HasForeignKey(d => d.CateId)
                .HasConstraintName("FK__Products__cateID__3B75D760");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

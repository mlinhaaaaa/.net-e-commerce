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

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<New> News { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-DITISJL\\MYSQL;user id=sa; password= hongphat@3008;Database=shop; Trusted_Connection=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Uid);

            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Cid);

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<New>(entity =>
        {
            entity.HasKey(e => e.IdN);

            entity.ToTable("New");

            entity.Property(e => e.IdN).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(200);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.CateId).HasColumnName("CateID");
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.Cate).WithMany(p => p.Products)
                .HasForeignKey(d => d.CateId)
                .HasConstraintName("FK_Products_Categories");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

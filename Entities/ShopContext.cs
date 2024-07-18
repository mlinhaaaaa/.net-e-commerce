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

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-DITISJL\\MYSQL;user id=sa; password= hongphat@3008;Database=shop; Trusted_Connection=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Uid);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Cid);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.IdN);

            entity.Property(e => e.TimeCreate).HasColumnType("datetime");

            entity.HasOne(d => d.Cate).WithMany(p => p.News)
                .HasForeignKey(d => d.CateId)
                .HasConstraintName("FK_News_Categories");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.CateId).HasColumnName("CateID");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Cate).WithMany(p => p.Products)
                .HasForeignKey(d => d.CateId)
                .HasConstraintName("FK_Products_Categories");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

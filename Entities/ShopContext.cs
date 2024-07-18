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

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Checkout> Checkouts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ZERO_ONE;Database=shop;User Id=sa; Password=77882664; Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Accounts__C5B69A4AFC105B6D");

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.ConfirmPass).HasColumnName("confirmPass");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("lastName");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK_CartItem");

            entity.ToTable("Cart");

            entity.Property(e => e.CartId).HasColumnName("cartID");
            entity.Property(e => e.ProdId)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("prodID");
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Prod).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProdId)
                .HasConstraintName("FK_Cartitem_Products");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Accounts");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CateId).HasName("PK__Categori__C1FFD861C763F1AB");

            entity.Property(e => e.CateId).HasColumnName("cateID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Checkout>(entity =>
        {
            entity.HasKey(e => e.CheckId);

            entity.ToTable("Checkout");

            entity.Property(e => e.CheckId).HasColumnName("checkID");
            entity.Property(e => e.CartId).HasColumnName("cartID");
            entity.Property(e => e.CheckoutDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("checkoutDate");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("money")
                .HasColumnName("totalAmount");

            entity.HasOne(d => d.Cart).WithMany(p => p.Checkouts)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Checkout_CartItem");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProdId).HasName("PK__Products__3214EC0702C7E6EA");

            entity.Property(e => e.ProdId).HasColumnName("prodID");
            entity.Property(e => e.CateId).HasColumnName("cateID");
            entity.Property(e => e.ImgUrl).HasColumnName("imgUrl");
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.Cate).WithMany(p => p.Products)
                .HasForeignKey(d => d.CateId)
                .HasConstraintName("FK__Products__cateID__45F365D3");
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(e => e.WishId);

            entity.ToTable("Wishlist");

            entity.Property(e => e.WishId)
                .ValueGeneratedNever()
                .HasColumnName("wishID");
            entity.Property(e => e.ProdId).HasColumnName("prodID");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Prod).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.ProdId)
                .HasConstraintName("FK_Wishlist_Products");

            entity.HasOne(d => d.User).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Wishlist_Accounts");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

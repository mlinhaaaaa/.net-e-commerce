-- Viết database

-- thay đổi:
 //ShopContext
     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=**DESKTOP-MC950J7\\NVMINH**;Database=**shop**;User Id=**sa**;Password=*******;TrustServerCertificate=True;Trusted_Connection=True;");
-- run project

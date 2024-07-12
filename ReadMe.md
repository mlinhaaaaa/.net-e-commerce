-- Viết database
-- Create Categories table
CREATE TABLE dbo.Categories (
    Cid INT IDENTITY(1,1) NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    CONSTRAINT PK_Categories PRIMARY KEY CLUSTERED (Cid)
);
GO

-- Create Products table
CREATE TABLE dbo.Products (
    Id INT IDENTITY(1,1) NOT NULL,
    Name NVARCHAR(MAX) NOT NULL,
    ImagePath NVARCHAR(MAX) NOT NULL,
    Price MONEY NOT NULL,
    Size NVARCHAR(MAX) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    CateID INT NULL,
    CONSTRAINT PK_Products PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_Products_Categories FOREIGN KEY (CateID) REFERENCES dbo.Categories (Cid)
);
GO

-- Create Accounts table
CREATE TABLE dbo.Accounts (
    Uid INT IDENTITY(1,1) NOT NULL,
    FirstName NVARCHAR(50) NULL,
    LastName NVARCHAR(50) NULL,
    [User] NVARCHAR(MAX) NOT NULL,
    [Pass] NVARCHAR(MAX) NOT NULL,
    ConfirmPassword NVARCHAR(MAX) NULL,
    IsAdmin INT NOT NULL,
    CONSTRAINT PK_Accounts PRIMARY KEY CLUSTERED (Uid)
);
GO
-- thay đổi:
/appsettings.json/
 "ConnectionStrings": {
   "ConnectionString": "Server=;Database=shop;Trusted_Connection=True; User Id=; Password=;TrustServerCertificate=True"
 },

 //
     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=**DESKTOP-MC950J7\\NVMINH**;Database=**shop**;User Id=**sa**;Password=*******;TrustServerCertificate=True;Trusted_Connection=True;");
-- run project

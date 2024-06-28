create database shop
go

use shop
go

create table Categories
(
	Cid INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
)

Create table Accounts
(
	Uid INT PRIMARY KEY IDENTITY(1,1),
    [User] NVARCHAR(MAX) NOT NULL,
	pass NVARCHAR(MAX) NOT NULL,
	IsAdmin int NOT NULL
)
create table Products
(
	Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(Max) NOT NULL,
	ImagePath NVARCHAR(Max) NOT NULL,
	Price Money NOT NULL,
	Size NVARCHAR(Max) NOT NULL,
	Description NVARCHAR(Max) NOT NULL,
	cateID int,
	FOREIGN KEY (cateID) REFERENCES Categories(Cid),
)

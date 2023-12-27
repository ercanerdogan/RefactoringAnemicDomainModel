Create table Customer (
	CustomerId int,
	Name varchar(max),
	Email varchar(max),
	Status int,
	StatusExpirationDate DateTime,
	MoneySpent decimal,

);


Create table Movie (
	MovieId int,
	Name varchar(max),
	LicensingModel int
);

Create table PurchasedMovie(
	PurchasedMovieId int,
	MovieId int,
	CustomerId int,
	Price decimal,
	PurchaseDate DateTime,
	ExpirationDate DateTime
);



  
insert into Movie ([MovieId],[Name] ,[LicensingModel]) values(1, 'Test1',1);
insert into Movie ([MovieId],[Name] ,[LicensingModel]) values(2, 'Test2',1);
insert into Movie ([MovieId],[Name] ,[LicensingModel]) values(3, 'Test3',1);
insert into Movie ([MovieId],[Name] ,[LicensingModel]) values(4, 'Test3',1);


insert into Customer ([CustomerId], [Name],[Email], [Status]) values(1, 'Test Customer1', 'a@a.com', 1)
insert into Customer ([CustomerId], [Name],[Email], [Status]) values(2, 'Test Customer2', 'b@a.com', 1)
insert into Customer ([CustomerId], [Name],[Email], [Status]) values(3, 'Test Customer3', 'c@a.com', 1)
insert into Customer ([CustomerId], [Name],[Email], [Status]) values(4, 'Test Customer4', 'd@a.com', 1)

insert into [dbo].[PurchasedMovie] ([PurchasedMovieId], [MovieId], [CustomerId], [Price], [PurchaseDate], [ExpirationDate]) values (1, 1, 1, 10,  getdate(), GETDATE()+1);
insert into [dbo].[PurchasedMovie] ([PurchasedMovieId], [MovieId], [CustomerId], [Price], [PurchaseDate], [ExpirationDate]) values (2, 1, 2, 10,  getdate(), GETDATE()+1);
insert into [dbo].[PurchasedMovie] ([PurchasedMovieId], [MovieId], [CustomerId], [Price], [PurchaseDate], [ExpirationDate]) values (3, 1, 3, 10,  getdate(), GETDATE()+1);
insert into [dbo].[PurchasedMovie] ([PurchasedMovieId], [MovieId], [CustomerId], [Price], [PurchaseDate], [ExpirationDate]) values (4, 2, 1, 10,  getdate(), GETDATE()+1);
insert into [dbo].[PurchasedMovie] ([PurchasedMovieId], [MovieId], [CustomerId], [Price], [PurchaseDate], [ExpirationDate]) values (5, 3, 1, 10,  getdate(), GETDATE()+1);
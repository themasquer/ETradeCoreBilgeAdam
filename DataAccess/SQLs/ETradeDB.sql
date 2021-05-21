delete from ETradeProducts
delete from ETradeCategories
delete from ETradeUsers
delete from ETradeRoles
delete from ETradeUserDetails
delete from ETradeCities
delete from ETradeCountries

SET IDENTITY_INSERT [dbo].[ETradeCategories] ON 
INSERT [dbo].[ETradeCategories] ([Id], [Name]) VALUES (1, N'Computer')
INSERT [dbo].[ETradeCategories] ([Id], [Name], [Description]) VALUES (2, N'Home Entertainment', 'Home theater systems and TVs.')
SET IDENTITY_INSERT [dbo].[ETradeCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[ETradeProducts] ON 
INSERT [dbo].[ETradeProducts] ([Id], [Name], [UnitPrice], [StockAmount], [CategoryId], [ExpirationDate]) VALUES (1, N'Laptop', 3000.0000, 10, 1, '2020-12-13')
INSERT [dbo].[ETradeProducts] ([Id], [Name], [UnitPrice], [StockAmount], [CategoryId], [ExpirationDate], [Description]) VALUES (2, N'Mouse', 20.0000, 20, 1, '2021-11-23', 'Computer peripherals.')
INSERT [dbo].[ETradeProducts] ([Id], [Name], [UnitPrice], [StockAmount], [CategoryId], [Description]) VALUES (3, N'Keyboard', 40.0000, 21, 1, 'Computer peripherals.')
INSERT [dbo].[ETradeProducts] ([Id], [Name], [UnitPrice], [StockAmount], [CategoryId]) VALUES (18, N'Speaker', 2500.0000, 5, 2)
INSERT [dbo].[ETradeProducts] ([Id], [Name], [UnitPrice], [StockAmount], [CategoryId]) VALUES (19, N'Receiver', 5000.0000, 9, 2)
INSERT [dbo].[ETradeProducts] ([Id], [Name], [UnitPrice], [StockAmount], [CategoryId], [Description]) VALUES (23, N'Monitor', 2500.0000, 27, 1, 'Computer peripherals.')
INSERT [dbo].[ETradeProducts] ([Id], [Name], [UnitPrice], [StockAmount], [CategoryId]) VALUES (24, N'Equalizer', 1000.0000, 11, 2)
SET IDENTITY_INSERT [dbo].[ETradeProducts] OFF
GO
set identity_insert ETradeRoles on
insert into ETradeRoles (Id, Name) values (1, 'Admin')
insert into ETradeRoles (Id, Name) values (2, 'User')
set identity_insert ETradeRoles off
go
set identity_insert ETradeCountries on
insert into ETradeCountries (Id, Name) values (1, 'Turkey')
insert into ETradeCountries (Id, Name) values (2, 'United States')
set identity_insert ETradeCountries off
go
set identity_insert ETradeCities on
insert into ETradeCities (Id, Name, CountryId) values (1, 'Ankara', 1)
insert into ETradeCities (Id, Name, CountryId) values (2, 'Istanbul', 1)
insert into ETradeCities (Id, Name, CountryId) values (3, 'Izmir', 1)
insert into ETradeCities (Id, Name, CountryId) values (4, 'New York', 2)
set identity_insert ETradeCities off
go
set identity_insert ETradeUserDetails on
insert into ETradeUserDetails (Id, EMail, CountryId, CityId, Address) values (1, 'cagil@alsac.com', 1, 1, 'Çankaya')
insert into ETradeUserDetails (Id, EMail, CountryId, CityId, Address) values (2, 'leo@alsac.com', 2, 4, 'Manhattan')
set identity_insert ETradeUserDetails off
go
set identity_insert ETradeUsers on
insert into ETradeUsers (Id, UserName, Password, Active, RoleId, UserDetailId) values (1, 'cagil', 'cagil', 1, 1, 1)
insert into ETradeUsers (Id, UserName, Password, Active, RoleId, UserDetailId) values (2, 'leo', 'leo', 1, 2, 2)
set identity_insert ETradeUsers off
go
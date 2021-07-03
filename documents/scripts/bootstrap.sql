-- use master;
-- go
-- ALTER DATABASE  [ProductsInventory]
-- SET SINGLE_USER
-- WITH ROLLBACK IMMEDIATE
-- drop database [ProductsInventory]
-- go
------ normal creation after here
use master;
go
if not exists (select name from master..syslogins where name = 'ProductsInventoryWeb')
    begin
        create login ProductsInventoryWeb with password = 'P4$$w0rd';
    end;
go


if not exists (select name from master..sysdatabases where name = 'ProductsInventory')
begin
create database ProductsInventory
end;
GO

use ProductsInventory
if not exists (select * from sysusers where name = 'ProductsInventoryWeb')
begin
create user ProductsInventoryWeb
	for login ProductsInventoryWeb
	with default_schema = dbo
end;
GO
grant connect to ProductsInventoryWeb
go
exec sp_addrolemember N'db_datareader', N'ProductsInventoryWeb';
go
exec sp_addrolemember N'db_datawriter', N'ProductsInventoryWeb';
go
exec sp_addrolemember N'db_owner', N'ProductsInventoryWeb';
GO
use master;
GO


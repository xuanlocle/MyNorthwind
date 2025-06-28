-- Initialize Northwind Database
USE master;
GO

-- Create Northwind database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Northwind')
BEGIN
    CREATE DATABASE Northwind;
END
GO

USE Northwind;
GO

-- Create DeviceTokens table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DeviceTokens')
BEGIN
    CREATE TABLE DeviceTokens (
        DeviceTokenId INT IDENTITY(1,1) PRIMARY KEY,
        Token NVARCHAR(500) NOT NULL,
        RegisteredAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

-- Create Customers table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Customers')
BEGIN
    CREATE TABLE Customers (
        CustomerId NVARCHAR(5) PRIMARY KEY,
        CompanyName NVARCHAR(40) NOT NULL,
        ContactName NVARCHAR(30),
        ContactTitle NVARCHAR(30),
        Address NVARCHAR(60),
        City NVARCHAR(15),
        Region NVARCHAR(15),
        PostalCode NVARCHAR(10),
        Country NVARCHAR(15),
        Phone NVARCHAR(24),
        Fax NVARCHAR(24)
    );
END
GO

-- Create Orders table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Orders')
BEGIN
    CREATE TABLE Orders (
        OrderId INT IDENTITY(1,1) PRIMARY KEY,
        CustomerId NVARCHAR(5),
        OrderDate DATETIME2,
        RequiredDate DATETIME2,
        ShippedDate DATETIME2,
        ShipVia INT,
        Freight DECIMAL(18,2),
        ShipName NVARCHAR(40),
        ShipAddress NVARCHAR(60),
        ShipCity NVARCHAR(15),
        ShipRegion NVARCHAR(15),
        ShipPostalCode NVARCHAR(10),
        ShipCountry NVARCHAR(15),
        CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
    );
END
GO

-- Insert sample data if tables are empty
IF NOT EXISTS (SELECT TOP 1 * FROM Customers)
BEGIN
    INSERT INTO Customers (CustomerId, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax)
    VALUES 
        ('ALFKI', 'Alfreds Futterkiste', 'Maria Anders', 'Sales Representative', 'Obere Str. 57', 'Berlin', NULL, '12209', 'Germany', '030-0074321', '030-0076545'),
        ('ANATR', 'Ana Trujillo Emparedados y helados', 'Ana Trujillo', 'Owner', 'Avda. de la Constitución 2222', 'México D.F.', NULL, '05021', 'Mexico', '(5) 555-4729', '(5) 555-3745'),
        ('ANTON', 'Antonio Moreno Taquería', 'Antonio Moreno', 'Owner', 'Mataderos  2312', 'México D.F.', NULL, '05023', 'Mexico', '(5) 555-3932', NULL);
END
GO

IF NOT EXISTS (SELECT TOP 1 * FROM Orders)
BEGIN
    INSERT INTO Orders (CustomerId, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry)
    VALUES 
        ('ALFKI', '2024-01-15', '2024-01-22', '2024-01-16', 1, 29.46, 'Alfreds Futterkiste', 'Obere Str. 57', 'Berlin', NULL, '12209', 'Germany'),
        ('ANATR', '2024-01-16', '2024-01-23', '2024-01-18', 1, 3.25, 'Ana Trujillo Emparedados y helados', 'Avda. de la Constitución 2222', 'México D.F.', NULL, '05021', 'Mexico'),
        ('ANTON', '2024-01-17', '2024-01-24', '2024-01-19', 2, 11.61, 'Antonio Moreno Taquería', 'Mataderos  2312', 'México D.F.', NULL, '05023', 'Mexico');
END
GO

PRINT 'Northwind database initialized successfully!';
GO 
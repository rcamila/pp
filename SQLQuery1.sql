CREATE DATABASE PPDB;
USE PPDB;

CREATE TABLE Provincias(
PciaId INT PRIMARY KEY,
DescPcia VARCHAR (50));

CREATE TABLE Localidades(
LocalId INT NOT NULL PRIMARY KEY,
descLocal VARCHAR (50),
Pcia INT NOT NULL FOREIGN KEY REFERENCES Provincias(PciaId),
mail VARCHAR (100));

CREATE TABLE Clientes(
ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
Nombre VARCHAR (40),
Apellido VARCHAR (40),
FechaAlta DATE,
CP INT NOT NULL FOREIGN KEY REFERENCES Localidades(LocalId)
mail VARCHAR (100));

SELECT *  FROM Clientes;

CREATE TABLE Categorias(
catId INT NOT NULL PRIMARY KEY,
desccat VARCHAR (30)
mail VARCHAR (100));

SELECT * FROM Localidades;

SELECT cat from Usuarios WHERE cat = 2

--ALTER TABLE Categorias ADD Email VARCHAR (100)

CREATE TABLE Usuarios(
ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
Nombre VARCHAR (40),
Apellido VARCHAR (40),
mail VARCHAR (40),
pw VARCHAR (20),
cat INT NOT NULL FOREIGN KEY REFERENCES Categorias(catId));
SELECT * FROM Clientes
SELECT ID, Nombre, Apellido, mail, pw, cat, desccat  FROM Usuarios INNER JOIN Categorias ON cat = catId;
DELETE FROM Clientes WHERE ID = 2;
SELECT TOP 1  ID FROM Historico ORDER BY Fecha desc;
UPDATE Historico SET Usuario = 'aaa' WHERE ID = 42;
UPDATE Historico SET mail = 'administrador' WHERE ID = 3;
SELECT * FROM Historico;


CREATE TABLE Historico(
ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
NombTabla VARCHAR (25),
Accion VARCHAR (20),
Fecha DATE,
Usuario VARCHAR (25),
Terminal VARCHAR (35)); 






CREATE DATABASE SmartPhone7;

USE SmartPhone7;
go
Create TABLE Clientes(
  Id INT IDENTITY(1,1) PRIMARY KEY,
  NombreCliente VARCHAR(50) NOT NULL,
  Telefono VARCHAR(20) NOT NULL,
  MarcaModelo VARCHAR(50) NOT NULL,
  SerieIMEI VARCHAR(50) NULL,
  Diagnostico VARCHAR(200) NULL
);

CREATE TABLE Producto (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  NombreProducto VARCHAR(50) NOT NULL,
  ModeloMarca VARCHAR(50) NOT NULL,
  DescripcionProducto VARCHAR(200) NULL,
  Codigo VARCHAR(50) NOT NULL,
  Stock INT NOT NULL,
  PrecioCompra DECIMAL(10,2) NOT NULL,
  PrecioVenta DECIMAL(10,2) NOT NULL,
  Nota VARCHAR(200) NULL
);

INSERT INTO Producto (NombreProducto, ModeloMarca, DescripcionProducto, Codigo, Stock, PrecioCompra, PrecioVenta, Nota) VALUES
('Smartphone', 'Samsung Galaxy S21', 'Color: Negro, 256 GB', 'SAMS21-256BK', 50, 800.00, 1100.00, NULL),
('Laptop', 'Dell XPS 13', 'Color: Plata, 512 GB', 'DELLXPS13-512SV', 20, 1200.00, 1500.00, 'Incluye adaptador USB-C a HDMI'),
('Smartwatch', 'Apple Watch Series 6', 'Color: Rojo, GPS + Celular', 'APW6-RED-GC', 30, 500.00, 700.00, NULL),
('Audifonos', 'Sony WH-1000XM4', 'Color: Negro, Cancelación de Ruido', 'SNYWH1000XM4-BK', 40, 250.00, 350.00, 'Incluye estuche de transporte');

CREATE TABLE Usuario (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  Nombre VARCHAR(50) NOT NULL,
  Correo VARCHAR(50) NOT NULL,
  Rol VARCHAR(50) NOT NULL,
  NombreUsuario VARCHAR(50) NOT NULL,
  Contrasena VARCHAR(50) NOT NULL,
);


Create TABLE Tecnicos (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  NombreTecnico varchar(30) not null,
  Telefono VARCHAR(20) NOT NULL,
  Cedula VARCHAR(50) NOT NULL,
  FechaEntrada date NOT NULL,
  Direccion VARCHAR(200) NOT NULL,
  CorreoElectronico VARCHAR(50) NULL,
  PersonaContacto VARCHAR(50)  NULL,
  TelefonoPersonaContacto VARCHAR(20) NULL
);

CREATE TABLE Factura (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  IdCliente INT NOT NULL,
  FOREIGN KEY (IdCliente) REFERENCES Clientes(Id),
  NumeroOrden VARCHAR(50) NOT NULL,
  FechaFactura DATE NOT NULL,
  Falla VARCHAR(200) NULL,
  DiagnosticoFinal VARCHAR(200) NULL,
  EstadoOrden VARCHAR(50) NOT NULL,
  IdTecnico INT NOT NULL,
  FOREIGN KEY (IdTecnico) REFERENCES Tecnicos(Id),
  ServicioOfrecido VARCHAR(200) NULL,
  ManoDeObra DECIMAL(10,2) NULL,
  ITEBIS DECIMAL(10,2) NULL,
  Descuento DECIMAL(10,2) NULL,
  Subtotal DECIMAL(10,2) NULL,
  Total DECIMAL(10,2) NULL,
  Nota VARCHAR(200) NULL
);

CREATE TABLE DetalleFactura (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  IdFactura INT NOT NULL,
  FOREIGN KEY (IdFactura) REFERENCES Factura(Id),
  IdProducto INT NOT NULL,
  Cantidad INT NOT NULL,
  Precio DECIMAL(10,2) NOT NULL,
  Subtotal DECIMAL(10,2) NOT NULL
);



CREATE TABLE Servicios (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  NombreServicio VARCHAR(50) NOT NULL,
  Codigo VARCHAR(50) NOT NULL,
  Precio DECIMAL(10,2) NOT NULL,
  Descripcion VARCHAR(200) NULL
);



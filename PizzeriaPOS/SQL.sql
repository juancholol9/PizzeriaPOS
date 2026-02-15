CREATE DATABASE PizzeriaPOS

USE PizzeriaPOS

CREATE TABLE Categoria (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(255),
    Activa BIT DEFAULT 1
);

CREATE TABLE Producto (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CategoriaId INT NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(255),
    Precio DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL,
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Producto_Categoria
        FOREIGN KEY (CategoriaId)
        REFERENCES Categoria(Id)
);

CREATE TABLE Cliente (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20),
    Email VARCHAR(100),
    FechaRegistro DATETIME DEFAULT GETDATE()
);

CREATE TABLE Direccion (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ClienteId INT NOT NULL,
    Calle VARCHAR(150) NOT NULL,
    Ciudad VARCHAR(100) NOT NULL,
    Referencia VARCHAR(255),
    Activa BIT DEFAULT 1,

    CONSTRAINT FK_Direccion_Cliente
        FOREIGN KEY (ClienteId)
        REFERENCES Cliente(Id)
        ON DELETE CASCADE
);

CREATE TABLE Usuario (
    Id INT PRIMARY KEY IDENTITY(1,1),
    NombreUsuario VARCHAR(50) NOT NULL UNIQUE,
    Email VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Rol VARCHAR(50) NOT NULL DEFAULT 'Empleado',
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE()
);

SELECT * FROM Usuario

CREATE TABLE Pedido (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ClienteId INT NOT NULL,
    DireccionId INT NOT NULL,
    UsuarioId INT NOT NULL,
    Fecha DATETIME DEFAULT GETDATE(),
    Total DECIMAL(10,2) NOT NULL,
    Estado VARCHAR(50) DEFAULT 'Pendiente',

    CONSTRAINT FK_Pedido_Cliente
        FOREIGN KEY (ClienteId)
        REFERENCES Cliente(Id),

    CONSTRAINT FK_Pedido_Direccion
        FOREIGN KEY (DireccionId)
        REFERENCES Direccion(Id),

    CONSTRAINT FK_Pedido_Usuario
        FOREIGN KEY (UsuarioId)
        REFERENCES Usuario(Id)
);

CREATE TABLE PedidoDetalle (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PedidoId INT NOT NULL,
    ProductoId INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL,
    SubTotal DECIMAL(10,2) NOT NULL,

    CONSTRAINT FK_PedidoDetalle_Pedido
        FOREIGN KEY (PedidoId)
        REFERENCES Pedido(Id)
        ON DELETE CASCADE,

    CONSTRAINT FK_PedidoDetalle_Producto
        FOREIGN KEY (ProductoId)
        REFERENCES Producto(Id)
);

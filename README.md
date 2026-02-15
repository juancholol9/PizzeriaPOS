# Laboratorio de Evaluación de Candidatos – Parte 1

Desarrollado por **Juan Mejia**

## Descripción del Proyecto

Este proyecto consiste en el desarrollo de una **API REST para un sistema POS de una pizzería**.

La API permite gestionar:

* Productos
* Clientes
* Direcciones
* Pedidos
* Colaboradores
* Autenticación de usuarios

El repositorio incluye todos los commits realizados durante el desarrollo.

---

## Pasos para Ejecutar el Proyecto

### Clonar el repositorio

```bash
git clone https://github.com/juancholol9/PizzeriaPOS.git
```

---

### Abrir la solución

1. Ingresar a la carpeta `PizzeriaPOS`
2. Abrir el archivo:

```
api-pos-pizza.sln
```

Esto abrirá el proyecto en Visual Studio.

---

### Configurar la Base de Datos

Antes de ejecutar la API:

1. Ir a la carpeta `esquema`
2. Abrir el archivo `sql.txt`
3. Ejecutar su contenido en SQL Server

Esto creará la base de datos y todas las tablas necesarias.

---

### Crear un Colaborador

La API cuenta con autenticación, por lo tanto:

* Es necesario crear **al menos un colaborador**
* Esto permitirá generar credenciales válidas para realizar peticiones autenticadas

---

### Ejecutar el Proyecto

1. Ejecutar la API desde Visual Studio
2. Probar los endpoints usando Swagger o Postman

---

## Tecnologías Utilizadas

* .NET
* Entity Framework Core
* SQL Server
* Autenticación basada en tokens
* Swagger

---

## Notas Importantes

* Verificar la cadena de conexión en `appsettings.json`
* Asegurarse de que SQL Server esté en ejecución antes de iniciar la API

---

Proyecto desarrollado como parte de un laboratorio técnico para evaluación de candidatos.

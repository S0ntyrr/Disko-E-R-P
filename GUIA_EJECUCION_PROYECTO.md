# 🚀 Guía de Ejecución del Proyecto Disko ERP

## 📋 Requisitos Previos

Antes de ejecutar el proyecto, asegúrate de tener instalado:

- **.NET 8.0 SDK** o superior
- **Visual Studio 2022** (recomendado) o **VS Code**
- **SQL Server** (opcional - el proyecto funciona con localStorage si no hay BD)

### Verificar Instalación de .NET

```bash
dotnet --version
```

Debe mostrar la versión 8.0 o superior. Si no está instalado, descárgalo desde: https://dotnet.microsoft.com/download

---

## 🔧 Pasos para Ejecutar el Proyecto

### **Opción 1: Usando la Terminal/CMD (Recomendado)**

#### **1. Navegar al directorio del proyecto**

```bash
cd LOGIN/USUARIO/SEGURIDAD
```

#### **2. Restaurar dependencias NuGet**

```bash
dotnet restore
```

Este comando descarga todos los paquetes NuGet necesarios definidos en `DiskoERP.csproj`.

#### **3. Compilar el proyecto**

```bash
dotnet build
```

Verifica que no haya errores de compilación.

#### **4. Ejecutar el proyecto**

```bash
dotnet run
```

El proyecto se iniciará y mostrará algo como:

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

#### **5. Abrir en el navegador**

Abre tu navegador y ve a:
- **HTTP:** http://localhost:5000
- **HTTPS:** https://localhost:5001

**Nota:** La primera vez que uses HTTPS, el navegador mostrará una advertencia de certificado (es normal en desarrollo). Haz clic en "Continuar" o "Avanzado" y luego "Continuar al sitio".

---

### **Opción 2: Usando Visual Studio**

#### **1. Abrir el proyecto**

1. Abre Visual Studio 2022
2. Selecciona **File > Open > Project/Solution**
3. Navega a la carpeta `LOGIN/USUARIO/SEGURIDAD`
4. Selecciona el archivo `DiskoERP.csproj`

#### **2. Restaurar paquetes**

Visual Studio restaurará automáticamente los paquetes NuGet. Si no lo hace:
- Click derecho en el proyecto > **Restore NuGet Packages**

#### **3. Ejecutar**

Presiona **F5** o haz clic en el botón **▶ IIS Express** (o **▶ DiskoERP**)

---

### **Opción 3: Usando Visual Studio Code**

#### **1. Abrir el proyecto**

```bash
code LOGIN/USUARIO/SEGURIDAD
```

#### **2. Instalar extensiones (si no las tienes)**

- **C#** (Microsoft)
- **C# Dev Kit** (Opcional pero recomendado)

#### **3. Restaurar y ejecutar**

Presiona **Ctrl + Shift + P** y ejecuta:
- `dotnet: Restore Packages`
- Luego `dotnet: Run Project`

O desde la terminal integrada:

```bash
dotnet restore
dotnet run
```

---

## 🌐 URLs del Módulo de Inventario

Una vez que el proyecto esté ejecutándose, puedes acceder a las siguientes rutas:

### **Autenticación**
- **Login:** http://localhost:5000/Auth/Login
- **Registro:** http://localhost:5000/Auth/Register

### **Dashboard Principal**
- **Dashboard:** http://localhost:5000/Dashboard

### **Módulo de Inventario** 📦

- **Dashboard de Inventario:** 
  - http://localhost:5000/Inventario/Dashboard
  
- **Gestión de Productos:**
  - http://localhost:5000/Inventario/Index
  - http://localhost:5000/Producto/Index (alternativa)

- **Categorías:**
  - http://localhost:5000/Inventario/Categorias

- **Movimientos:**
  - http://localhost:5000/Inventario/Movimientos
  - http://localhost:5000/Inventario/Movimientos/Create

- **Crear Producto:**
  - http://localhost:5000/Producto/Create

---

## ⚙️ Configuración de Base de Datos (Opcional)

El proyecto funciona **100% sin base de datos** usando localStorage. Sin embargo, si quieres usar SQL Server:

### **1. Configurar Connection String**

Edita `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=DiskoERP;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

**Ajusta según tu configuración:**
- `localhost\\SQLEXPRESS` → Tu instancia de SQL Server
- `DiskoERP` → Nombre de tu base de datos

### **2. Crear la Base de Datos**

```bash
# En SQL Server Management Studio o usando sqlcmd
sqlcmd -S localhost\SQLEXPRESS -Q "CREATE DATABASE DiskoERP"
```

### **3. Ejecutar Migraciones (si las hay)**

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

O ejecuta el script SQL manualmente:
- `03_InsertSeedData.sql`

---

## 🐛 Solución de Problemas Comunes

### **Problema 1: Error "No se puede encontrar el proyecto"**

**Solución:**
```bash
# Asegúrate de estar en el directorio correcto
cd LOGIN/USUARIO/SEGURIDAD
pwd  # En Linux/Mac
cd   # En Windows
```

### **Problema 2: Error "No se pueden restaurar los paquetes"**

**Solución:**
```bash
# Limpiar cache de NuGet
dotnet nuget locals all --clear

# Restaurar nuevamente
dotnet restore
```

### **Problema 3: Puerto ya en uso**

**Error:** `Address already in use` o `The port is already in use`

**Solución:**

**Opción A:** Cambiar el puerto en `Properties/launchSettings.json`:

```json
{
  "applicationUrl": "http://localhost:5002;https://localhost:5003"
}
```

**Opción B:** Especificar el puerto al ejecutar:

```bash
dotnet run --urls "http://localhost:5002;https://localhost:5003"
```

### **Problema 4: Error de certificado SSL en HTTPS**

**Solución:**
- En desarrollo, puedes aceptar el certificado auto-firmado
- O ejecutar solo en HTTP:

```bash
dotnet run --urls "http://localhost:5000"
```

### **Problema 5: Error "No se encuentra el controlador InventarioController"**

**Solución:**
Asegúrate de que el archivo `Controllers/InventarioController.cs` existe y está compilado:

```bash
dotnet clean
dotnet build
dotnet run
```

### **Problema 6: Las vistas no se cargan**

**Solución:**
Verifica que las vistas estén en:
- `Views/Inventario/Dashboard.cshtml`
- `Views/Inventario/Index.cshtml`
- `Views/Inventario/Categorias.cshtml`
- `Views/Inventario/Movimientos.cshtml`
- `Views/Inventario/MovimientosCreate.cshtml`

---

## 📝 Comandos Útiles

### **Limpiar y Reconstruir**

```bash
dotnet clean
dotnet build
```

### **Ejecutar en modo Release**

```bash
dotnet run --configuration Release
```

### **Verificar que el proyecto compila sin ejecutarlo**

```bash
dotnet build --no-incremental
```

### **Listar todos los paquetes instalados**

```bash
dotnet list package
```

### **Actualizar paquetes NuGet**

```bash
dotnet add package [NombrePaquete] --version [Version]
```

---

## 🔍 Verificar que Todo Funciona

### **1. Verificar Compilación**

```bash
dotnet build
```

Debe mostrar:
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

### **2. Verificar que el Servidor Inicia**

```bash
dotnet run
```

Debe mostrar:
```
Now listening on: http://localhost:5000
Now listening on: https://localhost:5001
Application started.
```

### **3. Verificar en el Navegador**

1. Abre http://localhost:5000/Auth/Login
2. Deberías ver la página de login
3. Si ves errores 404, verifica las rutas en `program.cs`

---

## 🎯 Primer Uso - Crear Usuario

1. Ejecuta el proyecto: `dotnet run`
2. Abre http://localhost:5000/Auth/Register
3. Crea un usuario nuevo
4. Inicia sesión con ese usuario
5. Accede al Dashboard
6. Navega a **Inventario/Dashboard** para ver el módulo completo

---

## 📚 Estructura de Carpetas Importante

```
LOGIN/USUARIO/SEGURIDAD/
├── Controllers/
│   ├── InventarioController.cs    ← Nuevo controlador
│   ├── ProductoController.cs
│   └── ...
├── Views/
│   ├── Inventario/                ← Nuevas vistas
│   │   ├── Dashboard.cshtml
│   │   ├── Index.cshtml
│   │   ├── Categorias.cshtml
│   │   ├── Movimientos.cshtml
│   │   └── MovimientosCreate.cshtml
│   └── ...
├── wwwroot/
│   └── js/
│       └── inventario.js          ← JavaScript del módulo
├── program.cs                     ← Configuración de rutas
├── appsettings.json               ← Configuración
└── DiskoERP.csproj               ← Archivo del proyecto
```

---

## ✅ Checklist de Ejecución

- [ ] .NET 8.0 SDK instalado (`dotnet --version`)
- [ ] Navegado al directorio `LOGIN/USUARIO/SEGURIDAD`
- [ ] Ejecutado `dotnet restore`
- [ ] Ejecutado `dotnet build` (sin errores)
- [ ] Ejecutado `dotnet run`
- [ ] Navegador abierto en http://localhost:5000
- [ ] Página de login visible
- [ ] Módulo de inventario accesible en `/Inventario/Dashboard`

---

## 🆘 ¿Necesitas Ayuda?

Si encuentras algún error:

1. **Revisa los logs** en la consola donde ejecutaste `dotnet run`
2. **Abre la consola del navegador** (F12) para ver errores JavaScript
3. **Verifica que todas las vistas existan** en la carpeta `Views/Inventario/`
4. **Asegúrate de que `inventario.js` esté en `wwwroot/js/`**

---

**¡Listo! El proyecto debería estar ejecutándose correctamente. 🎉**

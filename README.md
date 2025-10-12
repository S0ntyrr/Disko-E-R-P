<p align="center">
<img src="https://capsule-render.vercel.app/api?type=waving&color=gradient&height=250&section=header&text=Disko%20E-R-P&fontSize=80&fontAlign=50&fontAlignY=30&animation=twinkling" />
</p>

<p align="center">
<a href="https://git.io/typing-svg"><img src="https://readme-typing-svg.demolab.com?font=Orbitron&size=30&pause=1000&color=5865F2&center=true&width=500&lines=Sistema+ERP+Moderno;Gestión+Integral+de+Negocios;Plataforma+de+Automatización" alt="Typing SVG" /></a>
</p>

<p align="center">
<img src="https://img.shields.io/badge/versión-1.0.0-blue" />
<img src="https://img.shields.io/badge/licencia-MIT-green" />
<img src="https://img.shields.io/badge/estado-desarrollo-yellow" />
<img src="https://img.shields.io/badge/plataforma-web-important" />
<img src="https://img.shields.io/badge/arquitectura-moderna-informational" />
</p>

<p align="center">
<img width="430" src="https://github-readme-stats.vercel.app/api?username=S0ntyrr&theme=dark&show_icons=true&hide_title=true&hide_border=true" />
<img width="400" src="https://streak-stats.demolab.com?user=S0ntyrr&theme=dark&hide_border=true" />
</p>

<p align="center">
<img align="center" src="https://github-readme-stats.vercel.app/api/top-langs/?username=S0ntyrr&theme=dark&hide_border=true&layout=compact&langs_count=6&hide=html,css" />
</p>

<p align="center">
<img width="800" src="https://github-readme-activity-graph.vercel.app/graph?username=S0ntyrr&theme=github-dark&hide_border=true&area=true" />
</p>

---

## 📊 Descripción del Proyecto

**Disko E-R-P** es un sistema ERP (Enterprise Resource Planning) moderno diseñado para ofrecer una solución integral de gestión empresarial. Desarrollado con tecnologías web actuales, proporciona una plataforma escalable y eficiente para la administración de todos los aspectos de un negocio.

### 🎯 Características Principales

| Módulo | Funcionalidades |
|--------|----------------|
| **📦 Gestión de Inventarios** | Control de stock, alertas, categorización |
| **💰 Ventas y Facturación** | POS, facturas electrónicas, reportes |
| **👥 Gestión de Clientes** | CRM, historial, segmentación |
| **📊 Contabilidad** | Estados financieros, balances, impuestos |
| **🧑‍💼 Recursos Humanos** | Nóminas, asistencia, perfiles |
| **📈 Analytics** | Dashboards, KPIs, business intelligence |

---

## 🛠️ Tecnologías Utilizadas

<p align="center">
<img src="https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
<img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white" />
<img src="https://img.shields.io/badge/Entity_Framework-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
<img src="https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white" />
<img src="https://img.shields.io/badge/Bootstrap-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white" />
<img src="https://img.shields.io/badge/JavaScript-F7DF1E?style=for-the-badge&logo=javascript&logoColor=black" />
</p>

---

## 🚀 Instalación y Configuración

### Prerrequisitos

- .NET 8.0 SDK o superior
- SQL Server 2019+ (opcional - el sistema funciona con localStorage)
- Visual Studio 2022 o VS Code

### 🔧 Instalación Paso a Paso

1. **Clonar el repositorio**
   ```bash
   git clone https://github.com/S0ntyrr/Disko-E-R-P.git
   cd Disko-E-R-P
   ```

2. **Navegar al proyecto principal**
   ```bash
   cd LOGIN/USUARIO/SEGURIDAD
   ```

3. **Restaurar dependencias**
   ```bash
   dotnet restore
   ```

4. **Configurar la base de datos (opcional)**
   
   Editar `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=DiskoERP;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```

5. **Aplicar migraciones (si usas BD)**
   ```bash
   dotnet ef database update
   ```

6. **Iniciar el servidor**
   ```bash
   dotnet run
   ```

7. **Acceder al sistema**
   
   Abre tu navegador en: `http://localhost:5000`

---

## 🎮 Uso del Sistema

### 📝 Credenciales Iniciales

Para comenzar, puedes registrar un nuevo usuario o usar:

- **Usuario:** `admin`
- **Contraseña:** `admin123`
- **Rol:** Administrador

### Módulos Disponibles

#### 1. **Dashboard Principal**

- Vista general del negocio
- Métricas clave en tiempo real
- Acceso rápido a todos los módulos

#### 2. **Gestión de Clientes** 👥

```javascript
// Ejemplo: Datos de cliente
{
  "nombreCompleto": "Juan Pérez",
  "email": "juan@example.com",
  "telefono": "+1234567890",
  "direccion": "Calle Principal 123",
  "activo": true
}
```

**Funcionalidades:**
- ✅ Crear nuevos clientes
- ✅ Editar información
- ✅ Ver historial
- ✅ Eliminar clientes

#### 3. **Gestión de Proveedores** 🚚

- Registro de proveedores
- Control de contactos
- Gestión de órdenes de compra

#### 4. **Gestión de Inventario** 📦

```javascript
// Ejemplo: Producto
{
  "nombre": "Cerveza Corona 355ml",
  "descripcion": "Cerveza importada clara",
  "stock": 150,
  "precio": 2.50,
  "activo": true
}
```

**Funcionalidades:**
- ✅ Control de stock en tiempo real
- ✅ Alertas de inventario bajo
- ✅ Gestión de precios
- ✅ Categorización de productos

#### 5. **Panel de Administrador** 🛡️

Solo accesible para usuarios con rol **Administrador**:

- Gestión completa de usuarios
- Estadísticas del sistema
- Configuración avanzada
- Reportes globales

---

## 💾 Persistencia de Datos

El sistema utiliza un **enfoque híbrido**:

1. **Intenta guardar en SQL Server** (si está disponible)
2. **Si falla, usa localStorage automáticamente**

Esto garantiza que el sistema **funcione sin base de datos**.

### Datos almacenados en localStorage:

```javascript
localStorage.setItem('usuariosRegistrados', JSON.stringify(usuarios));
localStorage.setItem('clientes', JSON.stringify(clientes));
localStorage.setItem('proveedores', JSON.stringify(proveedores));
localStorage.setItem('productos', JSON.stringify(productos));
localStorage.setItem('sesionActiva', JSON.stringify(sesion));
```

---

## 🐛 Solución de Problemas

### Problemas Comunes

#### 1. **Error de conexión a la base de datos**

```bash
# El sistema automáticamente usa localStorage
# Mensaje esperado: "Sistema funcionando en modo local"
```

**Solución:** No requiere acción, el sistema continúa funcionando normalmente.

#### 2. **Puerto ya en uso**

```bash
# Cambiar puerto en launchSettings.json o usar:
dotnet run --urls "http://localhost:5001"
```

#### 3. **Limpiar localStorage para pruebas**

```javascript
// En consola del navegador (F12):
localStorage.clear();
location.reload();
```

#### 4. **Error de compilación**

```bash
# Limpiar y reconstruir
dotnet clean
dotnet build
dotnet run
```

---

## 🧪 Pruebas (Testing)

### Ejecutar pruebas del sistema

Para guía completa de pruebas, consulta: [`DOCS/GUIA_PRUEBAS_SPRINT1.md`](DOCS/GUIA_PRUEBAS_SPRINT1.md)

### Casos de prueba principales:

- ✅ **CP-CLI-01:** Crear cliente exitosamente
- ✅ **CP-PRO-01:** Crear proveedor
- ✅ **CP-INV-01:** Gestión de inventario
- ✅ **CP-EMP-01:** Gestión de empleados
- ✅ **CP-ADM-01:** Control de acceso por roles

---

## 🤝 Contribución

¡Contribuciones son bienvenidas! Por favor sigue estos pasos:

1. Fork el proyecto
2. Crea una rama para tu feature
   ```bash
   git checkout -b feature/AmazingFeature
   ```
3. Commit tus cambios
   ```bash
   git commit -m 'Add some AmazingFeature'
   ```
4. Push a la rama
   ```bash
   git push origin feature/AmazingFeature
   ```
5. Abre un Pull Request

### Guía de Estilo

- Sigue las convenciones de código C# existentes
- Incluye comentarios XML para métodos públicos
- Actualiza la documentación correspondiente
- Prueba tu código antes de hacer commit

---

## 📋 Roadmap

- [x] **Sprint 1:** Módulos CRUD básicos
  - [x] Gestión de Clientes
  - [x] Gestión de Proveedores
  - [x] Gestión de Inventario
  - [x] Gestión de Empleados
  - [x] Sistema de autenticación
  - [x] Panel de administrador

- [ ] **Sprint 2:** Módulos avanzados
  - [ ] Facturación electrónica
  - [ ] Reportes PDF
  - [ ] Gráficos y analytics

- [ ] **Sprint 3:** Integraciones
  - [ ] APIs externas
  - [ ] E-commerce integration
  - [ ] Notificaciones por email

- [ ] **Sprint 4:** Optimizaciones
  - [ ] Mobile responsive
  - [ ] PWA support
  - [ ] Performance improvements

---

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver archivo `LICENSE` para más detalles.

---

## 👨‍💻 Autores

**S0ntyrr & function14**

- GitHub: [@S0ntyrr](https://github.com/S0ntyrr) | [@function14](https://github.com/function14)
- Proyecto: [Disko E-R-P](https://github.com/S0ntyrr/Disko-E-R-P)

---

## 📞 Soporte

Si encuentras algún problema o tienes sugerencias:

1. Abre un [Issue](https://github.com/S0ntyrr/Disko-E-R-P/issues)
2. Revisa la [documentación](DOCS/)
3. Contacta al equipo de desarrollo

---

<p align="center">
<img src="https://capsule-render.vercel.app/api?type=waving&color=gradient&height=100&section=footer" />
</p>

<p align="center">
⭐ Dale una estrella si este proyecto te fue útil ⭐
</p>


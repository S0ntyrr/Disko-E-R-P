# 🔐 Sistema de Autenticación - Disko ERP

## ✅ Cambios Implementados

### 1. Sistema de Autenticación Híbrido
- **Base de Datos**: Valida usuarios registrados en SQL Server
- **LocalStorage**: Permite autenticación local cuando no hay conexión a BD

### 2. Flujo de Autenticación

#### **Login (Inicio de Sesión)**
1. El usuario ingresa credenciales en `/Auth/Login`
2. El sistema intenta validar primero en **localStorage** (lado cliente)
3. Si no existe localmente, valida contra la **Base de Datos**
4. Crea sesión con **Cookies de autenticación** de ASP.NET Core
5. También guarda en **Session** del servidor
6. Redirige al **Dashboard**

#### **Registro (Crear Cuenta)**
1. El usuario ingresa datos en `/Auth/Register`
2. Los datos se guardan en **localStorage** del navegador
3. También intenta guardar en BD (si está disponible)
4. Usuario puede iniciar sesión inmediatamente

### 3. Archivos Creados/Modificados

#### **JavaScript**
- `wwwroot/js/login.js` - Manejo de login con localStorage
- `wwwroot/js/register.js` - Manejo de registro con localStorage

#### **CSS**
- `wwwroot/css/login.css` - Estilos modernos para login/registro

#### **Controladores**
- `AuthController.cs` - Actualizado para soportar autenticación híbrida
- `DashboardController.cs` - Verificación de sesión mejorada

#### **Vistas**
- `Views/Auth/Login.cshtml` - Enlace a registro agregado
- `Views/Auth/Register.cshtml` - Vista moderna completa
- `Views/Dashboard/Index.cshtml` - Muestra nombre del usuario

## 🧪 Cómo Probar

### Opción 1: Registro y Login Local (Sin BD)

1. **Ejecutar la aplicación:**
   ```bash
   cd LOGIN/USUARIO/SEGURIDAD
   dotnet run
   ```

2. **Abrir en navegador:**
   ```
   https://localhost:5001/Auth/Login
   ```

3. **Crear cuenta nueva:**
   - Click en "Crear cuenta"
   - Llenar formulario:
     - Usuario: `testuser`
     - Email: `test@example.com`
     - Contraseña: `123456`
     - Confirmar contraseña: `123456`
   - Aceptar términos
   - Click en "Crear Cuenta"

4. **Iniciar sesión:**
   - Usuario: `testuser`
   - Contraseña: `123456`
   - Click en "Iniciar Sesión"

5. **Verificar acceso:**
   - Deberías ser redirigido a `/Dashboard`
   - Ver tu nombre de usuario en el header
   - Ver mensaje de bienvenida

### Opción 2: Login con BD (Si está configurada)

Si tienes usuarios en la base de datos, puedes iniciar sesión directamente:

1. **Ir a login:**
   ```
   https://localhost:5001/Auth/Login
   ```

2. **Usar credenciales de BD:**
   - Usuario: [tu usuario en BD]
   - Contraseña: [tu contraseña en BD]
   - Click en "Iniciar Sesión"

## 🔍 Verificación de Sesión

### En el Navegador (F12 -> Console):

```javascript
// Ver usuarios registrados localmente
console.log(JSON.parse(localStorage.getItem('usuariosRegistrados')));

// Ver sesión activa
console.log(JSON.parse(localStorage.getItem('sesionActiva')));

// Verificar autenticación
console.log(sessionStorage.getItem('autenticado'));
```

### Desde el Servidor (Logs):

Los logs mostrarán:
- Intentos de login
- Validación de usuarios
- Accesos al dashboard
- Errores de autenticación

## 🚀 Flujo Completo del Sistema

```
┌─────────────────┐
│   Login Page    │
└────────┬────────┘
         │
         v
┌─────────────────────────────┐
│ Validar en localStorage     │
│ (JavaScript - Cliente)      │
└────────┬────────────────────┘
         │
    ┌────┴─────┐
    │ ¿Existe? │
    └────┬─────┘
         │
    YES  │  NO
    ┌────┴─────┐
    │          │
    v          v
┌───────┐  ┌──────────────────┐
│Guardar│  │Enviar al Servidor│
│Sesión │  │(Validar en BD)   │
└───┬───┘  └────────┬─────────┘
    │               │
    │               v
    │     ┌─────────────────┐
    │     │¿Usuario válido? │
    │     └────────┬────────┘
    │              │
    └──────┬───────┘
           │
           v
    ┌──────────────┐
    │Crear Cookie  │
    │+ Session     │
    └──────┬───────┘
           │
           v
    ┌──────────────┐
    │  Dashboard   │
    └──────────────┘
```

## 🛠️ Solución de Problemas

### Problema: "No redirige al Dashboard"

**Solución:**
1. Abrir F12 (DevTools)
2. Ver errores en Console
3. Verificar que `login.js` se cargue correctamente
4. Verificar que exista usuario en localStorage o BD

### Problema: "No guarda el usuario"

**Solución:**
1. Verificar que JavaScript esté habilitado
2. Limpiar localStorage: `localStorage.clear()`
3. Intentar registrar nuevamente

### Problema: "Error 404 al cargar JS/CSS"

**Solución:**
1. Verificar que existan los archivos en `wwwroot/`
2. Ejecutar: `dotnet clean && dotnet build`
3. Reiniciar la aplicación

## 📋 Credenciales de Prueba

### Usuario Local (localStorage)
Crear tu propio usuario usando el formulario de registro

### Usuario Demo (si está en código)
- Usuario: `admin`
- Contraseña: `123456`

## 🔒 Seguridad

**Nota Importante:** Este sistema usa localStorage para demostración. En producción:

- ✅ Hashear contraseñas (BCrypt)
- ✅ Usar HTTPS
- ✅ Implementar tokens JWT
- ✅ Rate limiting para login
- ✅ Validación de sesión en servidor
- ✅ Protección CSRF (ya implementada con AntiForgeryToken)

## 📞 Soporte

Si tienes problemas, revisa:
1. Los logs de la aplicación
2. La consola del navegador (F12)
3. Que el puerto no esté ocupado
4. Que las rutas sean correctas

---

**Última actualización:** 11 de Octubre, 2025
**Versión:** 1.0


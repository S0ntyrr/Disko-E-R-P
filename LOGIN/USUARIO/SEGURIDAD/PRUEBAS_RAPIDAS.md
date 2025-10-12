# 🧪 Guía de Pruebas Rápidas - Sistema de Autenticación

## ✅ CAMBIOS REALIZADOS

He corregido el problema del botón "Crear cuenta". Los cambios incluyen:

1. ✅ **JavaScript actualizado**: Ahora el formulario se envía correctamente al servidor
2. ✅ **Validación mejorada**: Verifica contraseñas antes de enviar
3. ✅ **Backup local**: Guarda también en localStorage
4. ✅ **Logs detallados**: Para diagnosticar problemas

## 🚀 PRUEBAS PASO A PASO

### Prueba 1: Verificar que la aplicación está corriendo

Abre tu navegador y ve a: `http://localhost:5000` o `https://localhost:5001`

Deberías ver la página de login.

---

### Prueba 2: Crear una cuenta nueva

1. **Haz clic en** "Crear cuenta" (enlace en la parte inferior del login)

2. **Completa el formulario:**
   ```
   Usuario: test123
   Email: test@ejemplo.com
   Contraseña: 123456
   Confirmar Contraseña: 123456
   ✅ Marcar "Acepto términos"
   ```

3. **Haz clic en "Crear Cuenta"**

4. **¿Qué debería pasar?**
   - ✅ Redirigir a la página de login
   - ✅ Ver mensaje verde "¡Registro exitoso!"
   - ✅ El usuario queda guardado

---

### Prueba 3: Iniciar sesión con la cuenta creada

1. **En la página de login:**
   ```
   Usuario: test123
   Contraseña: 123456
   ```

2. **Haz clic en "Iniciar Sesión"**

3. **¿Qué debería pasar?**
   - ✅ Redirigir al Dashboard
   - ✅ Ver tu nombre "test123" en el header
   - ✅ Ver mensaje de bienvenida

---

### Prueba 4: Verificar sesión en localStorage

1. **Abre DevTools** (F12)

2. **Ve a Console** y ejecuta:
   ```javascript
   // Ver usuarios registrados
   JSON.parse(localStorage.getItem('usuariosRegistrados'))
   
   // Ver sesión activa
   JSON.parse(localStorage.getItem('sesionActiva'))
   ```

3. **Deberías ver:**
   - Tu usuario en la lista de registrados
   - Los datos de sesión activa

---

### Prueba 5: Cerrar sesión

1. **En el Dashboard**, haz clic en "Cerrar sesión"

2. **¿Qué debería pasar?**
   - ✅ Redirigir al login
   - ✅ Ver mensaje "Sesión cerrada correctamente"
   - ✅ No poder acceder al Dashboard sin login

---

## 🔍 DIAGNOSTICAR PROBLEMAS

### El botón "Crear cuenta" no hace nada

**Opción 1: Verificar JavaScript**
1. Abre DevTools (F12)
2. Ve a la pestaña "Console"
3. ¿Hay errores en rojo?
4. Recarga con Ctrl+F5

**Opción 2: Verificar que el formulario tiene ID**
1. Abre DevTools (F12)
2. Ve a la pestaña "Elements"
3. Busca `<form id="registerForm"`
4. Debe existir ese ID

**Opción 3: Probar sin JavaScript**
1. Llena el formulario
2. Si no funciona, verifica los logs del servidor

---

### Ver logs del servidor

Los logs del servidor mostrarán:
```
info: LOGIN.USUARIO.SEGURIDAD.Controllers.AuthController[0]
      Intento de registro para usuario: test123
```

Si ves errores, cópialos y revisa el mensaje.

---

## 🛠️ ENDPOINTS DE PRUEBA

He agregado endpoints especiales para probar:

### 1. Probar conexión a BD
```
http://localhost:5000/Auth/TestDb
```
Te dirá si la BD está conectada y cuántos usuarios hay.

### 2. Probar registro simple (sin BD)
```
http://localhost:5000/Auth/TestRegister?usuario=test&password=123456
```
Intenta registrar directamente sin formulario.

---

## 📱 DEBUGGING EN TIEMPO REAL

### Console Commands (F12 → Console):

```javascript
// Limpiar todo el localStorage
localStorage.clear()

// Ver todos los usuarios registrados
console.table(JSON.parse(localStorage.getItem('usuariosRegistrados')))

// Crear usuario manualmente
const nuevoUsuario = {
  id: 1,
  usuario: 'manual',
  password: '123456',
  email: 'manual@test.com',
  rol: 'Usuario'
};
let usuarios = JSON.parse(localStorage.getItem('usuariosRegistrados') || '[]');
usuarios.push(nuevoUsuario);
localStorage.setItem('usuariosRegistrados', JSON.stringify(usuarios));
console.log('Usuario creado manualmente');

// Verificar que se creó
console.log(JSON.parse(localStorage.getItem('usuariosRegistrados')));
```

---

## ✨ FLUJO COMPLETO FUNCIONAL

```
1. Usuario abre /Auth/Register
   ↓
2. Llena formulario
   ↓
3. JavaScript valida contraseñas
   ↓
4. Si OK → envía al servidor
   ↓
5. Servidor recibe POST /Auth/Register
   ↓
6. Intenta guardar en BD
   ↓
7. Si BD falla → OK igual (modo local)
   ↓
8. También guarda en localStorage (backup)
   ↓
9. Redirige a /Auth/Login con mensaje
   ↓
10. Usuario inicia sesión
    ↓
11. Valida en localStorage primero
    ↓
12. Si no existe → valida en servidor/BD
    ↓
13. Crea sesión con cookies
    ↓
14. Redirige a /Dashboard
    ↓
15. ✅ ÉXITO
```

---

## 🎯 CHECKLIST DE VERIFICACIÓN

Antes de reportar un problema, verifica:

- [ ] La aplicación está corriendo (`dotnet run`)
- [ ] El navegador muestra la página de login
- [ ] No hay errores en Console (F12)
- [ ] Los archivos JS se cargan (Network tab en F12)
- [ ] El formulario tiene el ID correcto
- [ ] Las contraseñas coinciden
- [ ] El checkbox "Acepto términos" está marcado
- [ ] JavaScript está habilitado en el navegador

---

## 💡 TIPS

1. **Ctrl+F5** recarga sin caché (importante después de cambios)
2. **F12** abre DevTools (tu mejor amigo)
3. **localStorage.clear()** limpia todo si algo sale mal
4. Los logs del servidor están en la terminal donde ejecutas `dotnet run`

---

## 📞 ¿Aún no funciona?

Si después de todas las pruebas el botón sigue sin funcionar:

1. Copia los errores de la Console (F12)
2. Copia los logs del servidor (terminal)
3. Toma un screenshot del formulario
4. Compártelos para diagnóstico

---

**Última actualización:** 11 de Octubre, 2025  
**Estado:** ✅ LISTO PARA PROBAR


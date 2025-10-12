// Sistema de autenticación con localStorage
console.log('🔵 login.js cargado correctamente');

// Esperar a que el DOM esté listo
window.addEventListener('load', function() {
    console.log('🟢 DOM completamente cargado');
    
    const loginForm = document.getElementById('loginForm');
    const togglePassword = document.getElementById('togglePassword');
    const passwordInput = document.getElementById('passwordInput');
    const toggleIcon = document.getElementById('toggleIcon');
    const btnLogin = document.getElementById('btnLogin');
    const loginSpinner = document.getElementById('loginSpinner');

    console.log('📋 Formulario encontrado:', loginForm ? 'SI' : 'NO');

    // Toggle mostrar/ocultar contraseña
    if (togglePassword) {
        togglePassword.addEventListener('click', function() {
            const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
            passwordInput.setAttribute('type', type);
            toggleIcon.classList.toggle('fa-eye');
            toggleIcon.classList.toggle('fa-eye-slash');
        });
    }

    // Interceptar el click del botón de submit
    if (btnLogin) {
        console.log('🔘 Configurando listener del botón submit');
        btnLogin.addEventListener('click', function(e) {
            console.log('👆 Click en botón Iniciar Sesión detectado');
            e.preventDefault(); // Prevenir el submit inmediato
            e.stopPropagation(); // Detener la propagación
            
            // Obtener valores de forma más robusta
            console.log('🔎 loginForm:', loginForm);
            console.log('🔎 Buscando inputs en el formulario...');
            
            // Listar todos los inputs para debug
            const todosLosInputs = loginForm.querySelectorAll('input');
            console.log('🔎 Total de inputs encontrados:', todosLosInputs.length);
            todosLosInputs.forEach((inp, idx) => {
                console.log(`  Input ${idx}: type="${inp.type}", id="${inp.id}", name="${inp.name}"`);
            });
            
            // El campo usuario es el segundo input (el primero es el CSRF token)
            // Buscar por clase form-control que no sea password ni checkbox
            const allInputs = Array.from(loginForm.querySelectorAll('input'));
            const usuarioInput = allInputs.find(inp => 
                inp.classList.contains('form-control') && 
                inp.type !== 'password' && 
                inp.type !== 'checkbox' &&
                inp.type !== 'hidden'
            );
            const passwordInput = document.getElementById('passwordInput') ||
                                  loginForm.querySelector('input[type="password"]');
            const recordarInput = document.getElementById('recordarSesion') ||
                                  loginForm.querySelector('input[type="checkbox"]');
            
            console.log('Inputs encontrados:', {
                usuario: usuarioInput ? 'SI' : 'NO',
                password: passwordInput ? 'SI' : 'NO',
                recordar: recordarInput ? 'SI' : 'NO'
            });
            
            console.log('usuarioInput:', usuarioInput);
            
            if (!usuarioInput || !passwordInput) {
                console.log('❌ No se encontraron los campos del formulario');
                return;
            }
            
            const usuario = usuarioInput.value;
            const password = passwordInput.value;
            const recordar = recordarInput ? recordarInput.checked : false;
            
            console.log('Usuario ingresado:', usuario);
            console.log('Password ingresado:', password ? '***' : 'VACÍO');
            
            // Validar campos vacíos
            if (!usuario || !password) {
                console.log('❌ Campos vacíos - permitir validación normal');
                loginForm.submit(); // Enviar al servidor para mostrar errores
                return;
            }
            
            // Intentar validar con localStorage
            console.log('🔍 Buscando usuario en localStorage:', usuario);
            const usuarioLocal = validarUsuarioLocal(usuario, password);
            console.log('📊 Resultado búsqueda:', usuarioLocal);
            
            if (usuarioLocal) {
                // Login exitoso con localStorage
                console.log('🎯 Usuario encontrado en localStorage, guardando sesión...');
                guardarSesionLocal(usuarioLocal);
                
                console.log('✅ Login exitoso con localStorage');
                console.log('🚀 Redirigiendo a Dashboard...');
                
                // Redirigir inmediatamente
                window.location.href = '/Dashboard/Index';
                return;
            }
            
            // Si no está en localStorage, enviar al servidor
            console.log('🔄 Usuario no encontrado en localStorage, enviando al servidor...');
            loginForm.submit();
        });
    }
    
    // Manejar el envío del formulario (backup)
    if (loginForm) {
        console.log('✅ Configurando listener del formulario');
        
        loginForm.addEventListener('submit', function(e) {
            console.log('🚀 FORMULARIO ENVIADO - Iniciando validación');
            console.log('Evento submit disparado correctamente');
            
            const usuario = document.querySelector('input[name="Usuario"]').value;
            const password = document.querySelector('input[name="Password"]').value;
            const recordar = document.querySelector('input[name="RecordarSesion"]').checked;
            
            console.log('Usuario ingresado:', usuario);
            console.log('Password ingresado:', password ? '***' : 'VACÍO');
            
            // Validar campos vacíos
            if (!usuario || !password) {
                console.log('❌ Campos vacíos');
                return true; // Dejar que el formulario se envíe normalmente para mostrar errores
            }
            
            // Guardar usuario si está marcado "recordar"
            if (recordar) {
                localStorage.setItem('recordarUsuario', usuario);
            } else {
                localStorage.removeItem('recordarUsuario');
            }
            
            // Mostrar spinner
            if (loginSpinner && btnLogin) {
                const btnText = btnLogin.querySelector('.btn-text');
                if (btnText) btnText.classList.add('d-none');
                loginSpinner.classList.remove('d-none');
                btnLogin.disabled = true;
            }
            
            // Intentar validar con localStorage primero
            console.log('🔍 Buscando usuario en localStorage:', usuario);
            const usuarioLocal = validarUsuarioLocal(usuario, password);
            console.log('📊 Resultado búsqueda:', usuarioLocal);
            
            if (usuarioLocal) {
                // Login exitoso con localStorage
                e.preventDefault();
                console.log('🎯 Usuario encontrado en localStorage, guardando sesión...');
                guardarSesionLocal(usuarioLocal);
                
                console.log('✅ Login exitoso con localStorage');
                console.log('🚀 Redirigiendo a Dashboard en 500ms...');
                
                // Pequeña espera para asegurar que se guarde en localStorage
                setTimeout(function() {
                    window.location.href = '/Dashboard/Index';
                }, 500);
                
                return false;
            }
            
            // Si no está en localStorage, dejar que el formulario se envíe al servidor
            console.log('🔄 Usuario no encontrado en localStorage, enviando al servidor para validar en BD...');
            // El servidor lo validará y redirigirá
            return true;
        });
    }

    // Cargar usuario recordado
    const usuarioRecordado = localStorage.getItem('recordarUsuario');
    if (usuarioRecordado) {
        const inputUsuario = document.querySelector('input[name="Usuario"]');
        const checkRecordar = document.querySelector('input[name="RecordarSesion"]');
        if (inputUsuario) inputUsuario.value = usuarioRecordado;
        if (checkRecordar) checkRecordar.checked = true;
    }
});

// Función para guardar sesión en localStorage (llamada desde el servidor después de login exitoso)
function guardarSesionLocal(usuario) {
    const sesion = {
        usuario: usuario.nombreUsuario || usuario.usuario,
        nombreCompleto: usuario.nombreCompleto || usuario.usuario,
        email: usuario.email,
        rol: usuario.rol || 'Usuario',
        timestamp: new Date().getTime()
    };
    localStorage.setItem('sesionActiva', JSON.stringify(sesion));
    sessionStorage.setItem('autenticado', 'true');
    
    console.log('💾 Sesión guardada:', sesion);
}

// Función para verificar sesión activa
function verificarSesion() {
    const sesion = localStorage.getItem('sesionActiva');
    const autenticado = sessionStorage.getItem('autenticado');
    
    if (!sesion || !autenticado) {
        return false;
    }
    
    const dataSesion = JSON.parse(sesion);
    const ahora = new Date().getTime();
    const tiempoExpiracion = 8 * 60 * 60 * 1000; // 8 horas
    
    if (ahora - dataSesion.timestamp > tiempoExpiracion) {
        cerrarSesionLocal();
        return false;
    }
    
    return true;
}

// Función para cerrar sesión local
function cerrarSesionLocal() {
    localStorage.removeItem('sesionActiva');
    sessionStorage.removeItem('autenticado');
}

// Función para obtener usuario actual
function obtenerUsuarioActual() {
    const sesion = localStorage.getItem('sesionActiva');
    if (sesion) {
        return JSON.parse(sesion);
    }
    return null;
}

// Función para validar usuario local
function validarUsuarioLocal(usuario, password) {
    try {
        const usuarios = JSON.parse(localStorage.getItem('usuariosRegistrados') || '[]');
        
        // Buscar usuario
        const usuarioEncontrado = usuarios.find(u => 
            u.usuario === usuario && u.password === password
        );
        
        console.log('Usuarios en localStorage:', usuarios.length);
        console.log('Usuario buscado:', usuario);
        console.log('Usuario encontrado:', usuarioEncontrado ? 'Sí' : 'No');
        
        return usuarioEncontrado || null;
    } catch (e) {
        console.error('Error al validar usuario local:', e);
        return null;
    }
}


// Sistema de registro con localStorage
document.addEventListener('DOMContentLoaded', function() {
    const registerForm = document.getElementById('registerForm');
    const togglePassword = document.getElementById('togglePassword');
    const toggleConfirmPassword = document.getElementById('toggleConfirmPassword');
    const passwordInput = document.getElementById('passwordInput');
    const confirmPasswordInput = document.getElementById('confirmPasswordInput');
    const passwordError = document.getElementById('passwordError');

    // Toggle mostrar/ocultar contraseña
    if (togglePassword) {
        togglePassword.addEventListener('click', function() {
            const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
            passwordInput.setAttribute('type', type);
            this.querySelector('i').classList.toggle('fa-eye');
            this.querySelector('i').classList.toggle('fa-eye-slash');
        });
    }

    if (toggleConfirmPassword) {
        toggleConfirmPassword.addEventListener('click', function() {
            const type = confirmPasswordInput.getAttribute('type') === 'password' ? 'text' : 'password';
            confirmPasswordInput.setAttribute('type', type);
            this.querySelector('i').classList.toggle('fa-eye');
            this.querySelector('i').classList.toggle('fa-eye-slash');
        });
    }

    // Validar contraseñas coincidan
    if (confirmPasswordInput && passwordInput) {
        confirmPasswordInput.addEventListener('input', function() {
            if (passwordInput.value !== confirmPasswordInput.value) {
                confirmPasswordInput.setCustomValidity('Las contraseñas no coinciden');
                if (passwordError) {
                    passwordError.textContent = 'Las contraseñas no coinciden';
                }
            } else {
                confirmPasswordInput.setCustomValidity('');
                if (passwordError) {
                    passwordError.textContent = '';
                }
            }
        });

        passwordInput.addEventListener('input', function() {
            if (confirmPasswordInput.value && passwordInput.value !== confirmPasswordInput.value) {
                if (passwordError) {
                    passwordError.textContent = 'Las contraseñas no coinciden';
                }
            } else {
                if (passwordError) {
                    passwordError.textContent = '';
                }
            }
        });
    }

    // Manejar el envío del formulario
    if (registerForm) {
        registerForm.addEventListener('submit', function(e) {
            const password = document.querySelector('input[name="Password"]').value;
            const confirmPassword = document.querySelector('input[name="ConfirmPassword"]').value;
            
            // Validar que las contraseñas coincidan
            if (password !== confirmPassword) {
                e.preventDefault();
                alert('Las contraseñas no coinciden');
                return false;
            }
            
            // Validar longitud mínima
            if (password.length < 6) {
                e.preventDefault();
                alert('La contraseña debe tener al menos 6 caracteres');
                return false;
            }
            
            // Si todo está bien, también guardar en localStorage
            const usuario = document.querySelector('input[name="Usuario"]').value;
            const email = document.querySelector('input[name="Email"]').value;
            
            // Registrar en localStorage (para backup local)
            registrarUsuarioLocal(usuario, password, email);
            
            // Permitir que el formulario se envíe normalmente al servidor
            return true;
        });
    }
});

// Función para registrar usuario en localStorage
function registrarUsuarioLocal(usuario, password, email) {
    // Obtener usuarios existentes
    let usuarios = JSON.parse(localStorage.getItem('usuariosRegistrados') || '[]');
    
    // Verificar si el usuario ya existe
    if (usuarios.some(u => u.usuario === usuario)) {
        return { exitoso: false, mensaje: 'El usuario ya existe' };
    }
    
    // Agregar nuevo usuario
    const nuevoUsuario = {
        id: usuarios.length + 1,
        usuario: usuario,
        password: password, // En un sistema real, esto debería estar hasheado
        email: email,
        nombreCompleto: usuario,
        rol: 'Usuario',
        rolId: 2,
        estado: 'Activo',
        fechaCreacion: new Date().toISOString()
    };
    
    usuarios.push(nuevoUsuario);
    localStorage.setItem('usuariosRegistrados', JSON.stringify(usuarios));
    
    return { exitoso: true, mensaje: 'Usuario registrado correctamente' };
}

// Función para validar usuario local
function validarUsuarioLocal(usuario, password) {
    const usuarios = JSON.parse(localStorage.getItem('usuariosRegistrados') || '[]');
    
    // Buscar usuario
    const usuarioEncontrado = usuarios.find(u => 
        u.usuario === usuario && u.password === password && u.estado === 'Activo'
    );
    
    return usuarioEncontrado || null;
}


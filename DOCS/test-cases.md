# Test Cases por Historia de Usuario

---

## HU1: Login del usuario

| ID | Caso de prueba | Entrada | Resultado esperado |
|----|----------------|---------|---------------------|
| TC-1 | Login válido | user@test.com / 1234 | Acceso exitoso al sistema |
| TC-2 | Contraseña incorrecta | user@test.com / 0000 | Mensaje "credenciales inválidas" |
| TC-3 | Email inválido | user@@test | Validación de formato de email |
| TC-4 | Campos vacíos | "" / "" | Mensaje "todos los campos son obligatorios" |
| TC-5 | Usuario inactivo | user@bloqueado.com / 1234 | Mensaje "usuario inactivo" |

---

## HU2: Registro de usuario

| ID | Caso de prueba | Entrada | Resultado esperado |
|----|----------------|---------|---------------------|
| TC-6 | Registro válido | Datos completos correctos | Usuario creado con éxito |
| TC-7 | Email ya registrado | user@test.com | Mensaje "email ya existe" |
| TC-8 | Contraseña débil | 123 | Mensaje "contraseña insegura" |
| TC-9 | Campos vacíos | "" | Mensaje de validación por campo vacío |
| TC-10 | Edad inválida | -5 | Mensaje "edad no válida" |

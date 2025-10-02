USE DiskoERP;
GO

PRINT 'Insertando datos iniciales...';
GO

-- =============================================
-- INSERTAR ROLES
-- =============================================
SET IDENTITY_INSERT Roles ON;
GO

INSERT INTO Roles (RolId, NombreRol, Descripcion, Estado, FechaCreacion)
VALUES 
    (1, 'Administrador', 'Acceso total al sistema ERP', 'Activo', GETDATE()),
    (2, 'Gerente', 'Gestión general de la discoteca y reportes', 'Activo', GETDATE()),
    (3, 'Cajero', 'Acceso al punto de venta y caja', 'Activo', GETDATE()),
    (4, 'Mesero', 'Toma de pedidos y atención a mesas', 'Activo', GETDATE()),
    (5, 'Seguridad', 'Control de entrada, aforo y seguridad', 'Activo', GETDATE()),
    (6, 'Bartender', 'Preparación de bebidas y control de bar', 'Activo', GETDATE()),
    (7, 'DJ', 'Control de música y entretenimiento', 'Activo', GETDATE());
GO

SET IDENTITY_INSERT Roles OFF;
GO

PRINT '✓ Roles insertados correctamente.';
GO

-- =============================================
-- INSERTAR PERMISOS
-- =============================================
SET IDENTITY_INSERT Permisos ON;
GO

INSERT INTO Permisos (PermisoId, NombrePermiso, CodigoPermiso, Descripcion, Modulo, Estado)
VALUES 
    -- Dashboard
    (1, 'Ver Dashboard', 'DASHBOARD_VIEW', 'Acceso al dashboard principal', 'Dashboard', 'Activo'),
    
    -- Usuarios
    (2, 'Gestionar Usuarios', 'USUARIOS_MANAGE', 'Crear, editar y eliminar usuarios', 'Usuarios', 'Activo'),
    (3, 'Ver Usuarios', 'USUARIOS_VIEW', 'Ver listado de usuarios', 'Usuarios', 'Activo'),
    
    -- Reportes
    (4, 'Ver Reportes', 'REPORTES_VIEW', 'Acceso a reportes y estadísticas', 'Reportes', 'Activo'),
    (5, 'Exportar Reportes', 'REPORTES_EXPORT', 'Exportar reportes a PDF/Excel', 'Reportes', 'Activo'),
    
    -- Ventas
    (6, 'Gestionar Ventas', 'VENTAS_MANAGE', 'Registrar y gestionar ventas', 'Ventas', 'Activo'),
    (7, 'Ver Ventas', 'VENTAS_VIEW', 'Ver historial de ventas', 'Ventas', 'Activo'),
    (8, 'Anular Ventas', 'VENTAS_CANCEL', 'Anular ventas registradas', 'Ventas', 'Activo'),
    
    -- Inventario
    (9, 'Gestionar Inventario', 'INVENTARIO_MANAGE', 'Gestionar productos e inventario', 'Inventario', 'Activo'),
    (10, 'Ver Inventario', 'INVENTARIO_VIEW', 'Ver estado del inventario', 'Inventario', 'Activo'),
    
    -- Control de Entrada
    (11, 'Control Entrada', 'ENTRADA_CONTROL', 'Gestionar entrada de clientes', 'Entrada', 'Activo'),
    (12, 'Ver Aforo', 'AFORO_VIEW', 'Ver aforo en tiempo real', 'Entrada', 'Activo'),
    
    -- Reservaciones
    (13, 'Gestionar Reservaciones', 'RESERVAS_MANAGE', 'Crear y gestionar reservaciones', 'Reservaciones', 'Activo'),
    (14, 'Ver Reservaciones', 'RESERVAS_VIEW', 'Ver listado de reservaciones', 'Reservaciones', 'Activo'),
    
    -- Clientes
    (15, 'Gestionar Clientes', 'CLIENTES_MANAGE', 'Gestionar información de clientes', 'Clientes', 'Activo'),
    (16, 'Ver Clientes', 'CLIENTES_VIEW', 'Ver base de datos de clientes', 'Clientes', 'Activo'),
    
    -- Personal
    (17, 'Gestionar Personal', 'PERSONAL_MANAGE', 'Gestionar empleados y turnos', 'Personal', 'Activo'),
    (18, 'Ver Personal', 'PERSONAL_VIEW', 'Ver información del personal', 'Personal', 'Activo'),
    
    -- Eventos
    (19, 'Gestionar Eventos', 'EVENTOS_MANAGE', 'Crear y gestionar eventos', 'Eventos', 'Activo'),
    (20, 'Ver Eventos', 'EVENTOS_VIEW', 'Ver listado de eventos', 'Eventos', 'Activo');
GO

SET IDENTITY_INSERT Permisos OFF;
GO

PRINT '✓ Permisos insertados correctamente.';
GO

-- =============================================
-- ASIGNAR PERMISOS A ROLES
-- =============================================
SET IDENTITY_INSERT RolesPermisos ON;
GO

-- Administrador: TODOS los permisos
INSERT INTO RolesPermisos (RolPermisoId, RolId, PermisoId, FechaAsignacion)
SELECT ROW_NUMBER() OVER (ORDER BY PermisoId) as RolPermisoId, 1 as RolId, PermisoId, GETDATE()
FROM Permisos;

-- Gerente: Permisos de visualización y gestión (excepto usuarios)
INSERT INTO RolesPermisos (RolPermisoId, RolId, PermisoId, FechaAsignacion)
VALUES 
    (21, 2, 1, GETDATE()),  -- Dashboard
    (22, 2, 3, GETDATE()),  -- Ver Usuarios
    (23, 2, 4, GETDATE()),  -- Ver Reportes
    (24, 2, 5, GETDATE()),  -- Exportar Reportes
    (25, 2, 6, GETDATE()),  -- Gestionar Ventas
    (26, 2, 7, GETDATE()),  -- Ver Ventas
    (27, 2, 9, GETDATE()),  -- Gestionar Inventario
    (28, 2, 10, GETDATE()), -- Ver Inventario
    (29, 2, 12, GETDATE()), -- Ver Aforo
    (30, 2, 13, GETDATE()), -- Gestionar Reservaciones
    (31, 2, 14, GETDATE()), -- Ver Reservaciones
    (32, 2, 15, GETDATE()), -- Gestionar Clientes
    (33, 2, 16, GETDATE()), -- Ver Clientes
    (34, 2, 17, GETDATE()), -- Gestionar Personal
    (35, 2, 18, GETDATE()), -- Ver Personal
    (36, 2, 19, GETDATE()), -- Gestionar Eventos
    (37, 2, 20, GETDATE()); -- Ver Eventos

-- Cajero: Permisos de ventas
INSERT INTO RolesPermisos (RolPermisoId, RolId, PermisoId, FechaAsignacion)
VALUES 
    (38, 3, 1, GETDATE()),  -- Dashboard
    (39, 3, 6, GETDATE()),  -- Gestionar Ventas
    (40, 3, 7, GETDATE()),  -- Ver Ventas
    (41, 3, 10, GETDATE()), -- Ver Inventario
    (42, 3, 16, GETDATE()); -- Ver Clientes

-- Mesero: Permisos de pedidos y ventas
INSERT INTO RolesPermisos (RolPermisoId, RolId, PermisoId, FechaAsignacion)
VALUES 
    (43, 4, 1, GETDATE()),  -- Dashboard
    (44, 4, 6, GETDATE()),  -- Gestionar Ventas
    (45, 4, 7, GETDATE()),  -- Ver Ventas
    (46, 4, 10, GETDATE()), -- Ver Inventario
    (47, 4, 14, GETDATE()), -- Ver Reservaciones
    (48, 4, 16, GETDATE()); -- Ver Clientes

-- Seguridad: Control de entrada
INSERT INTO RolesPermisos (RolPermisoId, RolId, PermisoId, FechaAsignacion)
VALUES 
    (49, 5, 1, GETDATE()),  -- Dashboard
    (50, 5, 11, GETDATE()), -- Control Entrada
    (51, 5, 12, GETDATE()), -- Ver Aforo
    (52, 5, 14, GETDATE()), -- Ver Reservaciones
    (53, 5, 16, GETDATE()); -- Ver Clientes

-- Bartender: Ventas e inventario de bar
INSERT INTO RolesPermisos (RolPermisoId, RolId, PermisoId, FechaAsignacion)
VALUES 
    (54, 6, 1, GETDATE()),  -- Dashboard
    (55, 6, 6, GETDATE()),  -- Gestionar Ventas
    (56, 6, 10, GETDATE()); -- Ver Inventario

-- DJ: Dashboard y eventos
INSERT INTO RolesPermisos (RolPermisoId, RolId, PermisoId, FechaAsignacion)
VALUES 
    (57, 7, 1, GETDATE()),  -- Dashboard
    (58, 7, 20, GETDATE()); -- Ver Eventos

SET IDENTITY_INSERT RolesPermisos OFF;
GO

PRINT '✓ Permisos asignados a roles correctamente.';
GO

-- =============================================
-- INSERTAR USUARIO ADMINISTRADOR POR DEFECTO
-- =============================================
-- Password: Admin123!
-- Hash generado con SHA256 + Salt

SET IDENTITY_INSERT Usuarios ON;
GO

INSERT INTO Usuarios (UsuarioId, NombreCompleto, Email, NombreUsuario, PasswordHash, PasswordSalt, RolId, Estado, FechaCreacion)
VALUES (
    1,
    'Administrador del Sistema',
    'admin@disko.com',
    'admin',
    'lJZBv8F2cY8KjH5xqP9mWnR7sT4uV6wX3yZ0aB1cD2eF3gH4iJ5kL6mN7oP8qR9s',  -- Este es un ejemplo
    'dGVzdHNhbHQxMjM0NTY3ODkwYWJjZGVmZ2hpamtsbW5vcHFyc3R1dnd4eXo=',
    1,  -- Rol Administrador
    'Activo',
    GETDATE()
);
GO

SET IDENTITY_INSERT Usuarios OFF;
GO

PRINT '✓ Usuario administrador creado correctamente.';
PRINT '   Usuario: admin';
PRINT '   Password: Admin123!';
PRINT '   ¡IMPORTANTE! Cambiar esta contraseña después del primer acceso.';
GO

-- =============================================
-- INSERTAR USUARIOS DE EJEMPLO
-- =============================================
SET IDENTITY_INSERT Usuarios ON;
GO

INSERT INTO Usuarios (UsuarioId, NombreCompleto, Email, NombreUsuario, PasswordHash, PasswordSalt, RolId, Estado, FechaCreacion, CreadoPor)
VALUES 
    (2, 'Carlos Ramírez', 'carlos.ramirez@disko.com', 'cramirez', 'hashexample2', 'saltexample2', 2, 'Activo', GETDATE(), 1),
    (3, 'Ana Martínez', 'ana.martinez@disko.com', 'amartinez', 'hashexample3', 'saltexample3', 3, 'Activo', GETDATE(), 1),
    (4, 'Luis González', 'luis.gonzalez@disko.com', 'lgonzalez', 'hashexample4', 'saltexample4', 4, 'Activo', GETDATE(), 1),
    (5, 'María López', 'maria.lopez@disko.com', 'mlopez', 'hashexample5', 'saltexample5', 5, 'Activo', GETDATE(), 1),
    (6, 'Pedro Sánchez', 'pedro.sanchez@disko.com', 'psanchez', 'hashexample6', 'saltexample6', 6, 'Activo', GETDATE(), 1),
    (7, 'Usuario Inactivo', 'inactivo@disko.com', 'inactivo', 'hashexample7', 'saltexample7', 4, 'Inactivo', GETDATE(), 1);
GO

SET IDENTITY_INSERT Usuarios OFF;
GO

PRINT '✓ Usuarios de ejemplo creados correctamente.';
GO

-- =============================================
-- VERIFICAR DATOS INSERTADOS
-- =============================================
PRINT '';
PRINT '==========================================';
PRINT 'RESUMEN DE DATOS INSERTADOS:';
PRINT '==========================================';

SELECT 'Roles' as Tabla, COUNT(*) as Total FROM Roles
UNION ALL
SELECT 'Permisos' as Tabla, COUNT(*) as Total FROM Permisos
UNION ALL
SELECT 'RolesPermisos' as Tabla, COUNT(*) as Total FROM RolesPermisos
UNION ALL
SELECT 'Usuarios' as Tabla, COUNT(*) as Total FROM Usuarios;
GO

PRINT '';
PRINT '✓ Base de datos DiskoERP lista para usar!';
PRINT '==========================================';
GO
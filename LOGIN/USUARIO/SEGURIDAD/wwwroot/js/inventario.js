/**
 * Módulo de Inventario - Disko ERP
 * Funciones principales para gestión de inventario con localStorage
 */

console.log('📦 Módulo de Inventario cargado');

// ============================================
// FUNCIONES DE PRODUCTOS
// ============================================

/**
 * Obtener todos los productos
 */
function obtenerProductos() {
    try {
        return JSON.parse(localStorage.getItem('productos') || '[]');
    } catch (e) {
        console.error('Error al obtener productos:', e);
        return [];
    }
}

/**
 * Guardar productos
 */
function guardarProductos(productos) {
    try {
        localStorage.setItem('productos', JSON.stringify(productos));
        console.log('✅ Productos guardados');
        return true;
    } catch (e) {
        console.error('Error al guardar productos:', e);
        return false;
    }
}

/**
 * Obtener producto por ID
 */
function obtenerProductoPorId(productoId) {
    const productos = obtenerProductos();
    return productos.find(p => p.productoId === parseInt(productoId));
}

/**
 * Crear nuevo producto
 */
function crearProducto(producto) {
    const productos = obtenerProductos();
    const nuevoId = productos.length > 0 
        ? Math.max(...productos.map(p => p.productoId)) + 1 
        : 1;
    
    const nuevoProducto = {
        productoId: nuevoId,
        ...producto,
        fechaCreacion: new Date().toISOString()
    };
    
    productos.push(nuevoProducto);
    guardarProductos(productos);
    return nuevoProducto;
}

/**
 * Actualizar producto
 */
function actualizarProducto(productoId, datosActualizados) {
    const productos = obtenerProductos();
    const index = productos.findIndex(p => p.productoId === parseInt(productoId));
    
    if (index === -1) {
        console.error('Producto no encontrado:', productoId);
        return false;
    }
    
    productos[index] = {
        ...productos[index],
        ...datosActualizados,
        fechaActualizacion: new Date().toISOString()
    };
    
    guardarProductos(productos);
    return true;
}

/**
 * Eliminar producto
 */
function eliminarProducto(productoId) {
    const productos = obtenerProductos();
    const productosFiltrados = productos.filter(p => p.productoId !== parseInt(productoId));
    
    if (productos.length === productosFiltrados.length) {
        console.error('Producto no encontrado:', productoId);
        return false;
    }
    
    guardarProductos(productosFiltrados);
    return true;
}

// ============================================
// FUNCIONES DE CATEGORÍAS
// ============================================

/**
 * Obtener todas las categorías
 */
function obtenerCategorias() {
    try {
        return JSON.parse(localStorage.getItem('categoriasInventario') || '[]');
    } catch (e) {
        console.error('Error al obtener categorías:', e);
        return [];
    }
}

/**
 * Guardar categorías
 */
function guardarCategorias(categorias) {
    try {
        localStorage.setItem('categoriasInventario', JSON.stringify(categorias));
        console.log('✅ Categorías guardadas');
        return true;
    } catch (e) {
        console.error('Error al guardar categorías:', e);
        return false;
    }
}

/**
 * Crear nueva categoría
 */
function crearCategoria(categoria) {
    const categorias = obtenerCategorias();
    const nuevoId = categorias.length > 0 
        ? Math.max(...categorias.map(c => c.categoriaId)) + 1 
        : 1;
    
    const nuevaCategoria = {
        categoriaId: nuevoId,
        ...categoria,
        fechaCreacion: new Date().toISOString()
    };
    
    categorias.push(nuevaCategoria);
    guardarCategorias(categorias);
    return nuevaCategoria;
}

/**
 * Obtener categoría por ID
 */
function obtenerCategoriaPorId(categoriaId) {
    const categorias = obtenerCategorias();
    return categorias.find(c => c.categoriaId === parseInt(categoriaId));
}

// ============================================
// FUNCIONES DE MOVIMIENTOS
// ============================================

/**
 * Obtener todos los movimientos
 */
function obtenerMovimientos() {
    try {
        return JSON.parse(localStorage.getItem('movimientosInventario') || '[]');
    } catch (e) {
        console.error('Error al obtener movimientos:', e);
        return [];
    }
}

/**
 * Guardar movimientos
 */
function guardarMovimientos(movimientos) {
    try {
        localStorage.setItem('movimientosInventario', JSON.stringify(movimientos));
        console.log('✅ Movimientos guardados');
        return true;
    } catch (e) {
        console.error('Error al guardar movimientos:', e);
        return false;
    }
}

/**
 * Crear nuevo movimiento de inventario
 */
function crearMovimiento(movimiento) {
    const movimientos = obtenerMovimientos();
    const productos = obtenerProductos();
    
    // Obtener producto
    const producto = productos.find(p => p.productoId === movimiento.productoId);
    if (!producto) {
        console.error('Producto no encontrado:', movimiento.productoId);
        return false;
    }
    
    // Calcular stocks
    const stockAnterior = producto.stock;
    let stockNuevo;
    
    if (movimiento.tipo === 'Entrada') {
        stockNuevo = stockAnterior + movimiento.cantidad;
    } else if (movimiento.tipo === 'Salida') {
        if (stockAnterior < movimiento.cantidad) {
            console.error('Stock insuficiente');
            return false;
        }
        stockNuevo = stockAnterior - movimiento.cantidad;
    } else {
        console.error('Tipo de movimiento inválido:', movimiento.tipo);
        return false;
    }
    
    // Obtener usuario actual
    let usuario = 'Sistema';
    try {
        const sesion = JSON.parse(localStorage.getItem('sesionActiva') || '{}');
        usuario = sesion.usuario || 'Sistema';
    } catch (e) {}
    
    // Crear movimiento
    const nuevoId = movimientos.length > 0 
        ? Math.max(...movimientos.map(m => m.movimientoId)) + 1 
        : 1;
    
    const nuevoMovimiento = {
        movimientoId: nuevoId,
        productoId: movimiento.productoId,
        nombreProducto: producto.nombre,
        tipo: movimiento.tipo,
        cantidad: movimiento.cantidad,
        stockAnterior: stockAnterior,
        stockNuevo: stockNuevo,
        motivo: movimiento.motivo || '',
        observaciones: movimiento.observaciones || '',
        usuario: usuario,
        fecha: new Date().toISOString()
    };
    
    // Actualizar stock del producto
    producto.stock = stockNuevo;
    guardarProductos(productos);
    
    // Guardar movimiento
    movimientos.push(nuevoMovimiento);
    guardarMovimientos(movimientos);
    
    console.log('✅ Movimiento creado:', nuevoMovimiento);
    return nuevoMovimiento;
}

/**
 * Eliminar movimiento (revertir cambio de stock)
 */
function eliminarMovimiento(movimientoId) {
    const movimientos = obtenerMovimientos();
    const productos = obtenerProductos();
    
    const movimiento = movimientos.find(m => m.movimientoId === parseInt(movimientoId));
    if (!movimiento) {
        console.error('Movimiento no encontrado:', movimientoId);
        return false;
    }
    
    // Revertir cambio de stock
    const producto = productos.find(p => p.productoId === movimiento.productoId);
    if (producto) {
        if (movimiento.tipo === 'Entrada') {
            producto.stock -= movimiento.cantidad;
        } else {
            producto.stock += movimiento.cantidad;
        }
        guardarProductos(productos);
    }
    
    // Eliminar movimiento
    const movimientosFiltrados = movimientos.filter(m => m.movimientoId !== parseInt(movimientoId));
    guardarMovimientos(movimientosFiltrados);
    
    return true;
}

// ============================================
// FUNCIONES DE ESTADÍSTICAS
// ============================================

/**
 * Obtener estadísticas del inventario
 */
function obtenerEstadisticasInventario() {
    const productos = obtenerProductos();
    const categorias = obtenerCategorias();
    const movimientos = obtenerMovimientos();
    
    const productosActivos = productos.filter(p => p.activo);
    const stockTotal = productos.reduce((sum, p) => sum + (p.stock || 0), 0);
    const valorTotal = productos.reduce((sum, p) => sum + ((p.stock || 0) * (p.precio || 0)), 0);
    const productosStockBajo = productos.filter(p => {
        const stockMinimo = p.stockMinimo || 10;
        return p.stock <= stockMinimo && p.activo;
    });
    
    const entradas = movimientos.filter(m => m.tipo === 'Entrada');
    const salidas = movimientos.filter(m => m.tipo === 'Salida');
    
    return {
        totalProductos: productos.length,
        productosActivos: productosActivos.length,
        stockTotal: stockTotal,
        valorTotal: valorTotal,
        productosStockBajo: productosStockBajo.length,
        totalCategorias: categorias.length,
        totalMovimientos: movimientos.length,
        totalEntradas: entradas.reduce((sum, m) => sum + m.cantidad, 0),
        totalSalidas: salidas.reduce((sum, m) => sum + m.cantidad, 0)
    };
}

/**
 * Obtener productos con stock bajo
 */
function obtenerProductosStockBajo() {
    const productos = obtenerProductos();
    return productos.filter(p => {
        const stockMinimo = p.stockMinimo || 10;
        return p.stock <= stockMinimo && p.activo;
    });
}

// ============================================
// FUNCIONES DE VALIDACIÓN
// ============================================

/**
 * Validar si hay stock suficiente
 */
function validarStockSuficiente(productoId, cantidad) {
    const producto = obtenerProductoPorId(productoId);
    if (!producto) {
        return { valido: false, mensaje: 'Producto no encontrado' };
    }
    
    if (producto.stock < cantidad) {
        return {
            valido: false,
            mensaje: `Stock insuficiente. Disponible: ${producto.stock}, solicitado: ${cantidad}`
        };
    }
    
    return { valido: true };
}

/**
 * Validar stock mínimo
 */
function validarStockMinimo(productoId) {
    const producto = obtenerProductoPorId(productoId);
    if (!producto) {
        return { valido: false, mensaje: 'Producto no encontrado' };
    }
    
    const stockMinimo = producto.stockMinimo || 10;
    const estaBajo = producto.stock <= stockMinimo;
    
    return {
        valido: !estaBajo,
        estaBajo: estaBajo,
        stock: producto.stock,
        stockMinimo: stockMinimo,
        mensaje: estaBajo ? `Stock bajo (${producto.stock}/${stockMinimo})` : 'Stock normal'
    };
}

// ============================================
// FUNCIONES DE UTILIDAD
// ============================================

/**
 * Formatear fecha
 */
function formatearFecha(fecha) {
    const d = new Date(fecha);
    return d.toLocaleDateString('es-CO', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit'
    });
}

/**
 * Formatear moneda
 */
function formatearMoneda(valor) {
    return new Intl.NumberFormat('es-CO', {
        style: 'currency',
        currency: 'COP'
    }).format(valor);
}

/**
 * Obtener usuario actual
 */
function obtenerUsuarioActual() {
    try {
        const sesion = JSON.parse(localStorage.getItem('sesionActiva') || '{}');
        return sesion.usuario || 'Sistema';
    } catch (e) {
        return 'Sistema';
    }
}

// Exportar funciones para uso global
window.Inventario = {
    productos: {
        obtener: obtenerProductos,
        guardar: guardarProductos,
        obtenerPorId: obtenerProductoPorId,
        crear: crearProducto,
        actualizar: actualizarProducto,
        eliminar: eliminarProducto
    },
    categorias: {
        obtener: obtenerCategorias,
        guardar: guardarCategorias,
        crear: crearCategoria,
        obtenerPorId: obtenerCategoriaPorId
    },
    movimientos: {
        obtener: obtenerMovimientos,
        guardar: guardarMovimientos,
        crear: crearMovimiento,
        eliminar: eliminarMovimiento
    },
    estadisticas: {
        obtener: obtenerEstadisticasInventario,
        productosStockBajo: obtenerProductosStockBajo
    },
    validacion: {
        stockSuficiente: validarStockSuficiente,
        stockMinimo: validarStockMinimo
    },
    utilidades: {
        formatearFecha: formatearFecha,
        formatearMoneda: formatearMoneda,
        obtenerUsuario: obtenerUsuarioActual
    }
};

console.log('✅ Módulo de Inventario inicializado correctamente');


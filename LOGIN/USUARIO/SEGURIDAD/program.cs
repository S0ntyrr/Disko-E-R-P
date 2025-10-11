using Microsoft.AspNetCore.Authentication.Cookies;
using DiskoERP.Core.Services.Interfaces;
using DiskoERP.Core.Services.Implementations;
using LOGIN.USUARIO.SEGURIDAD.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --------------------------
// CONFIGURACIÓN DE SERVICIOS
// --------------------------
builder.Services.AddControllersWithViews();

// Autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccesoDenegado";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
            ? CookieSecurePolicy.SameAsRequest
            : CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

// Servicios personalizados
builder.Services.AddScoped<IAuthService, AuthService>();

// Sesión
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
        ? CookieSecurePolicy.SameAsRequest
        : CookieSecurePolicy.Always;
});

// Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Cultura regional (Colombia)
var cultureInfo = new System.Globalization.CultureInfo("es-CO");
System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// --------------------------
// PIPELINE DE MIDDLEWARES
// --------------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// --------------------------
// RUTAS PRINCIPALES
// --------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

// Ruta directa al dashboard
app.MapControllerRoute(
    name: "dashboard",
    pattern: "Dashboard/{action=Index}/{id?}",
    defaults: new { controller = "Dashboard", action = "Index" });

// Módulos del ERP
app.MapControllerRoute(
    name: "proveedores",
    pattern: "Proveedores/{action=Index}/{id?}",
    defaults: new { controller = "Proveedor", action = "Index" });

app.MapControllerRoute(
    name: "inventario",
    pattern: "Inventario/{action=Index}/{id?}",
    defaults: new { controller = "Producto", action = "Index" });

app.MapControllerRoute(
    name: "clientes",
    pattern: "Clientes/{action=Index}/{id?}",
    defaults: new { controller = "Cliente", action = "Index" });

// Módulo de administración de usuarios
app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{action=Index}/{id?}",
    defaults: new { controller = "Admin", action = "Index" });

app.Run();

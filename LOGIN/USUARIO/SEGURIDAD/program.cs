using Microsoft.AspNetCore.Authentication.Cookies;
using DiskoERP.Core.Services.Interfaces;
using DiskoERP.Core.Services.Implementations;
using LOGIN.USUARIO.SEGURIDAD.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuración básica
builder.Services.AddControllersWithViews();

// Autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

// Registrar servicios
builder.Services.AddScoped<IAuthService, AuthService>();

// Sesión
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

// Conexión a base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Aplicar cultura regional (opcional)
var cultureInfo = new System.Globalization.CultureInfo("es-CO");
System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Pipeline de la aplicación
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Archivos estáticos, rutas y middlewares
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Aplicar migraciones automáticas (EF Core)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "proveedores",
    pattern: "Proveedores/{action=Index}/{id?}",
    defaults: new { controller = "Proveedor", action = "Index" }
);

app.MapControllerRoute(
    name: "inventario",
    pattern: "Inventario/{action=Index}/{id?}",
    defaults: new { controller = "Producto", action = "Index" }
);

app.MapControllerRoute(
    name: "clientes",
    pattern: "Clientes/{action=Index}/{id?}",
    defaults: new { controller = "Cliente", action = "Index" }
);

app.Run();

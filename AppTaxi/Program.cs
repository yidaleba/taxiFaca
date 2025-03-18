using AppTaxi.Servicios;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<I_Conductor, API_Conductor>();
builder.Services.AddScoped<I_Empresa, API_Empresa>();
builder.Services.AddScoped<I_Horario, API_Horario>();
builder.Services.AddScoped<I_Invitado, API_Invitado>();
builder.Services.AddScoped<I_Propietario, API_Propietario>();
builder.Services.AddScoped<I_Rol, API_Rol>();
builder.Services.AddScoped<I_Usuario, API_Usuario>();
builder.Services.AddScoped<I_Vehiculo, API_Vehiculo>();
builder.Services.AddScoped<I_Transaccion, API_Transaccion>();


//Creacion de Sesiones: 

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(50); // Tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UseExceptionHandler("/Home/Error");         // Errores Con Estilos
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

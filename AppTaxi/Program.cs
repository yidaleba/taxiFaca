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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=Index}/{id?}");

app.Run();

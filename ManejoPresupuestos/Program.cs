using ManejoPresupuestos.Servicios;
using ManejoPresupuestos.Servicios.Interfaces;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IServicioUsuario, ServicioUsuarios>();
builder.Services.AddTransient<IRepositorioTipoCuenta, RepositorioTipoCuenta>();
builder.Services.AddTransient<IRepositorioCuentas, RepositorioCuentas>();
builder.Services.AddTransient<IRepositorioCategorias, RepositorioCategorias>();
builder.Services.AddTransient<IRepositorioTransacciones, RepositorioTransacciones>();
builder.Services.AddAutoMapper(typeof(Program)); //Con esto inicializo el automapper, que recibe algo del tipo program, para configurarle los mapeos se crea una clace profile en la carpeta servicios.


var app = builder.Build();

//Configuracion agregada por mi para que tome el modelo regional de eu-US
var culturasSoportadas = new[] { "en-US" };
var opcionesDeLocalizacion = new RequestLocalizationOptions()
    .SetDefaultCulture(culturasSoportadas[0])
    .AddSupportedCultures(culturasSoportadas)
    .AddSupportedUICultures(culturasSoportadas);

app.UseRequestLocalization(opcionesDeLocalizacion);

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

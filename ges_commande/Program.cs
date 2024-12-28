using ges_commande.Data;
using ges_commande.Services;
using ges_commande.Services.Implement;
using gesDetteWebCS.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql("Host=localhost;Database=ges_commande_csharp;Username=postgres;Password=SMS;Port=5432"));

// initialisation des services
builder.Services.AddScoped<IProduitService, ProduitService>();
builder.Services.AddScoped<ICommandeService, CommandeService>();
builder.Services.AddScoped<IPayementService, PayementService>();
builder.Services.AddScoped<ILivraisonService, LivraisonService>();
builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddControllersWithViews();
// Add services to the container.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Securite/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(25);
    });
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// builder.Services.ConfigureApplicationCookie(options =>
// {
//     options.Events.OnRedirectToAccessDenied = context =>
//     {
//         // Rediriger vers la page d'accueil
//         context.Response.Redirect("/Home/Index");
//         return Task.CompletedTask;
//     };
// });
var app = builder.Build();
// Autoriser les fichiers statiques nécessaires pour le navigateur
app.UseStaticFiles();
// Utilisez SeedData pour insérer les données initiales
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    SeedData.Initialize(services, context);
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Index");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 404)
    {
        context.Response.Redirect("/Securite/Error404");
    }
});


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Securite}/{action=Login}/{id?}");

app.Run();

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KalahariCollarV17.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using KalahariCollarV17.Areas.Identity.Data;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("KalahariCollarV17AuthDbContextConnection") ?? throw new InvalidOperationException("Connection string 'KalahariCollarV17AuthDbContextConnection' not found.");

builder.Services.AddDbContext<KalahariCollarV17AuthDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<KalahariCollarV17AuthDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<PetContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PetContext") ?? throw new InvalidOperationException("Connection string 'PetContext' not found.")));
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();

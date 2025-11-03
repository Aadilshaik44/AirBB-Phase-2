using AirBB.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AirBnbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("AirBnbContext") ?? "Data Source=AirBnb.sqlite"));

builder.Services.AddMemoryCache();
builder.Services.AddSession();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

// enable session before endpoints
app.UseSession();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

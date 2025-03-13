using Microsoft.EntityFrameworkCore;
using MvcNetCoreUtilidades.Helpers;
using ViajesMvcNetCore.Data;
using ViajesMvcNetCore.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<RepositoryHome>();
builder.Services.AddTransient<RepositoryLugar>();
builder.Services.AddTransient<RepositoryUsuarios>();
builder.Services.AddSingleton<HelperPathProvider>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

string connectionString =
    builder.Configuration.GetConnectionString("SqlViajes");
builder.Services.AddDbContext<ViajesContext>
    (options => options.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.UseStaticFiles();
app.MapStaticAssets();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using src;
using src.Core.Domain.Ordering.Services;
using src.Infrastructure.Data;
using src.SharedKernel;

var builder = WebApplication.CreateBuilder();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(60); // We're keeping this low to facilitate testing. Would normally be higher. Default is 20 minutes
    options.Cookie.IsEssential = true;              // Otherwise we need cookie approval
});

builder.Services.AddHttpContextAccessor();

// builder.Services.AddDbContext<ShopContext>(options =>
// {
//     options.UseSqlite($"Data Source={Path.Combine("Data", "shop.db")}");
// });

builder.Services.AddDbContext<ShopContext>(options =>
   options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddMediatR(typeof(Program));
builder.Services.AddScoped<IOrderingService, OrderingService>();

builder.Services.Scan(scan => scan
    .FromCallingAssembly()
        .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
        .AsImplementedInterfaces());

builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    if (app.Environment.IsDevelopment())
    {
        var db = scope.ServiceProvider.GetRequiredService<ShopContext>();
        if (!db.FoodItems.Any())
        {
            FakeData.Init();
            db.FoodItems.AddRange(FakeData.FoodItems);
            db.SaveChanges();
        }
    }
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // add 10 seconds delay to ensure the db server is up to accept connections
        // this won't be needed in real world application
        System.Threading.Thread.Sleep(10000);
        var db = scope.ServiceProvider.GetRequiredService<ShopContext>();
        var created = db.Database.EnsureCreated();
        if (!db.FoodItems.Any())
        {
            FakeData.Init();
            db.FoodItems.AddRange(FakeData.FoodItems);
            db.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

// app.UseHttpsRedirection();

var supportedCultures = new[]
{
            new CultureInfo("en-GB"),
        };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-GB"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();


app.Run();

public partial class Program { }
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SampleProject.Web.Models;
using SampleProject.Web.Services;
using System.Collections.Generic;
using System.Threading.Channels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<FileService>();

builder.Services.AddSingleton(Channel.CreateUnbounded<(string userId, List<Product> products)>()); 
//burda birden fazla parametre oldundan gizli bir tuple ile ekledim 


builder.Services.AddHttpContextAccessor();


builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();


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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

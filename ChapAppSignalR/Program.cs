using ChapAppSignalR.Data;
using ChapAppSignalR.Hubs;
using ChapAppSignalR.Models;
using ChapAppSignalR.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<RabbitMqService>();

builder.Services.AddSignalR();  // S�GNALR SERV�S�N� EKLEMEL�Y�Z

builder.Services.AddSingleton<RabbitMqService>(); //sonradan-------------------
builder.Services.AddHostedService<RabbitMqListener>(); //SONRADAN-------------





builder.Services.AddDbContext<ChatAppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});



//------ Identity Framework Ayarlar� //------
//appdbcontext in alt�nda olmal� identity ayarlar� !!!!!!!!!
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ChatAppDbContext>();
builder.Services.AddMemoryCache(); //bunu eklemezsek de�i�ik bir hata alabiliriz
builder.Services.AddSession(); //cookie autentication (use it if possible rather than jwt Authorization)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();                   //cookie autentication(simplier than jwt Authorization)
//------ Identity Framework Ayarlar� //------
//sonras�nda proje sa� t�k- open in terminal -dotnet run seeddata yaz  





var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    await Seed.SeedUsersAndRolesAsync(app);  //ilk ba�ta buray� kullanm�yoruz alttaki kod sat�r�n� kullan�yoruz. identity framework dan sonra buray� kullan�yoruz
                                             // Seed.SeedData(app);
}



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
app.UseAuthentication();

//app.UseSignalR(route =>
//{
//    route.MapHub<ChatHub>("/Home/Index");
//});

app.UseEndpoints(endpoints =>
{

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapHub<ChatHub>("/chathub");
});

app.Run();

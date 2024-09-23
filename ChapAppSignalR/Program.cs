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

builder.Services.AddSignalR();  // SÝGNALR SERVÝSÝNÝ EKLEMELÝYÝZ

builder.Services.AddSingleton<RabbitMqService>(); //sonradan-------------------
builder.Services.AddHostedService<RabbitMqListener>(); //SONRADAN-------------





builder.Services.AddDbContext<ChatAppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});



//------ Identity Framework Ayarlarý //------
//appdbcontext in altýnda olmalý identity ayarlarý !!!!!!!!!
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ChatAppDbContext>();
builder.Services.AddMemoryCache(); //bunu eklemezsek deðiþik bir hata alabiliriz
builder.Services.AddSession(); //cookie autentication (use it if possible rather than jwt Authorization)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();                   //cookie autentication(simplier than jwt Authorization)
//------ Identity Framework Ayarlarý //------
//sonrasýnda proje sað týk- open in terminal -dotnet run seeddata yaz  





var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    await Seed.SeedUsersAndRolesAsync(app);  //ilk baþta burayý kullanmýyoruz alttaki kod satýrýný kullanýyoruz. identity framework dan sonra burayý kullanýyoruz
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

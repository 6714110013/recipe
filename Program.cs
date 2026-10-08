using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipeApp;
using RecipeApp.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. กำหนด DbContext ให้รองรับทั้ง Local และ Render (ใช้ SQLite)
var connString = builder.Configuration.GetConnectionString("DefaultConnection") 
                 ?? "Data Source=app.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connString));

// 2. ตั้งค่า ASP.NET Core Identity พร้อมเชื่อมต่อ Store และ Token Provider
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddRazorPages();
builder.Services.AddScoped<RecipeService>();

var app = builder.Build();

// 3. สั่ง Auto-Migrate สร้าง/ปรับปรุงตารางฐานข้อมูลอัตโนมัติเมื่อรันแอปบน Render
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred migrating the DB.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
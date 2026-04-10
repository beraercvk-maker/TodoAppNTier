using Microsoft.EntityFrameworkCore;
using TodoAppNTier.Business.DependencyResolvers.Microsoft;
using TodoAppNTier.DataAccess.Contexts; // HATA 2'Yİ ÇÖZEN SATIR
using TodoAppNTier.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    
builder.Services.AddDependencies();

// Identity Sisteme Dahil Ediliyor
builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
    opt.Password.RequireDigit = false; 
    opt.Password.RequireLowercase = false; 
    opt.Password.RequireUppercase = false; 
    opt.Password.RequireNonAlphanumeric = false; 
    opt.Password.RequiredLength = 5; 
})
.AddEntityFrameworkStores<TodoContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/PageNotFound");
app.UseHttpsRedirection();
app.UseRouting();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "node_modules")),
    RequestPath = "/node_modules"
});

// KİMLİK DOĞRULAMA (Authorization'dan ÖNCE olmak zorundadır!)
app.UseAuthentication(); 
app.UseAuthorization();

app.MapStaticAssets();

// O uzun kodun yerine sadece bu satırı kullanabilirsin
app.MapDefaultControllerRoute().WithStaticAssets();

// SİSTEM BAŞLARKEN ÇALIŞACAK "KURUCU" (SEED) KODU
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    // 1. Veritabanında Admin ve Member rolleri yoksa otomatik olarak oluştur!
    if (!await roleManager.RoleExistsAsync("Admin")) 
        await roleManager.CreateAsync(new AppRole { Name = "Admin" });
        
    if (!await roleManager.RoleExistsAsync("Member")) 
        await roleManager.CreateAsync(new AppRole { Name = "Member" });

    // 2. Eğer "admin" adında bir kullanıcı yoksa, anahtar teslim bir hesap yarat!
    var adminUser = await userManager.FindByNameAsync("admin");
    if (adminUser == null)
    {
        var newAdmin = new AppUser 
        { 
            UserName = "admin", 
            Email = "admin@todo.com", 
            Name = "Sistem", 
            Surname = "Yöneticisi" 
        };
        
        // Şifreyi de burada veriyoruz
        // SADECE BU SATIRI DEĞİŞTİR: Şifreyi Admin123! yapıyoruz
var createResult = await userManager.CreateAsync(newAdmin, "Admin123!");
        
        // Hesap başarıyla açıldıysa, ona "Admin" pelerinini (Rolünü) giydir!
        if (createResult.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdmin, "Admin");
        }
    }
}
    
app.Run();

app.Run();
using Microsoft.EntityFrameworkCore;
using TodoAppNTier.Business.DependencyResolvers.Microsoft;
using TodoAppNTier.DataAccess.Contexts; // HATA 2'Yİ ÇÖZEN SATIR
using TodoAppNTier.Entities.Concrete;

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

app.Run();
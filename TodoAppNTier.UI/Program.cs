using Microsoft.EntityFrameworkCore;
using TodoAppNTier.Business.DependencyResolvers.Microsoft;
using TodoAppNTier.Entities.Concrete;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
app.UseAuthorization();

app.MapStaticAssets();

// O uzun kodun yerine sadece bu satırı kullanabilirsin
app.MapDefaultControllerRoute().WithStaticAssets();


app.Run();

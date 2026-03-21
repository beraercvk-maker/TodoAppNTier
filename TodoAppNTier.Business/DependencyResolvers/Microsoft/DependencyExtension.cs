using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoAppNTier.Entities.Concrete;
using TodoAppNTier.DataAccess;
using TodoAppNTier.DataAccess.UnitofWork;
using TodoAppNTier.Business.Interfaces;

using TodoAppNTier.Services.WorkService;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace TodoAppNTier.Business.DependencyResolvers.Microsoft
{
    public static class DependencyExtension
    {
        

       public static void AddDependencies(this IServiceCollection services)
        {
            
            services.AddDbContext<TodoContext>(options =>
            {
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TodoAppNTierDb;Trusted_Connection=True;MultipleActiveResultSets=true;");
                options.LogTo(Console.WriteLine, LogLevel.Information);
            });

            services.AddScoped<IUow, Uow>();
            services.AddScoped<IWorkService, WorkService>();
        }


    }
}
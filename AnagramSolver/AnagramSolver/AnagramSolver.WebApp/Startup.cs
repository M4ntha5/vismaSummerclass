using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnagramSolver.Contracts.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using AnagramSolver.EF.DatabaseFirst.Data;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.EF.CodeFirst;

namespace AnagramSolver.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            Console.Configuration.ReadAppSettingsFile();

            services               
                
                .AddScoped<IUserInterface, Console.UI.UserInterface>()
                .AddScoped<ICookiesHandlerServvice, BusinessLogic.Services.CookiesHandlerService>()

                .AddScoped<IUserLogRepository, BusinessLogic.Repositories.UserLogRepositoryEF>()
                .AddScoped<ICachedWordRepository, BusinessLogic.Repositories.CachedWordRepositoryEF>()
                .AddScoped<IWordRepository, BusinessLogic.Repositories.WordRepositoryEF>()
                .AddScoped<IAdditionalWordRepository, BusinessLogic.Repositories.WordRepositoryEF>()

                //.AddScoped<IUserLogRepository, BusinessLogic.Repositories.UserLogRepositoryDB>()
                //.AddScoped<ICachedWordRepository, BusinessLogic.Repositories.CachedWordRepositoryDB>()
                //.AddScoped<IWordRepository, BusinessLogic.Repositories.WordRepositoryDB>()
                //.AddScoped<IAdditionalWordRepository, BusinessLogic.Repositories.WordRepositoryDB>()

                .AddScoped<IAnagramSolver, BusinessLogic.Services.AnagramSolver>()
                .AddScoped<IWordService, BusinessLogic.Services.WordService>()
                .AddHttpContextAccessor();

            services.AddDbContext<AnagramSolverDBFirstContext>(options =>
                    options.UseSqlServer(Contracts.Utils.Settings.ConnectionString));

            services.AddDbContext<AnagramSolverCodeFirstContext>(options =>
                    options.UseSqlServer(Contracts.Utils.Settings.ConnectionStringCodeFirst));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

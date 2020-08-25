using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Profiles;
using AnagramSolver.EF.CodeFirst;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddCors();
            services.AddControllersWithViews();
            Console.Configuration.ReadAppSettingsFile();

            services

                .AddScoped<ICookiesHandlerService, BusinessLogic.Services.CookiesHandlerService>()

                .AddScoped<IUserLogRepository, BusinessLogic.Repositories.UserLogRepositoryEF>()
                .AddScoped<ICachedWordRepository, BusinessLogic.Repositories.CachedWordRepositoryEF>()
                .AddScoped<IWordRepository, BusinessLogic.Repositories.WordRepositoryEF>()
                .AddScoped<IAdditionalWordRepository, BusinessLogic.Repositories.WordRepositoryEF>()

                //.AddScoped<IAnagramSolver, BusinessLogic.Services.AnagramSolverRest>()

                .AddScoped<IAnagramSolver, BusinessLogic.Services.AnagramSolver>()
                .AddScoped<IWordService, BusinessLogic.Services.WordService>()
                .AddScoped<IUserLogService, BusinessLogic.Services.UserLogService>()
                .AddScoped<ICachedWordService, BusinessLogic.Services.CachedWordService>()
                .AddScoped<ISearchHistoryService, BusinessLogic.Services.SearchHistoryService>()

                .AddScoped<AnagramSolverCodeFirstContext>()

                .AddHttpContextAccessor();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddDbContext<AnagramSolverCodeFirstContext>(options =>
                    options.UseSqlServer(Contracts.Utils.Settings.ConnectionStringDevelopment));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
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

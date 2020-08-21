using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Profiles;
using AnagramSolver.Contracts.Utils;
using AnagramSolver.EF.CodeFirst;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using SoapCore;

namespace AnagramSolver.SOAP.WebService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            Console.Configuration.ReadAppSettingsFile();

            services
                .AddScoped<ICookiesHandlerService, BusinessLogic.Services.CookiesHandlerService>()

                .AddScoped<IUserLogRepository, BusinessLogic.Repositories.UserLogRepositoryEF>()
                .AddScoped<ICachedWordRepository, BusinessLogic.Repositories.CachedWordRepositoryEF>()
                .AddScoped<IWordRepository, BusinessLogic.Repositories.WordRepositoryEF>()
                .AddScoped<IAdditionalWordRepository, BusinessLogic.Repositories.WordRepositoryEF>()

                .AddScoped<IAnagramSolver, BusinessLogic.Services.AnagramSolver>()
                .AddScoped<IUserLogService, BusinessLogic.Services.UserLogService>()
                .AddScoped<ICachedWordService, BusinessLogic.Services.CachedWordService>()
                .AddScoped<IAnagramService, AnagramService>()
                .AddScoped<AnagramSolverCodeFirstContext>()                       

                .AddHttpContextAccessor();


            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);

            services.AddDbContext<AnagramSolverCodeFirstContext>(opt =>
                opt.UseSqlServer(Settings.ConnectionStringDevelopment));

            services.TryAddSingleton<IAnagramService, AnagramService>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           
            app.UseSoapEndpoint<IAnagramService>(path: "/AnagramService.svc", binding: new BasicHttpBinding());

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}

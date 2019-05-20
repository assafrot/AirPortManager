using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.SignalR;
using Server.Hubs;
using Manager.Interfaces;
using Server.Interfaces;
using Server.Services;

namespace Server
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
            services
                .AddDbContext<DAL.AirportDbContext>(opts => opts.UseInMemoryDatabase("airportDb"))
                .AddScoped<DAL.Interfaces.IUnitOfWork, DAL.UnitOfWork>()
                .AddMvc();
            services.AddTransient<IDBSeederService, DBSeederService>();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDBSeederService seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseSignalR(routes =>
            {
                routes.MapHub<AirportHub>("/airport");
            });

            app.UseMvc( routes => {
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}");
            });

            //load airport state
            seeder.JsonSeed(@"StationData");
        }
    }
}

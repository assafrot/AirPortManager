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
using DAL;
using DAL.Interfaces;

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
                .AddDbContext<AirportDbContext>(opts => opts.UseInMemoryDatabase("airportDb"))
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IDBSeederService,DBSeederService>()
                .AddScoped<IAirportStateArchiver,AirportStateArchiver>()
                .AddMvc();

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
            var stationsData = System.IO.File.ReadAllText(@"StationsData.json");
            var stationsLinksData = System.IO.File.ReadAllText(@"StationsLinks.json");
            seeder.JsonSeed(stationsData, stationsLinksData);
        }
    }
}

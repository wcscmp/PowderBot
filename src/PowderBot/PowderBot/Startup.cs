using BusinessLogic;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using WebClient;

namespace PowderBot
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
            // Register services
            services.AddMvc();

            // Use ApplicationInsights
            services.AddApplicationInsightsTelemetry(Configuration);

            // Register configurations
            services.Configure<StorageConfiguration>(Configuration.GetSection("Storage"));
            services.Configure<FacebookConfiguration>(Configuration.GetSection("Facebook"));

            // Solution services registration
            services.AddSingleton<HttpClient, HttpClient>();
            services.AddScoped<IGenericRepository<UserModel>, GenericRepository<UserModel>>();
            services.AddScoped<IGenericRepository<SubscriptionModel>, GenericRepository<SubscriptionModel>>();
            services.AddScoped<UserRepository, UserRepository>();
            services.AddScoped<SubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<CommandFactory, CommandFactory>();
            services.AddScoped<IMessanger, FacebookClient>();
            services.AddScoped<ISnowForecastClient, SnowForecastClient>();
            services.AddScoped<SnowfallChecker, SnowfallChecker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}

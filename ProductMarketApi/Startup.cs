using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;

namespace ProductMarketApi
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
            services.AddSingleton(Configuration);

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );


            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {

                    cfg.Host(new Uri("rabbitmq://localhost:5672/"), configurator =>
                    {
                        configurator.Username("guest");
                        configurator.Password("guest");
                    });


                    cfg.ConfigureJsonSerializer(settings =>
                    {
                        settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                        return settings;
                    });

                    cfg.ConfigureJsonDeserializer(settings =>
                    {
                        settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                        return settings;
                    });
                });
            });

            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // подключаем CORS для кросс платформенных запросов
            app.UseCors(builder => builder.AllowAnyOrigin());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

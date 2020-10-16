using System;
using System.Collections.Generic;
using System.Linq;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductMarketModels;
using ProductMarketServices.Products;
using ServiceProductMarket.Consumers.Products;

namespace ServiceProductMarket
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ProductMarketContext>(options =>
                options.UseSqlServer(connection), ServiceLifetime.Transient);


            services.AddTransient<IProductService, ProductService>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<GetProductsConsumer>();
                x.AddConsumer<AddProductConsumer>();
                //x.AddConsumer<GetCarsConsumer>();




                x.UsingRabbitMq((context, cfg) =>
                {


                    cfg.Host(new Uri("rabbitmq://localhost:5672/"), configurator =>
                    {
                        configurator.Username("guest");
                        configurator.Password("guest");
                    });
                    
                    cfg.ReceiveEndpoint("ProductsQueue", e =>
                    {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(r => r.Interval(2, 100));
                        e.Consumer<GetProductsConsumer>(context);
                        e.Consumer<AddProductConsumer>(context);

                    });


                    //cfg.ReceiveEndpoint("ManufacturerQueue", e =>
                    //{
                    //    e.PrefetchCount = 20;
                    //    e.UseMessageRetry(r => r.Interval(2, 100));
                    //    e.Consumer<ManufacturerConsumer>(context);
                    //});

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

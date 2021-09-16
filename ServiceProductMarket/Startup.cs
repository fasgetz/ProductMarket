using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;
using NETCore.MailKit.Core;
using Newtonsoft.Json;
using ProductMarketModels;
using ProductMarketModels.MassTransit.Requests.Products;
using ProductMarketServices.Basket;
using ProductMarketServices.Categories;
using ProductMarketServices.ElasticSearch;
using ProductMarketServices.EmailServiceNew;
using ProductMarketServices.Fondy;
using ProductMarketServices.PayPal;
using ProductMarketServices.Products;
using ProductMarketServices.ProductsDiscount;
using ProductMarketServices.Stripe;
using ServiceProductMarket.Consumers.Basket;
using ServiceProductMarket.Consumers.Category;
using ServiceProductMarket.Consumers.Discounts;
using ServiceProductMarket.Consumers.Fondy;
using ServiceProductMarket.Consumers.PayPal;
using ServiceProductMarket.Consumers.Products;
using ServiceProductMarket.Consumers.Stripe;

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


            services.AddMemoryCache();

            // Добавляем автомаппер
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient<IProductDiscountService, ProductDiscountService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<IBasketService, BasketService>();
            services.AddTransient<IPayPalService, PayPalService>();
            services.AddTransient<IFondyService, FondyService>();
            services.AddTransient<IStripeService, StripeService>();
            services.AddTransient<ProductMarketServices.EmailServiceNew.IEmalService, ProductMarketServices.EmailServiceNew.EmailService>();

            services.AddMassTransit(x =>
            {
                // Basket
                x.AddConsumer<GetBasketProductConsumer>();
                x.AddConsumer<AddOrderConsumer>();
                x.AddConsumer<GetUserOrdersConsumer>();


                // Discounts
                x.AddConsumer<AddDiscountConsumer>();
                x.AddConsumer<EditDiscountConsumer>();
                x.AddConsumer<GetDiscountsProductConsumer>();
                x.AddConsumer<RemoveDiscountConsumer>();

                // PayPal
                x.AddConsumer<GetUrlPaymentConsumer>();
                x.AddConsumer<ExecutePaymenConsumer>();

                // Fondy
                x.AddConsumer<GetUrlPaymentFondyConsumer>();
                x.AddConsumer<ExecutePaymentStripeConsumer>();

                // Stripe
                x.AddConsumer<GetUrlPaymentStripeConsumer>();

                // Products
                x.AddConsumer<AddProductConsumer>();
                x.AddConsumer<EditProductConsumer>();
                x.AddConsumer<GetProductsConsumer>();
                x.AddConsumer<GetProductsNameConsumer>();
                x.AddConsumer<GetNewsProductsConsumer>();
                x.AddConsumer<ExistProductConsumer>();
                x.AddConsumer<GetRandomProductsConsumer>();
                x.AddConsumer<GetProductIdConsumer>();

                // Categories
                x.AddConsumer<GetProductsOnSubcategoryInCategoryConsumer>();
                x.AddConsumer<GetSubCategoriesOnCategoryConsumer>();
                x.AddConsumer<AddCategoryConsumer>();
                x.AddConsumer<AddSubCategoryConsumer>();
                x.AddConsumer<EditCategoryConsumer>();



                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri("rabbitmq://192.168.1.66:5672/"), configurator =>
                    {
                        configurator.Username("guest");
                        configurator.Password("guest");
                    });


                    #region Продукты

                    // Очередь для админ действий
                    cfg.ReceiveEndpoint("ProductsAdminQueue", e =>
                    {
                        e.PrefetchCount = 16;

                        e.Consumer<AddDiscountConsumer>(context);
                        e.Consumer<EditDiscountConsumer>(context);
                        e.Consumer<RemoveDiscountConsumer>(context);


                        e.Consumer<AddProductConsumer>(context);
                        e.Consumer<EditProductConsumer>(context);
                    });


                    // Очередь для обычных пользователей
                    cfg.ReceiveEndpoint("ProductsQueue", e =>
                    {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(r => r.Interval(2, 100));

                        // Basket
                        e.Consumer<GetBasketProductConsumer>(context);
                        e.Consumer<AddOrderConsumer>(context);
                        e.Consumer<GetUserOrdersConsumer>(context);

                        // PayPal
                        e.Consumer<GetUrlPaymentConsumer>(context);
                        e.Consumer<ExecutePaymenConsumer>(context);

                        // Fondy
                        e.Consumer<GetUrlPaymentFondyConsumer>(context);

                        // Stripe
                        e.Consumer<GetUrlPaymentStripeConsumer>(context);
                        e.Consumer<ExecutePaymentStripeConsumer>(context);

                        // Discount
                        e.Consumer<GetDiscountsProductConsumer>(context);


                        e.Consumer<GetProductsConsumer>(context);
                        e.Consumer<GetProductsNameConsumer>(context);
                        e.Consumer<GetNewsProductsConsumer>(context);
                        e.Consumer<GetRandomProductsConsumer>(context);
                        e.Consumer<ExistProductConsumer>(context);
                        e.Consumer<GetProductIdConsumer>(context);

                    });

                    #endregion

                    #region Категории

                    cfg.ReceiveEndpoint("CategoriesQueue", e =>
                    {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(r => r.Interval(2, 100));

                        e.Consumer<GetProductsOnSubcategoryInCategoryConsumer>(context);
                        e.Consumer<GetSubCategoriesOnCategoryConsumer>(context);
                    });


                    cfg.ReceiveEndpoint("CategoriesAdminQueue", e =>
                    {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(r => r.Interval(2, 100));

                        e.Consumer<AddCategoryConsumer>(context);
                        e.Consumer<AddSubCategoryConsumer>(context);
                        e.Consumer<EditCategoryConsumer>(context);
                    });

                    #endregion

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

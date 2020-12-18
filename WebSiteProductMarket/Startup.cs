using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.WebEncoders;
using Microsoft.IdentityModel.Tokens;
using WebSiteProductMarket.Identity;
using WebSiteProductMarket.Service;

namespace WebSiteProductMarket
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

            services.AddCors();
            services.AddHttpContextAccessor();

            // получаем строку подключения из файла конфигурации
            string userConnect = Configuration.GetConnectionString("UsersIdentityConnection");

            // Подключаем контекст работы с пользователями Identity
            services.AddDbContext<UsersContext>(options =>
                options.UseSqlServer(userConnect));


            // Добавляем сервис администрирования профилем
            services.AddScoped<IProfileService, ProfileService>();


            services.AddIdentity<User, IdentityRole>(i =>
            {                
                i.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<UsersContext>()
               .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = "https://localhost:44336/",
                    ValidIssuer = "https://localhost:44336/",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"])),
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddAuthorization(config =>
            {
                config.AddPolicy(Policies.Admin, Policies.AdminPolicy());
                config.AddPolicy(Policies.User, Policies.UserPolicy());
            });


            // Переадресация в случае неавторизованности
            services.ConfigureApplicationCookie(options => options.LoginPath = "/Home/Index");



            //Чтобы кирилические символы не переводились в соответствующий Unicode Hex Character Code
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });

            services.AddMemoryCache();
            services.AddSession(options =>
            {                
                options.Cookie.Name = ".AspNetCore.Session";
                options.IdleTimeout = TimeSpan.FromDays(1); // Сбрасывание сессии, если нет активности в течении суток
                options.Cookie.IsEssential = true;
            });

            services.AddResponseCompression(options => options.EnableForHttps = true);

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

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

            //// подключаем CORS для кросс платформенных запросов
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse =
                    r =>
                    {
                        string path = r.File.PhysicalPath;
                        if (path.EndsWith(".css") || path.EndsWith(".js") || path.EndsWith(".gif") || path.EndsWith(".jpg") || path.EndsWith(".png") || path.EndsWith(".svg"))
                        {
                            TimeSpan maxAge = new TimeSpan(7, 0, 0, 0);
                            r.Context.Response.Headers.Append("Cache-Control", "max-age=" + maxAge.TotalSeconds.ToString("0"));
                        }
                    }
            });

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseResponseCompression();

            app.UseEndpoints(endpoints =>
            {
  

                endpoints.MapControllerRoute(
                    name: "admin",
                    pattern: "admin/products/{action}",
                    defaults: new { controller = "AdminProducts", action = "Index" }
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "admin",
                    pattern: "admin/",
                    defaults: new { controller = "Admin", action = "Index" });
            });
        }
    }
}

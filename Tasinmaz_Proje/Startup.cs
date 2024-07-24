using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Tasinmaz_Proje.DataAccess;
using Tasinmaz_Proje.Services;
using System;
using Tasinmaz_Proje.Entities;
using Tasinmaz_Proje.Business.Abstract;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


namespace Tasinmaz_Proje
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("Appsettings:Token").Value);
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddScoped<IUserService, UserService>();


            services.AddControllersWithViews();

            services.AddDbContext<TasinmazDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddScoped<IDurumService, DurumService>();
            services.AddScoped<ITasinmazBilgiService, TasinmazBilgiService>();
            services.AddScoped<IIlceService, IlceService>();
            services.AddScoped<IIlService, IlService>();
            services.AddScoped<IIslemTipService, IslemTipService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IMahalleService, MahalleService>();

            // Swagger konfigürasyonu
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "TasinmazYonetimi API",
                    Version = "v1",
                    Description = "Taþýnmaz Yönetimi API belgeleri",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Destek Ekibi",
                        Email = "destek@tasinmaz.com",
                        Url = new Uri("https://tasinmaz.com"),
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AllowAll"); // CORS politikasýný uygula

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Swagger'ý kullanýn
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TasinmazYonetimi API v1");
                c.RoutePrefix = string.Empty; // Swagger UI'nýn kök URL'de olmasýný saðlar
            });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using BLL.App;
using Contracts.BLL.App;
using Contracts.BLL.Base;
using Contracts.DAL.App;
using DAL.APP.EF;
using DAL.APP.EF.AppDataInit;
using Domain.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApp.Areas.IdentityErrors;
using WebApp.Helpers;
#pragma warning disable 4014

#pragma warning disable 1591

namespace WebApp
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
            services.AddDbContext<AppDbContext>(options =>
                options
                    .UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection"))
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
            );
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();

            services.AddScoped<IAppBLL, AppBLL>();

            JwtSecurityTokenHandler.DefaultInboundClaimFilter.Clear();
            services.AddAuthentication(option =>
                {
                    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddCookie(options => { options.SlidingExpiration = true; })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["Jwt:JwtIssuer"],
                        ValidAudience = Configuration["Jwt:JwtIssuer"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:JwtKey"])),
                        ClockSkew = TimeSpan.Zero // Remove delay of token when it expires
                    };
                });

            services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddDefaultUI()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddErrorDescriber<LocalizedCustomIdentityErrorDescriber>()
                .AddDefaultTokenProviders();

            services.AddControllersWithViews();

            // for front end to use api
            services.AddCors(options =>
            {
                options.AddPolicy("CorsAllowAll", builder =>
                    {
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                        builder.AllowAnyOrigin();
                    }
                );
            });
            services.AddAutoMapper(
                typeof(DAL.App.DTO.MappingProfiles.AutoMapperProfile),
                typeof(BLL.App.DTO.MappingProfiles.AutoMapperProfile),
                typeof(PublicApiDTO.v1.v1.Mappers.MappingProfiles.AutoMapperProfiles));

            //Support for api versioning
            services.AddApiVersioning(options => { options.ReportApiVersions = true; });
            //Support for m2m api documentations
            services.AddVersionedApiExplorer(options => { options.GroupNameFormat = "'v'VVV"; });
            //Support for human readable documentation from machine2machine docs
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                // TODO: Should be in appsettings.json
                var supportedCultures = new[]
                {
                    new CultureInfo("et"),
                    new CultureInfo("en"),
                };
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.DefaultRequestCulture = new RequestCulture("en-GB", "en");
                options.SetDefaultCulture("en-GB");
                options.RequestCultureProviders = new List<IRequestCultureProvider>()
                {
                    // first we try to get it from query, then we try to get culture from the cookie
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                };
            });
            services.AddSingleton<IConfigureOptions<MvcOptions>, ConfigureModelBindingLocalization>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            SetupAppData(app, Configuration);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var apiVersionDescription in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{apiVersionDescription.GroupName}/swagger.json",
                        apiVersionDescription.GroupName.ToUpperInvariant()
                    );
                }
            });

            app.UseStaticFiles();

            // for front end to use api
            app.UseCors("CorsAllowAll");
            app.UseRouting();

            app.UseRequestLocalization(
                app.ApplicationServices
                    .GetService<IOptions<RequestLocalizationOptions>>()?.Value);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        private static async Task SetupAppData(IApplicationBuilder app, IConfiguration configuration)
        {
            using var serviceScope =
                app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            await using var ctx = serviceScope.ServiceProvider.GetService<AppDbContext>();

            if (ctx != null)
            {
                //In case of testing, skip seeding data setup
                if (ctx!.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory") return;

                if (configuration.GetValue<bool>("AppData:DropDatabase"))
                {
                    Console.Write(@"Drop database");
                    DataInit.DropDatabase(ctx);
                    Console.WriteLine(@" - done");
                }

                if (configuration.GetValue<bool>("AppData:Migrate"))
                {
                    Console.Write(@"Migrate database");
                    DataInit.MigrateDatabase(ctx);
                    Console.WriteLine(@" - done");
                }

                if (configuration.GetValue<bool>("AppData:SeedIdentity"))
                {
                    using var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();
                    using var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<AppRole>>();
                    Console.WriteLine(@"Seeding identity database");
                    if (userManager != null && roleManager != null)
                    {
                        DataInit.SeedIdentity(userManager, roleManager, configuration);
                    }
                    else
                    {
                        Console.Write(@"No User manager or role manager!");
                    }

                    Console.WriteLine(@" - done");
                }


                if (configuration.GetValue<bool>("AppData:SeedData"))
                {
                    Console.WriteLine(@"Seed app data database");
                    await DataInit.SeedAppTestData(ctx);
                    Console.WriteLine(@" - done");
                }
            }
        }
    }
}
using E_Commerce.Data;
using E_Commerce.Helper;
using E_Commerce.Logic;
using E_Commerce.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

namespace E_Commerce
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDALServices(builder.Configuration);
            builder.Services.AddBLLServices();
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                            .AddEntityFrameworkStores<ECommerceDbContext>()
                            .AddDefaultTokenProviders();

            builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JwtSettings"));

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                options.User.RequireUniqueEmail = true;

                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            });

            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JWTSettings>() ?? new JWTSettings();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtSettings.SecretKey)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminsOnly", policy => policy.RequireRole("Admin"));
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var rootPath = builder.Environment.ContentRootPath;
            var staticFilesPath = Path.Combine(rootPath, "Files");
            if(!Directory.Exists(staticFilesPath))
            {
                Directory.CreateDirectory(staticFilesPath);
            }
            builder.Services.Configure<StaticFileOptions>(cfg =>
            {
                cfg.FileProvider = new PhysicalFileProvider(staticFilesPath);
                cfg.RequestPath = "Files";
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();
                db.Database.Migrate();

                var seeder = scope.ServiceProvider.GetRequiredService<ApplicationSeeder>();
                await seeder.SeedAllAsync(db, scope.ServiceProvider);
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

                await AspUsersDataSeeder.SeedAdminAsync(roleManager, userManager);
                await AspUsersDataSeeder.SeedCustomerRoleAsync(roleManager);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

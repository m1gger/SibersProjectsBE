using Application;
using Application.Configuration;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using WebAPI.Middlewares;

namespace WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("FrontendPolicy", builder =>
                {
                    builder
                        .WithOrigins(
                            "http://localhost:3000",
                            "http://localhost:5173", 
                            "http://localhost:4173",
                            "http://localhost:8080",
                            "https://localhost:3000",
                            "https://localhost:5173"
                        )
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetPreflightMaxAge(TimeSpan.FromMinutes(5)); 
                });

                options.AddPolicy("Development", builder =>
                {
                    builder
                        .SetIsOriginAllowed(origin => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddPersistence(Configuration);
            services.AddApplication();

            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<SibersDbContext>()
            .AddDefaultTokenProviders();
            
            services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));
            var jwtSettings = Configuration.GetSection("Jwt").Get<JwtSettings>();
            var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
                        var userId = context.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                        if (userId == null)
                        {
                            context.Fail("Missing user ID in token");
                            return;
                        }

                        var user = await userManager.FindByIdAsync(userId);
                        if (user == null)
                        {
                            context.Fail("User not found");
                            return;
                        }
                    }
                };
            });

            services.AddSwaggerGen(config =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Sibers TEST API",
                    Version = "v1",
                    Description = "Документация API тестового задания"
                });

                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Введите ваш JWT токен в формате Bearer."
                });

                config.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (string.IsNullOrEmpty(env.WebRootPath))
                env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            if (!Directory.Exists(env.WebRootPath))
                Directory.CreateDirectory(env.WebRootPath);

           // app.UseDeveloperExceptionPage();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sibers TEST API v1");
                c.RoutePrefix = string.Empty;
            });
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseCors("Development");
            }
            else
            {
                app.UseCors("FrontendPolicy");
            }

            app.UseRouting();
            
            
            app.UseHttpsRedirection();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapMethods("{*path}", new[] { "OPTIONS" }, context =>
                {
                    context.Response.StatusCode = 200;
                    return Task.CompletedTask;
                });
            });
            
        }
    }
}

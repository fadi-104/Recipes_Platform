using BusinessLogicLayer.Mapper;
using BusinessLogicLayer.Services.AiService;
using BusinessLogicLayer.Services.CategoryService;
using BusinessLogicLayer.Services.FavouriteService;
using BusinessLogicLayer.Services.ImageService;
using BusinessLogicLayer.Services.MessageService;
using BusinessLogicLayer.Services.RatingService;
using BusinessLogicLayer.Services.RecipecService;
using BusinessLogicLayer.Services.Storage;
using BusinessLogicLayer.Services.UserService;
using DataAccessLayer;
using DataAccessLayer.Repository.CategoryRepository;
using DataAccessLayer.Repository.FavouriteRepository;
using DataAccessLayer.Repository.IdentityRepository;
using DataAccessLayer.Repository.ImageReository;
using DataAccessLayer.Repository.MessageRepository;
using DataAccessLayer.Repository.RatingRepository;
using DataAccessLayer.Repository.RecipecRepository;
using DomainLayer.Entites;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Project_Structure.Middlewares;
using Serilog;
using System.Text;



namespace Project_Structure
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddAutoMapper(typeof(GeneralProfile));

            builder.Services.AddHttpClient<IAiService,AiService>();

            //repo
            builder.Services.AddScoped<Seed>();
            builder.Services.AddScoped<IAppUserManager, AppUserManager>();
            builder.Services.AddScoped<IAppRoleManager, AppRoleManager>();
            builder.Services.AddScoped<IAppSignInManager, AppSignInManager>();
            builder.Services.AddScoped<IRecipecRepository, RecipecRepository>();
            builder.Services.AddScoped<IRatingRepository, RatingRepository>();
            builder.Services.AddScoped<IImageRepository, ImageRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IFavouriteRepository, FavouriteRepository>();


            //services
            builder.Services.AddScoped<IStorageService, FileDiskStorageService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRecipecService, RecipecService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IRatingService, RatingService>();
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<IFavouriteService, FavouriteService>();

            builder.Services.AddSignalR();

            builder.Services.AddMemoryCache();

            builder.Services.AddControllers();

            var con = builder.Configuration.GetConnectionString("DefaultConnectionString");

            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(con);
            });

            builder.Services.AddIdentity<UserApp,RoleApp>(option =>
            {
                // Password Settings
                option.Password.RequiredLength = 8;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireLowercase = false;
                option.Password.RequireDigit = true;

                // Lockout Settings
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                option.Lockout.MaxFailedAccessAttempts = 5;

                option.User.RequireUniqueEmail = true;

            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

                };
            });

            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("V1", new OpenApiInfo
                {
                    Version = "V1",
                    Title = "Wasfaty",

                });
            });

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Error()
                .WriteTo.File("Logger/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddCors(option =>             {
                option.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                         
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/V1/swagger.json", "Wasfaty");
            });

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.MapControllers();

            using(var scope = app.Services.CreateScope())
            {
                var seedHelper = scope.ServiceProvider.GetService<Seed>();
                
                if(seedHelper is not null)
                {
                    await seedHelper.UseSeedAsync();
                }
            }


            app.MapHub<ChatHub.ChatHub>("/chatHub");

            app.Run();
        }
    }
}

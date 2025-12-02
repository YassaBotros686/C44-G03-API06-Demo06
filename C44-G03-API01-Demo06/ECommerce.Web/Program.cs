
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.Persistence.Data.DataSeed;
using ECommerce.Persistence.Data.DbContexts;
using ECommerce.Persistence.IdentityData.DbContexts;
using ECommerce.Persistence.Repositories;
using ECommerce.Services;
using ECommerce.Services.MappingProfiles;
using ECommerce.ServicesAbstraction;
using ECommerce.Web.CustomMiddleWares;
using ECommerce.Web.Extensions;
using ECommerce.Web.Factories;   
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;    
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace ECommerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                // ConnectionString
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                // ConnectionString
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
                // Add-Migration "IdentityTableCreate" -OutputDir Persistence/IdentityData/Migrations -Context StoreIdentityDbContext
            });

            //builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<StoreIdentityDbContext>();
            // ==
            builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();

            builder.Services.AddKeyedScoped<IDataInitializer, DataInitializer>("Default");
            builder.Services.AddKeyedScoped<IDataInitializer, IdentityDataInitializer>("Identity");

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(X => X.AddProfile<ProductProfile>());
            builder.Services.AddAutoMapper(X => X.AddProfile<BasketProfile>());
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<IBasketService, BasketService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationResponse;
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection"));
            });

            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddScoped<ICacheService, CacheService>();

            var app = builder.Build();

            #region DataSeed

            await app.MigrateDbAsync();
            await app.MigrateIdentityDbAsync();
            await app.SeedDbAsync();
            await app.SeedIdentityDbAsync();

            #endregion



            // Configure the HTTP request pipeline.
            // Exceptions Here

            app.UseMiddleware<ExceptionHandlerMiddleWare>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

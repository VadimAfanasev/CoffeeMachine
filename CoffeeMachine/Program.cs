
using CoffeeMachine.Models.Data;
using CoffeeMachine.Services;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMachine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run(async context =>
            {
                var coffeeBuyServices = context.RequestServices.GetService<CoffeeBuyServices>();
                await context.Response.WriteAsync("CoffeeBuyServices Dependency Resolved");
            });

            //app.UseServices(ConfigureServices);

            //app.Run(Configure);

            //void ConfigureServices(IServiceCollection services)
            //{
            //services.AddTransient<CalculateChange>();
            //services.AddTransient<DecrementAvailableNotes>();
            //services.AddTransient<IncrementAvailableNotes>();
            //services.AddTransient<IncrementCoffeeBalances>();

            //services.AddTransient<CoffeeBuyServices>();
            //}

            //void Configure(IApplicationBuilder app)
            //{
            //    app.Run(async context =>
            //    {
            //        var coffeeBuyServices = context.RequestServices.GetService<CoffeeBuyServices>();
            //        await context.Response.WriteAsync("CoffeeBuyServices Dependency Resolved");
            //    });
            //}

            app.Run();
        }
    }
}
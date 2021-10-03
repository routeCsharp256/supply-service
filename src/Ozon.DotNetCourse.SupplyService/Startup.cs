using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ozon.DotNetCourse.SupplyService.Background;
using Ozon.DotNetCourse.SupplyService.Domain.Interfaces.Notifiers;
using Ozon.DotNetCourse.SupplyService.Domain.Interfaces.Repositories;
using Ozon.DotNetCourse.SupplyService.Domain.Interfaces.Services;
using Ozon.DotNetCourse.SupplyService.GrpcServices;
using Ozon.DotNetCourse.SupplyService.Infrastructure.Kafka;
using Ozon.DotNetCourse.SupplyService.Infrastructure.Postgres.Abstractions;
using Ozon.DotNetCourse.SupplyService.Infrastructure.Postgres.Database;
using Ozon.DotNetCourse.SupplyService.Infrastructure.Postgres.Repositories;

namespace Ozon.DotNetCourse.SupplyService
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Infrastructure.Postgres.Configuration>(_configuration.GetSection("DbConfiguration"));
            services.Configure<Infrastructure.Kafka.Configuration>(_configuration.GetSection("KafkaConfiguration"));
            
            services
                .AddScoped<IDbConnectionFactory, DbConnectionFactory>()
                .AddScoped<ISupplyRepository, SupplyRepository>()
                .AddScoped<ISupplyItemRepository, SupplyItemRepository>()
                .AddScoped<ISupplyService, Services.SupplyService>()
                .AddScoped<ITransactionFactory, TransactionFactory>()
                .AddSingleton<ISupplyShippedNotifier, SupplyShippedProducer>();
            services.AddMvcCore().AddApiExplorer();
            services.AddSwaggerGen();
            services.AddGrpc();
            services.AddHostedService<SupplySender>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<SupplyGrpcService>();
            });
        }
    }
}
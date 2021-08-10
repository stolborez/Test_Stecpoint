using System;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Stecpoint.Core;
using Stecpoint.Core.Commands;
using Stecpoint.Data.Contexts;
using Stecpoint.Infrastructure.Repositories;

namespace Stecpoint.ServiceA
{
    public class Startup
    {
        static bool? _isRunningInContainer;
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        IConfiguration Configuration { get; }
        static bool IsRunningInContainer =>
            _isRunningInContainer ??= bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inContainer) && inContainer;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddUserCommandValidator>());
            services.AddTransient<IValidator<AddUserCommand>, AddUserCommandValidator>();

            services.AddDbContext<DataContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING_POSTGRES")));
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            
            
            services.AddMassTransit(mt  =>
            {
                mt.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                    cfg.Host(IsRunningInContainer ? Environment.GetEnvironmentVariable("RABBITMQ_HOST") : "localhost", h =>
                    {
                        h.Username(Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_USER"));
                        h.Password(Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_PASS"));
                    });
                });
            });
            
            services.AddMassTransitHostedService();
            
            services.AddApiVersioning();
            
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Stecpoint.ServiceA", Version = "v1"}); });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stecpoint.ServiceA v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
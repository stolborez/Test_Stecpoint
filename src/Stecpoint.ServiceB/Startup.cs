using System;
using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Stecpoint.Core;
using Stecpoint.Core.Commands;
using Stecpoint.Core.DTO;
using Stecpoint.Core.MapperProfiles;
using Stecpoint.Core.Queries;
using Stecpoint.Data;
using Stecpoint.Data.Contexts;
using Stecpoint.Infrastructure.Repositories;

namespace Stecpoint.ServiceB
{
    public class Startup
    {
        static bool? _isRunningInContainer;
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        IConfiguration Configuration { get; }

        static bool IsRunningInContainer => _isRunningInContainer ??= bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inContainer) && inContainer;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApiVersioning();
            services.AddDbContext<DataContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING_POSTGRES")));
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            services.AddTransient(typeof(IRequestHandler<LinkUserCommand, User>), typeof(LinkUserCommand.LinkUserCommandHandler));
            services.AddTransient(typeof(IRequestHandler<AddUserCommand, User>), typeof(AddUserCommand.AddUserCommandHandler));
            services.AddTransient(typeof(IRequestHandler<GetUsersQuery, List<UserDto>>), typeof(GetUsersQuery.GetUsersHandler));
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(typeof(Startup));
            services.AddAutoMapper(typeof(Profile), typeof(UserProfile));
            
            
            
            services.AddMassTransit(mt  =>
            {
                mt.AddConsumer<EventConsumer>();
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
            /**/

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Stecpoint.ServiceB", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stecpoint.ServiceB v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
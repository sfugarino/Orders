using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OrderSystem.Data;
using OrderSystem.Data.Entities;
using OrderSystem.Messaging;
using RabbitMQ.Client;

namespace OrderSystem.Api
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
            

            services.AddDbContext<RestaurantDbContext>(opt => opt.UseSqlite(Configuration.GetConnectionString("restaurant")));

            services.AddSingleton<IConnectionFactory>(ctx =>
            {
                var rabbitMQConnectionSettings = this.Configuration["RabbitMQ"];
                return new ConnectionFactory()
                {
                    HostName = Configuration["RabbitMQ:HostName"],
                    UserName = Configuration["RabbitMQ:UserName"],
                    Password = Configuration["RabbitMQ:Password"]
                };
            });

            services.AddSingleton<IRabbitMQConnection, RabbitMQConnection>();
            services.AddSingleton<IOrderReceiver, OrderReceiver>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IOrderQueueChannelAdapter, OrderQueueChannelAdapter>();
            services.AddScoped<IStationQueueChannelAdapter, StationOueueChannelAdapter>();

            services.AddSingleton<IOrderForwarder>(ctx => {
                var channel = ctx.GetRequiredService<System.Threading.Channels.Channel<OrderItem>>();
                var logger = ctx.GetRequiredService<ILogger<OrderForwarder>>();
                return new OrderForwarder(logger, channel.Writer);
            });

            services.AddSingleton<IEnumerable<IOrderConsumer>>(ctx => {
                var channel = ctx.GetRequiredService<System.Threading.Channels.Channel<Order>>();
                var logger = ctx.GetRequiredService<ILogger<OrderConsumer>>();
                var repo = ctx.GetRequiredService<IRepository<Order>>();

                var consumers = Enumerable.Range(1, 10)
                                          .Select(i => new OrderConsumer(i, logger, channel.Reader, repo))
                                          .ToArray();
                return consumers;
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderSystem.Api", Version = "v1" });
            });

            services.AddMemoryCache();

            services.AddHostedService<BackgroundWorker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderSystem.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

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
using NB.EFCore;
using NB.EFCore.Repositories;
using NyhedsBlog_Backend.Core.IServices;
using NyhedsBlog_Backend.Core.Models;
using NyhedsBlog_Backend.Core.Models.Post;
using NyhedsBlog_Backend.Core.Models.Subscription;
using NyhedsBlog_Backend.Core.Models.User;
using NyhedsBlog_Backend.Domain.IRepositories;
using NyhedsBlog_Backend.Domain.Services;

namespace NB.WebAPI
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
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "NB.WebAPI", Version = "v1"}); });
            
            var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

            //Main Application DB CTX
            services.AddDbContext<NbContext>(opt =>
            {
                opt
                    .UseLoggerFactory(loggerFactory)
                    .UseSqlite("Data Source=appDb.db");
            });
            
            //CORS Policy Setup
            services.AddCors(options =>
            {
                options.AddPolicy("Dev-cors", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
                options.AddPolicy("Prod-cors", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                } );
            });
            
            services.AddHttpContextAccessor();
            
            //Dependency Injections Here
            services.AddScoped<ICreateReadRepository<Customer>,CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();

            services.AddScoped<ICreateReadRepository<Subscription>, SubscriptionRepository>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            
            services.AddScoped<ICreateReadRepository<Post>,PostRepository>();
            services.AddScoped<IPostService, PostService>();
            
            services.AddScoped<IPageRepository,PageRepository>();
            services.AddScoped<IPageService, PageService>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<ICreateReadRepository<User>, UserRepository>();
            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NB.WebAPI v1"));
                app.UseCors("Dev-cors");
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NB.WebAPI v1"));
                app.UseCors("Prod-cors");
            }
            
            
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetService<NbContext>();
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();
            }
            
            

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
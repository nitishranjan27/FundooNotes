using Buisness_Layer.Interface;
using Buisness_Layer.Service;
using FundooNotes.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository_Layer.Context;
using Repository_Layer.Interface;
using Repository_Layer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotes
{
    /// <summary>
    /// Startup Class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">configuration parameter</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets properties for configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        /// <summary>
        /// ConfigureServices Method 
        /// </summary>
        /// <param name="services">services parameter</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<FundooContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:FundooDB"]));

            services.AddTransient<IUserBL, UserBL>();
            services.AddTransient<IUserRL, UserRL>();
            //Note Service
            services.AddTransient<INoteBL, NoteBL>();
            services.AddTransient<INoteRL, NoteRL>();
            //Collabs Service
            services.AddTransient<ICollabBL, CollabBL>();
            services.AddTransient<ICollabRL, CollabRL>();
            //Label Service
            services.AddTransient<ILabelBL, LabelBL>();
            services.AddTransient<ILabelRL, LabelRL>();

            //// Redis
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
            });

            services.AddMemoryCache();

            services.AddControllers();
            //policy
            services.AddCors(options =>
            {
                options.AddPolicy(
                name: "AllowOrigin",
              builder => {
                  builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
              });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Welcome to FundooNotes" });

                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "Using the Authorization header with the Bearer scheme.",

                    Name = "Authorization",

                    In = ParameterLocation.Header,

                    Type = SecuritySchemeType.Http,

                    Scheme = "bearer",

                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,

                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }

                });
            });
            //var jwtSection = Configuration.GetSection("Jwt:Key");
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }) 
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,

                    ValidateAudience = false,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecKey"])) //Configuration["JwtToken:SecretKey"]
                };

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        /// <summary>
        /// Configure Method for configure the HTTP request pipeline
        /// </summary>
        /// <param name="app">app parameter</param>
        /// <param name="env">env parameter</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowOrigin");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FundooNotes API ");
               // c.RoutePrefix = String.Empty;
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
           
            // global error handler
            app.UseMiddleware<ErrorHandlerMiddleware>();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

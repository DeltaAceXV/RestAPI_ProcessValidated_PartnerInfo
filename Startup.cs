using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi.Models;
using RestAPI_ProcessValidated_PartnerInfo.DTO;
using RestAPI_ProcessValidated_PartnerInfo.Middleware;
using RestAPI_ProcessValidated_PartnerInfo.Repository;
using RestAPI_ProcessValidated_PartnerInfo.Service;
using Swashbuckle.AspNetCore.Swagger;
using System.Text.Json.Serialization;

namespace RestAPI_ProcessValidated_PartnerInfo
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options => { 
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "RestAPI_ProcessValidated_PartnerInfo",
                    Version = "v1",
                    Description = "Testing api",
                    Contact = new OpenApiContact
                    {
                        Name = "Joseph",
                    }
                });
            });

            services.AddCors(cors => cors.AddPolicy("Policy", builder =>
            {
                builder
                .WithOrigins("http://localhost:8080", "http://localhost")
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

            /* Configuring the services' life state */
            services.AddSingleton<ILoggerService, LoggerService>();

            services.AddScoped<IProcessAmountService, ProcessAmountService>();
            services.AddScoped<IAesEncryptionService, AesEncryptionService>();
            services.AddScoped<IValidationService, ValidationService>();
            services.AddScoped<IPartnerService, PartnerService>();

            services.AddScoped<IPartnerRepository, PartnerRepository>();
            services.AddScoped<IApiRequestLogger, ApiRequestLogger>();

            /* Bind settings */
            var settings = new Settings();
            this.Configuration.Bind(settings);

            services.AddSingleton(settings);
        }

        public void Configure(WebApplication app)
        {
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors("Policy");

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<LoggingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "RestAPI Process Validated Partner Info - v1");
                options.RoutePrefix = "swagger";
            });

            app.MapControllers();
            app.Run();
        }
    }
}

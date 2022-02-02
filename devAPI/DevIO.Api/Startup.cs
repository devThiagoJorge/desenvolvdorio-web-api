using DevIO.Api.Configuration;
using DevIO.Api.Extensions;
using DevIO.Data.Context;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DevIO.Api
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
            services.AddDbContext<MeuDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddLoggingConfiguration();

            services.AddControllers();

            services.AddHealthChecks()
                .AddSqlServer(Configuration.GetConnectionString("DefaultConnection"), name: "Banco SQL");

            services.AddHealthChecksUI().AddSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"));

            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", 
                    p => p.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:4200"));
            });

            services.AddIdentityConfiguration(Configuration);

            services.AddApiVersioning(opt => {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;
            });

            services.AddJWTConfiguration(Configuration);

            services.ResolveDepencencies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

           
            app.UseLoggingConfiguration();

            app.UseHealthChecks("/api/hc", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions() { 
                Predicate = _ => true, 
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(opt =>
            {
                opt.UIPath = "/api/hc-ui";
                opt.ResourcesPath = $"{opt.UIPath}/resources";
                opt.UseRelativeApiPath = false;
                opt.UseRelativeResourcesPath = false;
                opt.UseRelativeWebhookPath = false;
            });
        }
    }
}

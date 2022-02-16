using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reports.Server.DAL.Database;
using Reports.Server.DAL.Repositories;
using Reports.Server.Domain.Services;

namespace Reports.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEntityFrameworkSqlite();
            
            services.AddOpenApiDocument(settings =>
            {
                settings.Title = "Reports";
            });
            
            services.AddDbContext<ReportsDatabaseContext>(options =>
            {
                options.UseNpgsql("Host=localhost;Port=5432;Database=reports4;Username=postgres;Password=password");
            });
            
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();

            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IReportService, ReportService>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
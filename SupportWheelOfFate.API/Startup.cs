using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SupportWheelOfFate.API.Jobs;
using SupportWheelOfFate.API.Persistence;
using SupportWheelOfFate.API.Repositories;

namespace SupportWheelOfFate.API
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
            services.AddScoped<IScheduleRepository, EFScheduleRepository>();
            services.AddScoped<IEmployeeRepository, DummyEmployeeRepository>();

            services.AddMvc();
            services.AddCors(options =>
                options.AddPolicy("AllowAll", builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod()));

            // ignore SQLite for now
            //services.AddEntityFrameworkSqlite()
            //    .AddDbContext<SupportWheelOfFateContext>(options =>
            //        options.UseSqlite(Configuration.GetConnectionString("SQLiteDB")));

            var connectionStringBuilder = new SqlConnectionStringBuilder(
                Configuration.GetConnectionString("AzureSqlDB"))
            {
                Password = Configuration["DbPassword"]
            };

            var connectionString = connectionStringBuilder.ConnectionString;

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<SupportWheelOfFateContext>(options =>
                    options.UseSqlServer(connectionString));

            services.AddHangfire(config => config.UseMemoryStorage());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // allow anything, for now
            app.UseCors("AllowAll");

            app.UseMvc();

            app.UseHangfireServer();

            ConfigureBackgroudJobs(serviceProvider);
        }

        private void ConfigureBackgroudJobs(IServiceProvider serviceProvider)
        {
            var scheduleRepository = serviceProvider.GetService<IScheduleRepository>();
            var employeeRepository = serviceProvider.GetService<IEmployeeRepository>();

            BackgroundJob.Schedule(
                () => new GenerateSchedulesForCurrentMonthIfNeededJob(scheduleRepository, employeeRepository).Execute(),
                TimeSpan.FromMilliseconds(3 * 1000));

            BackgroundJob.Schedule(
                () => new GenerateSchedulesForNextMonthIfNeededJob(scheduleRepository, employeeRepository).Execute(),
                TimeSpan.FromMilliseconds(20 * 1000));

            RecurringJob.AddOrUpdate("GenerateSchedulesForNextMonthIfNeeded",
                () => new GenerateSchedulesForNextMonthIfNeededJob(scheduleRepository, employeeRepository).Execute(),
                Cron.Daily);
        }
    }
}

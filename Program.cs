using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using ISession = NHibernate.ISession;
using EmployeeManagement.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register CORS services
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", builder =>
            {
                // Allow your frontend app's origin (e.g., React running on http://localhost:3000)
                builder.WithOrigins("http://localhost:3000")  // Change this to match your frontend URL
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        // Register NHibernate and other services
        builder.Services.AddSingleton<ISessionFactory>(serviceProvider =>
        {
            var connectionString = builder.Configuration.GetConnectionString("EmployeeManagementDb");
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                    .ConnectionString(connectionString)
                    .ShowSql()
                    .AdoNetBatchSize(10))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Program>())
                .BuildSessionFactory();
        });

        builder.Services.AddScoped<ISession>(serviceProvider =>
        {
            var sessionFactory = serviceProvider.GetRequiredService<ISessionFactory>();
            return sessionFactory.OpenSession();
        });

        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<IUserRoleService, UserRoleService>();
        builder.Services.AddScoped<ITimeOffRequestService, TimeOffRequestService>();
        builder.Services.AddScoped<IAvailabilityService, AvailabilityService>();

        // Add session support
        builder.Services.AddDistributedMemoryCache(); // Adds memory cache for session state
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;  // Marks session cookie as essential
        });

        // Add controllers
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Enable CORS middleware
        app.UseCors("AllowSpecificOrigin");

        // Enable session middleware
        app.UseSession();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}

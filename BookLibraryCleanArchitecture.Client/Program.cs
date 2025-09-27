using BookLibraryCleanArchitecture.Application.Dtos;
using BookLibraryCleanArchitecture.Client.Extensions;
using BookLibraryCleanArchitecture.Client.Middlewares;
using BookLibraryCleanArchitecture.Domain.Entities;
using BookLibraryCleanArchitecture.Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

// PSEUDOCODE / PLAN (simple, step-by-step):
// 1. Read application configuration (builder.Configuration).
// 2. Create a Serilog LoggerConfiguration and load settings from configuration.
// 3. Enrich logs with contextual information from the execution context.
// 4. Create the logger and assign it to the global Log.Logger.
// 5. Tell the host to use Serilog for logging.
// This mirrors the actual code below which initializes Serilog and assigns it.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Register JwtOptions configuration
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.JwtOptionsSection));

// Add DbContext with SQL Server provider
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Set up Serilog logging from app settings so logs are written the right way and include context info
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Add Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.Password.RequiredLength = 8;
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequireUppercase = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireDigit = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Register your all services
builder.Services.AddAllServices();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseBookLibraryMiddlewares();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using PickPoint.back.Constants;
using PickPoint.back.EFCore;
using PickPoint.back.Extensions;
using PickPoint.back.Models.AutoMapperProfiles;
using PickPoint.back.Repositories.OnlineStoresManagementRepository;
using PickPoint.back.Repositories.OrdersRepository;
using PickPoint.back.Repositories.ParcelAutomatsRepository;
using PickPoint.back.Services.JwtGenerator;
using PickPoint.back.Services.StoresAuth;
using System;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{

  var builder = WebApplication.CreateBuilder(args);
  var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
  builder.Configuration.AddJsonsFromDirectory("Configurations");

  builder.Services.ConfigureControllers().ConfigureFluentValidation();

  // NLog: Setup NLog for Dependency injection
  builder.Logging.ClearProviders();
  builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
  builder.Host.UseNLog();

  builder.Services.ConfigureAuthentication(builder.Configuration["JwtSecretKey"]);
  builder.Services.ConfigureIdentity();
  builder.Services.AddAutoMapper(typeof(OrderProfile));
  builder.Services.Configure<ApiBehaviorOptions>(x => { x.SuppressModelStateInvalidFilter = true; });
  builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(connectionString));
  builder.Services.ConfigureCors();
  builder.Services.AddHttpContextAccessor();
  builder.Services.ConfigureApiVersioning();

  builder.Services.AddScoped<IOrdersRepo, OrdersRepo>();
  builder.Services.AddScoped<IParcelAutomatsRepo, ParcelAutomatsRepo>();
  builder.Services.AddSingleton<IJwtGenerator, JwtGenerator>();
  builder.Services.AddScoped<IOnlineStoresManagementRepo, OnlineStoresManagementRepo>();
  builder.Services.AddScoped<IStoresAuth, StoresAuth>();

  // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
  builder.Services.AddEndpointsApiExplorer();
  builder.Services.ConfigureSwaggerGen();

  var app = builder.Build();

  // Configure the HTTP request pipeline.
  if (app.Environment.IsDevelopment())
  {
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.AddSeedDataAsync(logger);
  }

  app.UseHttpsRedirection();
  app.UseCors(CorsConstants.CORS_ANY_POLICY);
  app.UseAuthentication();
  app.UseAuthorization();
  app.MapControllers();

  app.Run();
}
catch (Exception ex)
{
  logger.Error(ex, "Stopped program because of exception");
  throw;
}
finally
{
  NLog.LogManager.Shutdown();
}
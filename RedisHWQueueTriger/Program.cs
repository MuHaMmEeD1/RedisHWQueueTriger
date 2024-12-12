using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedisHWQueueTriger.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Configuration.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);

builder.Services.AddSingleton<IRedisService, RedisService>();

builder.Build().Run();

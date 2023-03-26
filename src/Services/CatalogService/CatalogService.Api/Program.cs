using CatalogService.Api.Extensions;
using CatalogService.Api.Infrastructure;
using CatalogService.Api.Infrastructure.Context;
using Microsoft.Extensions.FileProviders;
using Serilog;

var config = ConfigurationExtension.appConfig;
var serilogConf = ConfigurationExtension.serilogConfig;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    Args = args,
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
    WebRootPath = "Pics"
});

builder.Configuration.AddConfiguration(config);
builder.Host.UseDefaultServiceProvider((context, options) =>
{
    options.ValidateOnBuild = false;
    options.ValidateScopes = false;
});
builder.Host.ConfigureLogging(s => s.ClearProviders()) // Remove all added providers before
            // https://github.com/serilog/serilog-aspnetcore
            .UseSerilog(
            //(context, serv, cfg) =>
            //{
            //cfg.ReadFrom.Configuration(context.Configuration)
            //   .ReadFrom.Services(serv)
            //   .Enrich.FromLogContext()
            //   .WriteTo.Console();
            //}, writeToProviders: true
            );

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

#region SERVICES
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Configure
builder.Services.Configure<CatalogSettings>(configuration.GetSection("CatalogSettings"));
#endregion

builder.Services.ConfigureConsul(configuration);
#region DbContext
builder.Services.ConfigureDbContext(configuration);
#endregion

#endregion

var app = builder.Build();

#region PIPELINE
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CatalogService.Api v1"));
}

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(System.IO.Path.Combine(app.Environment.ContentRootPath, "Pics")),
    RequestPath = "/pics"
});

app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(serilogConf)
                .CreateLogger();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("System Up and Running - Catalog Service");
#endregion

//Bu kýsým her istekte çalýþýyor olmamalý, ona göre ayarlanmalý.
app.MigrateDbContext<CatalogContext>((context, services) =>
{
    var env = services.GetService<IWebHostEnvironment>();
    var logger = services.GetService<ILogger<CatalogContextSeed>>();

    new CatalogContextSeed().SeedAsync(context, env, logger).Wait();
});

app.Start();

app.RegisterWithConsul(app.Lifetime, configuration);

app.WaitForShutdown();
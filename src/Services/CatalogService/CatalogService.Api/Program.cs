using CatalogService.Api.Extensions;
using CatalogService.Api.Infrastructure;
using CatalogService.Api.Infrastructure.Context;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    Args = args,
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
    EnvironmentName = Environments.Staging,
    WebRootPath = "Pics"
});

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
app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(System.IO.Path.Combine(environment.ContentRootPath, "Pics")),
    RequestPath = "/pics"
});
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
#endregion

//Bu k�s�m her istekte �al���yor olmamal�, ona g�re ayarlanmal�.
app.MigrateDbContext<CatalogContext>((context, services) =>
{
    var env = services.GetService<IWebHostEnvironment>();
    var logger = services.GetService<ILogger<CatalogContextSeed>>();

    new CatalogContextSeed().SeedAsync(context, env, logger).Wait();
});

app.Start();

app.RegisterWithConsul(app.Lifetime);

app.WaitForShutdown();
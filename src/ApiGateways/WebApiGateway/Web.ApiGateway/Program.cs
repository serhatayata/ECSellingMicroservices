using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Web.ApiGateway.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

#region SERVICES
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOcelot().AddConsul();

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config
    .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath) //Directory root path
    .AddJsonFile("Configurations/ocelot.json")
    .AddEnvironmentVariables();
});

#region HttpClient
builder.Services.AddSingleton<IHttpContextAccessor>();
builder.Services.AddHttpClient("basket", c =>
{
    c.BaseAddress = new Uri(configuration["urls:basket"]);
}).AddHttpMessageHandler<HttpClientDelegatingHandler>();

builder.Services.AddHttpClient("catalog", c =>
{
    c.BaseAddress = new Uri(configuration["urls:catalog"]);
}).AddHttpMessageHandler<HttpClientDelegatingHandler>();
#endregion

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins(
                configuration["Services:BlazorWebApp"],
                configuration["Services:PaymentService"],
                configuration["Services:OrderService"],
                configuration["Services:BasketService"],
                configuration["Services:CatalogService"],
                configuration["Services:IdentityService"]
            ).AllowAnyMethod()
             .AllowAnyHeader()
             .AllowCredentials();
    });
});
#endregion

var app = builder.Build();

#region PIPELINE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseCors("CorsPolicy");
await app.UseOcelot();

app.MapControllers();
#endregion

app.Run();

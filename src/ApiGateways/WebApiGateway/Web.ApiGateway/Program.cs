using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Web.ApiGateway.Infrastructure;
using Web.ApiGateway.Services;
using Web.ApiGateway.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

#region SERVICES
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins(
                "http://localhost:2000",
                configuration["Services:PaymentService"],
                configuration["Services:OrderService"],
                configuration["Services:BasketService"],
                configuration["Services:CatalogService"],
                configuration["Services:IdentityService"]
            )
             .AllowAnyMethod()
             .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOcelot().AddConsul();
builder.Services.AddHttpContextAccessor();

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config
    .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath) //Directory root path
    .AddJsonFile("Configurations/ocelot.json")
    .AddEnvironmentVariables();
});

builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<IBasketService, BasketService>();

builder.Services.AddTransient<HttpClientDelegatingHandler>();

#region HttpClient
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpClient("basket", c =>
{
    c.BaseAddress = new Uri(configuration["urls:basket"]);
}).AddHttpMessageHandler<HttpClientDelegatingHandler>();

builder.Services.AddHttpClient("catalog", c =>
{
    c.BaseAddress = new Uri(configuration["urls:catalog"]);
}).AddHttpMessageHandler<HttpClientDelegatingHandler>();
#endregion
#endregion

var app = builder.Build();

#region PIPELINE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseEndpoints(endpoints => {
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}");
});

await app.UseOcelot();

app.MapControllers();
#endregion

app.Run();

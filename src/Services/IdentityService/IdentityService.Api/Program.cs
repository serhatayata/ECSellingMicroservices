using IdentityService.Api.Application.Services;
using IdentityService.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

#region SERVICES
builder.Services.AddControllers();

builder.Services.AddScoped<IIdentityService, IdentityService.Api.Application.Services.IdentityService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureConsul(configuration);
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

app.MapControllers();

#endregion
app.Start();

app.RegisterWithConsul(app.Lifetime);

app.WaitForShutdown();
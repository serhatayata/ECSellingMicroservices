using IdentityService.Api.Application.Services;

var builder = WebApplication.CreateBuilder(args);

#region SERVICES
builder.Services.AddControllers();

builder.Services.AddScoped<IIdentityService, IdentityService.Api.Application.Services.IdentityService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

app.Run();

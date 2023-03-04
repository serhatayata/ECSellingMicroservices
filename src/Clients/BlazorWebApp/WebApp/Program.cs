using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebApp;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using WebApp.Utils;
using WebApp.Application.Services.Interfaces;
using WebApp.Application.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddTransient<IIdentityService, IdentityService>();

// This is only creates http clients for api gateway - not correct but for this app
builder.Services.AddScoped(sp =>
{
    var clientFactory = sp.GetRequiredService<IHttpClientFactory>();

    return clientFactory.CreateClient("ApiGatewayHttpClient");
});

builder.Services.AddHttpClient("ApiGatewayHttpClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000/");
});

await builder.Build().RunAsync();

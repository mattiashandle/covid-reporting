using Karolinska.BlazorClient;
using Karolinska.Reporting.SDK;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<IJSInProcessRuntime>(services =>
    (IJSInProcessRuntime)services.GetRequiredService<IJSRuntime>());

builder.Services.AddLoadingBar();


builder.Services.AddScoped<IHealthcareProviderClient>((serviceProvider) =>
{
    return new HealthcareProviderClient("http://localhost:5271", new HttpClient().EnableIntercept(serviceProvider));
});

builder.Services.AddScoped<ISupplierClient>((serviceProvider) =>
{
    return new SupplierClient("http://localhost:5271", new HttpClient().EnableIntercept(serviceProvider));
});

builder.UseLoadingBar();

await builder.Build().RunAsync();

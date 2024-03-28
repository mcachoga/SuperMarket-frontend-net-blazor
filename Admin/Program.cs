using SuperMarket.Client;
using SuperMarket.Client.Extensions;
using SuperMarket.Client.Services.Endpoints;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiEndpointsConfig = builder.Configuration.GetSection("ApiEndpoints").Get<ApiEndpoints>();
builder.Services.AddSingleton(apiEndpointsConfig);

builder.AddClientServices();

await builder.Build().RunAsync();
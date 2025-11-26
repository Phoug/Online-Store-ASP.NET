using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Online_Store_ASP_NET.Client;
using Online_Store_ASP_NET.Client.Services.Implementations;
using Online_Store_ASP_NET.Client.Services.Interfaces;
using System;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBase = builder.Configuration["ApiBaseUrl"] ?? builder.HostEnvironment.BaseAddress;
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBase) });

builder.Services.AddScoped<IProductApiService, ProductApiServiceImpl>();

await builder.Build().RunAsync();
using AniNexus.Web.Client;
using AniNexus.Web.Client.Services;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("AniNexus", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder.Services.AddBlazorise(options =>
{
    options.ChangeTextOnKeyPress = false;
    //options.DelayTextOnKeyPress = true;
    //options.DelayTextOnKeyPressInterval = 300;
    options.EnableNumericStep = true;
    options.ShowNumericStepButtons = true;
})
.AddBootstrapProviders()
.AddFontAwesomeIcons();

builder.Services
    .AddSingleton<JSConsoleLogger>()
    .AddSingleton<IAuthenticationService, AuthenticationService>()
    .AddSingleton<ILocalStorageService, LocalStorageService>()
    .AddSingleton<IHttpClientService, HttpClientService>();

await builder.Build().RunAsync();

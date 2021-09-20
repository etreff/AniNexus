using AniNexus.Domain;
using AniNexus.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddTriggeredPooledDbContextFactoryExtended<ApplicationDbContext>((provider, options) =>
{
    options.UseSqlServer(connectionString, b =>
    {
        b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name);
    });
    options.UseAniNexusTriggers();
    if (provider.GetRequiredService<IHostEnvironment>().IsDevelopment())
    {
        options.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
    }
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddHttpContextAccessor();

builder.Services.AddDefaultIdentity<ApplicationUserModel>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUserModel, ApplicationDbContext>();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

//builder.Services.AddGraphQLServer()
//    .AddAniNexusGraphQLTypes()
//    .ModifyOptions(options => options.DefaultBindingBehavior = HotChocolate.Types.BindingBehavior.Explicit);

//builder.Services.AddAniNexusProviders();

//builder.Services
//    .AddScoped<IAnimeCoverArtService, AnimeCoverArtService>()
//    .AddScoped<IContentPathProvider, ContentPathProvider>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapGraphQL("/api/graphql");
//});


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

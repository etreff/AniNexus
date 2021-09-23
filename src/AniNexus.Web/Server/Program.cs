using System.Text;
using AniNexus.Domain;
using AniNexus.Models.Configuration;
using AniNexus.Repository;
using AniNexus.Web.Server;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddUserSecrets<Program>(true, false)
    .AddEnvironmentVariables();

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddTriggeredPooledDbContextFactory<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString, b =>
    {
        b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name);
    });
    options.UseAniNexusTriggers();
    options.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JWT").Get<JwtSettings>();

    options.SaveToken = false;
    options.RequireHttpsMetadata = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        ClockSkew = TimeSpan.FromMinutes(jwtSettings.ClockSkew),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Key)),
    };
});

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();

    options.AddPolicy(Policy.User.UpdateInfo, policy => policy.RequireClaim(Policy.User.UpdateInfo));
});

builder.Services.Configure<TheTVDBSettings>(builder.Configuration.GetSection("TVDB"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));

//builder.Services.AddGraphQLServer()
//    .AddAniNexusGraphQLTypes()
//    .ModifyOptions(options => options.DefaultBindingBehavior = HotChocolate.Types.BindingBehavior.Explicit);

builder.Services.AddAniNexusProviders();

//builder.Services
//    .AddScoped<IAnimeCoverArtService, AnimeCoverArtService>()
//    .AddScoped<IContentPathProvider, ContentPathProvider>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//builder.Services.AddCors();

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

//app.UseCors(x => x
//    .AllowAnyOrigin()
//    .AllowAnyMethod()
//    .AllowAnyHeader());

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

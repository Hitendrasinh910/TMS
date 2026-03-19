using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
//using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using TMS.Helpers;
using TMS.Repositories;
using TMS.Services;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------
// 1. SERILOG CONFIGURATION
// ---------------------------------
builder.Logging.ClearProviders();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/tms-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// ---------------------------------
// 2. MVC + JSON OPTIONS
// ---------------------------------
builder.Services.AddControllersWithViews()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// ---------------------------------
// 3. SESSION CONFIGURATION
// ---------------------------------
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ---------------------------------
// 4. DEPENDENCY INJECTION (DI)
// ---------------------------------
builder.Services.AddSingleton<JwtHelper>();
builder.Services.AddScoped<IDapperHelper, DapperHelper>();

// Repositories
builder.Services.AddScoped<MasterUserRepo>();
builder.Services.AddScoped<MasterCountryRepo>();
builder.Services.AddScoped<MasterStateRepo>();
builder.Services.AddScoped<MasterCityRepo>();
builder.Services.AddScoped<MasterPartyAccountRepo>();
builder.Services.AddScoped<MasterTruckRepo>();
builder.Services.AddScoped<MasterDriverRepo>();
// Add other repos here...

// Services
builder.Services.AddScoped<MasterUserService>();
builder.Services.AddScoped<MasterCountryService>();
builder.Services.AddScoped<MasterStateService>();
builder.Services.AddScoped<MasterCityService>();
builder.Services.AddScoped<MasterPartyAccountService>();
builder.Services.AddScoped<MasterTruckService>();
builder.Services.AddScoped<MasterDriverService>();
// Add other services here...

// ---------------------------------
// 5. JWT AUTHENTICATION
// ---------------------------------
var jwtKey = Encoding.UTF8.GetBytes(
    builder.Configuration["Jwt:Key"] ?? throw new Exception("Jwt:Key is missing in appsettings.json")
);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// ---------------------------------
// 6. SWAGGER CONFIGURATION
// ---------------------------------
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token"
    });

    //options.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    //        },
    //        Array.Empty<string>()
    //    }
    //});
});

var app = builder.Build();

// ---------------------------------
// 7. MIDDLEWARE PIPELINE
// ---------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// 🔑 JWT Cookie-to-Header Middleware
app.Use(async (context, next) =>
{
    if (!context.Request.Headers.ContainsKey("Authorization") &&
        context.Request.Cookies.TryGetValue("jwt_token", out var token))
    {
        context.Request.Headers["Authorization"] = "Bearer " + token;
    }
    await next();
});

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Swagger for API testing
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ---------------------------------
// 8. ROUTES & MAPPING
// ---------------------------------

// Root redirect to Login
app.MapGet("/", ctx =>
{
    //ctx.Response.Redirect("/Master/User");
    ctx.Response.Redirect("/Account/Login");
    return Task.CompletedTask;
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapControllers(); // Required for [ApiController]

app.Run();
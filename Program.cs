using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using ApiCatalogo.Context;
using ApiCatalogo.DTOs.Mappings;
using ApiCatalogo.Extensions;
using ApiCatalogo.Filters;
using ApiCatalogo.Logging;
using ApiCatalogo.Models;
using ApiCatalogo.RateLimitOptions;
using ApiCatalogo.Repositories;
using ApiCatalogo.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{ options.Filters.Add(typeof(ApiExceptionFilter)); }
 ).AddJsonOptions(options =>
    options.JsonSerializerOptions
      .ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "apicatalogo", Version = "v1" });

  c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
  {
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "Bearer JWT",
  });
  c.AddSecurityRequirement(new OpenApiSecurityRequirement
  {
      {
        new OpenApiSecurityScheme
         {
          Reference = new OpenApiReference
           {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
          }
        },
        new string[] {}
      }
  });
});


builder.Services.AddIdentity<ApplicationUser, IdentityRole>().
AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
  options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

var secretKey = builder.Configuration["JWT:Secretkey"] ?? throw new ArgumentException("Invalid secret key");

builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
  options.SaveToken = true;
  options.RequireHttpsMetadata = false;
  options.TokenValidationParameters = new TokenValidationParameters()
  {
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ClockSkew = TimeSpan.Zero,
    ValidAudience = builder.Configuration["JWT:ValidAudience"],
    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
  };
});

builder.Services.AddAuthorization(options =>
{
  options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
  options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("Admin").RequireClaim("id", "wesley"));
  options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
  options.AddPolicy("ExclusiveOnly", policy => policy.RequireAssertion(context =>
  context.User.HasClaim(claim => claim.Type == "id" && claim.Value == "wesley" || context.User.IsInRole("SuperAdmin"))));
});

var myOptions = new MyRateLimitOptions();

builder.Configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(myOptions);

// limita as requests local

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
  rateLimiterOptions.AddFixedWindowLimiter(policyName: "fixedwindow", options =>
  {
    options.PermitLimit = myOptions.PermitLimit;
    options.Window = TimeSpan.FromSeconds(myOptions.Window);
    options.QueueLimit = myOptions.QueueLimit;
    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
  });
  rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// RateLimiter contexto global

// builder.Services.AddRateLimiter(options =>
// {
//   options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

//   options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext => RateLimitPartition.GetFixedWindowLimiter(
//              partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
//              factory: partition => new FixedWindowRateLimiterOptions
//              {
//                AutoReplenishment = true,
//                PermitLimit = 5,
//                QueueLimit = 0,
//                Window = TimeSpan.FromSeconds(10)
//              }));

// });

builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();


builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
  LogLevel = LogLevel.Information
}));


builder.Services.AddAutoMapper(typeof(ProductDTOMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
  app.ConfigureExceptionHandler();
}

app.UseRouting();
app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

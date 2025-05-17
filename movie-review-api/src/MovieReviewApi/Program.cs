// Purpose: Contains the main method for the MovieReviewApi project. This file is responsible for setting up the application and configuring the services and middleware.
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MovieReviewApi.Data;
using MovieReviewApi.DTOs;
using MovieReviewApi.Interfaces;
using MovieReviewApi.Mappings;
using MovieReviewApi.Models;
using MovieReviewApi.Repositories;
using MovieReviewApi.Services;
using Serilog;
using StackExchange.Redis;
using Nest;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

var settings = new ConnectionSettings(new Uri("http://elasticsearch:9200"))
    .DefaultIndex("movies");

var elasticClient = new ElasticClient(settings);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.AddSingleton<IElasticClient>(elasticClient);
builder.Services.AddSingleton(Log.Logger);



builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblyContaining<MovieCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ReviewCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5, // Maximum number of retries
                maxRetryDelay: TimeSpan.FromSeconds(5), // Delay between retries
                errorNumbersToAdd: null); // Retry on all transient errors
        });
});


builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING") ??
        builder.Configuration["Redis:ConnectionString"];
    if (string.IsNullOrEmpty(configuration))
    {
        throw new ArgumentNullException(nameof(configuration), "Redis connection string cannot be null or empty.");
    }
    return ConnectionMultiplexer.Connect(configuration);
});

// these dependency injections solve the error: The type 'IXRepository' cannot be resolved. 
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IWatchlistRepository, WatchlistRepository>();

builder.Services.AddScoped<IElasticsearchService, ElasticsearchService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IWatchlistService, WatchlistService>();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// builder.Services.AddMemoryCache();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
    options.InstanceName = builder.Configuration["Redis:InstanceName"];
});

builder.Services.AddAuthentication(options =>
{
    // fixing the error not found
    // to fix the error: No authenticationScheme was specified, and there was no DefaultChallengeScheme found.
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(
            builder.Configuration.GetSection("Jwt:Key").Value ?? "");

        options.RequireHttpsMetadata = false; // must be true in production
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Movie Review API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// using (var scope = builder.Services.BuildServiceProvider().CreateScope())
// {
//     var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();

//     try
//     {
//         // Redis'e bir test anahtarı yaz
//         await cache.SetStringAsync("testKey", "testValue");

//         // Redis'ten test anahtarını oku
//         var value = await cache.GetStringAsync("testKey");
//         Console.WriteLine($"Redis test key value: {value}");
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine($"Redis bağlantı hatası: {ex.Message}");
//     }
// }

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
Log.CloseAndFlush();
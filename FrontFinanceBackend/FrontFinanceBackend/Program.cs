using System.Net;
using System.Net.Security;
using FrontFinanceBackend.Config;
using FrontFinanceBackend.Models;
using FrontFinanceBackend.Repository;
using FrontFinanceBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Brokers.Alpaca.Configuration;
using Brokers.Alpaca.Services;
using Brokers.Coinbase;
using Brokers.Coinbase.Models;
using Core.Contracts;
using Core.Logic.Services;
using Brokers.Coinbase.Services;
using Brokers.Freedom.Configuration;
using Brokers.Freedom.Services;
using Brokers.InteractiveBrokers.Configuration;
using Brokers.InteractiveBrokers.Services;
using Brokers.Polygon.Configuration;
using Brokers.Polygon.Services;
using Brokers.Tradier.Configuration;
using Brokers.Tradier.Services;
using Brokers.Trading212.Configuration;
using Brokers.Trading212.Services;
using Core.Contracts.Adapters.Alpaca;
using Core.Contracts.Adapters.Freedom;
using Core.Contracts.Adapters.InteractiveBrokers;
using Core.Contracts.Adapters.Polygon;
using Core.Contracts.Adapters.Tradier;
using Core.Contracts.Adapters.Trading212;
using Org.Front.Core.Contracts.Adapters.Alpaca;
using Org.Front.Core.Contracts.Extensions;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
{
    return (sslPolicyErrors & SslPolicyErrors.RemoteCertificateNotAvailable) != SslPolicyErrors.RemoteCertificateNotAvailable;
};

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions();

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IStockDataRepo, StockDataRepo>();
builder.Services.AddScoped<IStockBarRepo, StockBarRepo>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMarketService, MarketService>();

//Coinbase
var configSection = configuration.GetSection(nameof(CoinbaseConfig));
builder.Services.Configure<CoinbaseConfig>(configSection);
builder.Services.AddHttpClientWithPolicies<ICoinbaseAuthService, CoinbaseAuthService, CoinbaseConfig>(configSection);

//IB
configSection = configuration.GetSection(nameof(InteractiveBrokersConfig));
builder.Services.Configure<InteractiveBrokersConfig>(configSection);
builder.Services.AddHttpClientWithPolicies<IInteractiveBrokersTransactionService, InteractiveBrokersTransactionService, InteractiveBrokersConfig>(configSection);
builder.Services.AddHttpClientWithPolicies<IInteractiveBrokersMarketDataService, InteractiveBrokersMarketDataService, InteractiveBrokersConfig>(configSection);
builder.Services.AddScoped<IInteractiveBrokersAuthService, InteractiveBrokersAuthService>();


//Freedom
builder.Services.AddHttpClientWithPolicies<IFreedomAuthService, FreedomAuthService, FreedomConfig>(configuration.GetSection(nameof(FreedomConfig)));
builder.Services.AddHttpClientWithPolicies<IFreedomMarketDataService, FreedomMarketDataService, FreedomConfig>(configuration.GetSection(nameof(FreedomConfig)));

//Tradier
builder.Services.AddHttpClientWithPolicies<ITradierAuthService, TradierAuthService, TradierConfig>(configuration.GetSection(nameof(TradierConfig)));
builder.Services.AddHttpClientWithPolicies<ITradierMarketDataService, TradierMarketDataService, TradierConfig>(configuration.GetSection(nameof(TradierConfig)));

//Alpaca
builder.Services.Configure<AlpacaConfig>(configuration.GetSection(nameof(AlpacaConfig)));
builder.Services.AddScoped<IAlpacaMarketDataService, AlpacaMarketDataService>();
builder.Services.AddScoped<IAlpacaTransactionService, AlpacaTransactionService>();
builder.Services.AddHttpClient<IAlpacaAuthService, AlpacaAuthService>();

//Trading212
builder.Services.Configure<Trading212Config>(configuration.GetSection(nameof(Trading212Config)));
builder.Services.AddHttpClientWithPolicies<ITrading212MarketDataService, Trading212MarketDataService, Trading212Config>(configuration.GetSection(nameof(Trading212Config)))
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
    {
        AutomaticDecompression = DecompressionMethods.GZip,
    });
//builder.Services.AddScoped<ITrading212TransactionService, Trading212TransactionService>();
builder.Services.AddHttpClientWithPolicies<ITrading212AuthService, Trading212AuthService, Trading212Config>(configuration.GetSection(nameof(Trading212Config)))
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
    {
        AutomaticDecompression = DecompressionMethods.GZip,
    });

//Polygon
builder.Services.Configure<PolygonConfig>(configuration.GetSection(nameof(PolygonConfig)));
builder.Services.AddHttpClientWithPolicies<IPolygonFinancesService, PolygonFinancesService, PolygonConfig>(configuration.GetSection(nameof(PolygonConfig)))
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
    {
        AutomaticDecompression = DecompressionMethods.GZip,
    });

//Broker Services
builder.Services.AddScoped<IBrokerAuthService, BrokerAuthService>();
builder.Services.AddScoped<IBrokerMarketDataService, BrokerMarketDataService>();



builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<FrontUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader().AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().Build();
    });
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FrontFinanceBackend", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminAccess", policy =>
    {
        policy.RequireRole("Admin");
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/api/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();

app.Run();
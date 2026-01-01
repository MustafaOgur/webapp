using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Business.Mappings;
using Business.Concrete;
using Core.Utilities.Security;
using System.Text;
using DataAccess;
using DataAccess.Concrete;
using Core.Utilities.Security.JWT;
using Core.Utilities.ExternalServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FluentValidation.AspNetCore;
using FluentValidation;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "DevOpsWizard API", Version = "v1" });

    // JWT Auth için ekleme
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});



// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Mapper
builder.Services.AddAutoMapper(
    typeof(ChatProfile).Assembly,
    typeof(MessageProfile).Assembly,
    typeof(ResponseProfile).Assembly,
    typeof(UserProfile).Assembly
);


var assemblies = new[]
{
    typeof(ChatService).Assembly,    // Business
    typeof(ChatRepository).Assembly  // DataAccess
};

builder.Services.Scan(scan => scan
    .FromAssemblies(assemblies)
    .AddClasses(classes => classes.InNamespaces("Business.Concrete"))
        .AsImplementedInterfaces()
        .WithScopedLifetime()
    .AddClasses(classes => classes.InNamespaces("DataAccess.Concrete"))
        .AsImplementedInterfaces()
        .WithScopedLifetime()
);

// TOKEN SERVİCE DI
builder.Services.AddScoped<ITokenService, TokenService>();

// CurrentUserService DI
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();


// External LLM Service
builder.Services.AddSingleton<ILlmClient>(sp =>
    new LlmClient(
        siteUrl: "https://devopswizard.com",
        siteName: "DevOpsWizard"
    )
);

// JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});

// Authorization
builder.Services.AddAuthorization();


// 1. CORS Ayarı (Frontend'e İzin Verme)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // React varsayılan portu
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// 2. Rate Limiting (Saldırı Önleme)
// Microsoft.AspNetCore.RateLimiting kütüphanesi .NET 7+ ile yerleşik gelir.
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,       // Dakikada en fazla 100 istek
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 2           // Kuyrukta bekleyen istek limiti
            });
    });

    // Reddedilen istekler için 429 (Too Many Requests) dön
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsync("Cok fazla istek attiniz. Lutfen biraz bekleyin. (Rate Limit Exceeded)", token);
    };
});


var app = builder.Build();

// Otomatik migration uygula
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();

    DataAccess.Seed.DataSeeder.SeedAdminUser(dbContext);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 1. CORS'u devreye al (Frontend kapıdan geçsin)
app.UseCors("AllowReactFrontend");

// 2. Rate Limiter'ı devreye al (Spam yapanı durdur)
app.UseRateLimiter();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// HEALTH CHECK ENDPOINT (MapControllers'dan ÖNCE)
app.MapGet("/health", () => 
{
    var hasApiKey = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("OPENROUTER_API_KEY"));
    return Results.Ok(new { 
        status = "healthy", 
        timestamp = DateTime.UtcNow,
        hasApiKey = hasApiKey
    });
});

app.MapControllers();

app.Run();

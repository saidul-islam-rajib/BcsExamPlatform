using BcsExamPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Note: Top-level statements don't support async Main, so we'll handle it differently

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register AI Service based on configuration
var aiProvider = builder.Configuration["AIProvider"] ?? "Fallback";

if (aiProvider == "Fallback")
{
    builder.Services.AddSingleton<BcsExamPlatform.Core.Services.IOpenAIService, BcsExamPlatform.Infrastructure.Services.FallbackAIService>();
}
else if (aiProvider == "Gemini")
{
    builder.Services.AddHttpClient<BcsExamPlatform.Core.Services.IOpenAIService, BcsExamPlatform.Infrastructure.Services.GeminiService>();
}
else if (aiProvider == "Groq")
{
    builder.Services.AddHttpClient<BcsExamPlatform.Core.Services.IOpenAIService, BcsExamPlatform.Infrastructure.Services.GroqService>();
}
else if (aiProvider == "HuggingFace")
{
    builder.Services.AddHttpClient<BcsExamPlatform.Core.Services.IOpenAIService, BcsExamPlatform.Infrastructure.Services.HuggingFaceService>();
}
else
{
    builder.Services.AddHttpClient<BcsExamPlatform.Core.Services.IOpenAIService, BcsExamPlatform.Infrastructure.Services.OpenAIService>();
}

builder.Services.AddHttpClientFactory();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFlutterApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Automatically create database and apply migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        logger.LogInformation("Checking database...");
        
        // Create database if it doesn't exist and apply any pending migrations
        await context.Database.MigrateAsync();
        
        logger.LogInformation("Database is up to date.");
        
        // Seed initial data
        logger.LogInformation("Seeding database...");
        await DbInitializer.Initialize(context);
        
        logger.LogInformation("Database initialization completed successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while initializing the database.");
        throw; // Re-throw to prevent app from starting with database issues
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable static files for admin panel
app.UseStaticFiles();
app.UseDefaultFiles();

// Disable HTTPS redirection in development for Flutter web
// app.UseHttpsRedirection();
app.UseCors("AllowFlutterApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

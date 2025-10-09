using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using SuzumesDeepDungeon.Data;
using SuzumesDeepDungeon.Hub;
using SuzumesDeepDungeon.Services;
using SuzumesDeepDungeon.Services.CSVLoad;
using System.Text;
using System.Text.Json.Serialization;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);



string redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection");
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));

//Console.WriteLine("THIS IS A CONNECTION STRING FOR REDIS   " + redisConnectionString);

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAngularApp", policy => {
        policy.WithOrigins("http://localhost:4200") 
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); 
    });
});
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

RawgApi.rawgApi = builder.Configuration["rawgAPI"] ?? "";


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
//builder.Services.AddSignalR();
builder.Services.AddAuthorization();
builder.Services.AddScoped<SteamApi>();
builder.Services.AddScoped<RawgApi>();
builder.Services.AddScoped<CSVLoad>();
builder.Services.AddScoped<TwitchService>();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));




var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DatabaseContext>(); // �������� �� ��� DbContext
        context.Database.Migrate(); // ��������� ��������
        Console.WriteLine("Migrations applied successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngularApp");

//app.UseHttpsRedirection();
app.UseAuthorization();


app.MapControllers();

app.MapGet("/api/health", () => "Backend is healthy");

//app.MapHub<ChatHub>("/chatHub");

app.Run();

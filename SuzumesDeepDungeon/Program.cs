using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SuzumesDeepDungeon.Data;
using SuzumesDeepDungeon.Services;
using SuzumesDeepDungeon.Services.CSVLoad;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

//var certificatePath = builder.Configuration["Kestrel:Certificates:Default:Path"];
//var certificatePassword = builder.Configuration["Kestrel:Certificates:Default:Password"];

//builder.WebHost.ConfigureKestrel(serverOptions => {
//    serverOptions.ConfigureHttpsDefaults(httpsOptions => {
//        if (!string.IsNullOrEmpty(certificatePath) && File.Exists(certificatePath))
//        {
//            httpsOptions.ServerCertificate = new X509Certificate2(
//                certificatePath,
//                certificatePassword
//            );
//        }
//        else
//        {
//            // Для разработки без сертификата
//            httpsOptions.ServerCertificate =
//                new X509Certificate2("/app/certs/aspnetcert.pfx", "123f7d_s12SAD_d_fd144f1");
//        }
//    });
//});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
Console.WriteLine(builder.Configuration["rawgAPI"]);
RawgApi.rawgApi = builder.Configuration["rawgAPI"] ?? "";
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

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

builder.Services.AddAuthorization();
builder.Services.AddScoped<SteamApi>();
builder.Services.AddScoped<RawgApi>();
builder.Services.AddScoped<CSVLoad>();


builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (args.Contains("--migrate"))
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        dbContext.Database.Migrate();
    }
    return; // Завершаем после миграций
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll"); // Применяем политику CORS


//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/api/health", () => "Backend is healthy");
app.MapGet("/api/test", () => "Test endpoint works");

app.Run();

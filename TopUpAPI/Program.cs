using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TopUpAPI.Services.Beneficiaries;
using TopUpAPI.Services.TopUp;
using TopUpDB.Entity;
using Microsoft.Extensions.Logging;
using TopUpDB.Seeding;
using Microsoft.AspNetCore.Identity;
using TopUpDB.Interface;
using TopUpDB.Implementation;
using TopUpAPI.Utilities;
using TopUpAPI.Services.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TopUpAPI.Services.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDBContext>(o =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    o.UseSqlite(connectionString);
});

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
        .AddJwtBearer(jwt => {
            var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig")["JwtSecret"]);

            jwt.SaveToken = true;
            jwt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false
            };
        });

builder.Services.AddLogging(config =>
{
    config.AddConsole(); // Adding console logging
                         // Other logging configurations can be added here
});


builder.Services.AddScoped<IBeneficiariesService, BeneficiariesService>();
builder.Services.AddScoped<ITopUpService, TopUpService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBeneficiary, BeneficiaryImplementation>();
builder.Services.AddScoped<ITopUp, TopUpImplementation>();
builder.Services.AddScoped<IUser, UserImplementation>();
builder.Services.AddScoped<IBalance, BalanceImplementation>();
builder.Services.AddScoped<IDataSeeder, UserSeeder>();




Const.JwtSecret = builder.Configuration.GetSection("JwtConfig")["JwtSecret"];
Const.TopUpOptions = builder.Configuration.GetSection("TopUpOptions").Get<List<int>>();
Const.BalanceApiUrl = builder.Configuration.GetSection("BalanceApiUrl").Value;
Const.ApiKey = builder.Configuration.GetSection("ApiKey").Value;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization(); ;

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {       
        var context = services.GetRequiredService<AppDBContext>(); 
        context.Database.Migrate();
      
        var seeder = services.GetRequiredService<IDataSeeder>();
        await seeder.SeedDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();
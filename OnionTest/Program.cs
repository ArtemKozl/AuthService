using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using OnionTest.Application.Services;
using OnionTest.DataAccess;
using OnionTest.DataAccess.Repositories;
using OnionTest.Extensions;
using OnionTest.Infastucture;
using OnionTest.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });
});
builder.Services.AddAutoMapper(typeof(UserProfiles));

builder.Services.AddDbContext<OnionTestDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("OnionTestDbContext"));
    });

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRefreshtokenService, RefreshtokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

builder.Services.AddScoped<IJWTProvider, JWTProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IRefreshToken, RefreshToken>();

builder.Services.AddApiAuthentication(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowChatDomain",
        builder => builder
           .WithOrigins("https://localhost:7125")
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Service V1");
    });
}

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always,
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowChatDomain");

app.UseMiddleware<JwtMiddleware>();

app.Run();


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Test_Backend.Authorization;
using Test_Backend.Data;
using Test_Backend.Reponsitory;
using Test_Backend.Services;
using Test_Backend.Validations;

var builder = WebApplication.CreateBuilder(args);

// ====== 🔹 Cấu hình JWT ======
IConfigurationSection jwtSettings = builder.Configuration.GetSection("Jwt");
string secretKey = builder.Configuration["AppSettings:SecretKey"];
byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddSingleton<JwtSecurityTokenHandler>();

builder.Services.AddSingleton(provider => new TokenValidationParameters
{
    ValidateIssuer = false, 
    ValidateAudience = false,
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
    ClockSkew = TimeSpan.Zero
});

// ====== 🔹 Thêm DbContext ======
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDB")));

// ====== 🔹 Đăng ký dịch vụ ======
builder.Services.AddScoped<IStudentServices, StudentServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IAuthService, AuthServices>();
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

// ====== 🔹 Cấu hình CORS cho Angular ======
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // cho phép frontend gọi
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // nếu bạn dùng cookie/token
    });
});

// ====== 🔹 Controller & Swagger ======
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ====== 🔹 Cấu hình xác thực JWT ======
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // ✅ Cho phép HTTP local
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
        ClockSkew = TimeSpan.Zero
    };
});

// ====== 🔹 Authorization ======
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

builder.Services.AddScoped<TokenValidation>(provider =>
{
    IRepositoryWrapper reponsitory = provider.GetRequiredService<IRepositoryWrapper>();
    JwtSecurityTokenHandler tokenHandler = provider.GetRequiredService<JwtSecurityTokenHandler>();
    TokenValidationParameters tokenValidationParameters = provider.GetRequiredService<TokenValidationParameters>();

    return new TokenValidation(reponsitory, tokenHandler, tokenValidationParameters);
});

var app = builder.Build();

// ====== 🔹 Swagger cho môi trường Dev ======
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ====== 🔹 Cho phép truy cập HTTP từ Angular ======
app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");

// ====== 🔹 Authentication / Authorization ======
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

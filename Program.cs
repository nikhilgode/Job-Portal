using JobPortal_New.Data;
using JobPortal_New.Interfaces.Repositories;
using JobPortal_New.Repositories;
using JobPortal_New.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });


builder.Services.AddDbContext<MyDbContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("JobPortal")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var token = context.SecurityToken as JwtSecurityToken;
                if (token != null)
                {
                    var tokenString = token.RawData;
                    var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenRepository>();
                    if (!tokenService.IsTokenValid(tokenString))
                    {
                        context.Fail("Token has been revoked.");
                    }
                }
                await Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RecruiterPolicy", policy => policy.RequireRole("Recruiter"));
    options.AddPolicy("CandidatePolicy", policy => policy.RequireRole("Candidate"));
});

//
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IUserRepository, UserService>();
builder.Services.AddScoped<ITokenRepository, TokenService>();
builder.Services.AddScoped<IJobRepository, JobService>();
builder.Services.AddScoped<IApplicationRepository, ApplicationService>();
builder.Services.AddScoped<IEmailRepository, EmailService>();
builder.Services.AddScoped<IOtpRepository, OtpService>();
builder.Services.AddScoped<IApiConsumptionRepository, ApiConsumeService>();


builder.Services.AddHttpClient<IApiConsumptionRepository,ApiConsumeService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5283/"); // Replace with your actual API base URL
});

//
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
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
            new string[] { }
        }
    });
});
//builder.Services.AddScoped<IEmailRepository, ApplicationService>();
//builder.Services.AddScoped<IConfiguration, EmailService>();
// Configure JWT settings
//var jwtSettingsSection = builder.Configuration.GetSection("Jwt");
//builder.Services.Configure<JwtSettings>(jwtSettingsSection);
//var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
//var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<JobPortal_New.Middlewares.BlacklistTokenMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

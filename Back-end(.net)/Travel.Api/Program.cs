using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Travel.Api;
using Travel.Api.Middleware;
using Travel.Core.Services;
using Travel.Infrastructure.Data;
using Travel.Infrastructure.Repositories;
using Quartz;
using Travel.Core.Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add dbContext
builder.Services.AddDbContext<TravelDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
    .LogTo(Console.WriteLine, LogLevel.Warning));

// Cấu hình JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
    options.AddPolicy("HotelPartner", policy => policy.RequireRole("HotelPartner"));
    options.AddPolicy("TourPartner", policy => policy.RequireRole("TourPartner"));
});

// Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddQuartz(q =>
{
    // Đăng ký job và trigger
    var jobKey = new JobKey("BookingTimeoutJob");
    q.AddJob<BookingTimeoutJob>(opts => opts.WithIdentity(jobKey)); 

    // Thiết lập trigger cho job
    q.AddTrigger(opts => opts
        .ForJob(jobKey) 
        .WithIdentity("BookingTimeoutJob") 
        .StartNow()
        .WithSimpleSchedule(x => x
            .WithInterval(TimeSpan.FromMinutes(1)) 
            .RepeatForever())
    );
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.RegisterServicesAndRepositories(
    typeof(UserService).Assembly,
    typeof(UserRepository).Assembly
);


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    
}); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowVueApp");
app.UseMiddleware<HotelOwnershipMiddleware>();
app.UseMiddleware<RoomOwnershipMiddleware>();
//app.UseMiddleware<ReviewMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

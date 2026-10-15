using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StageFlow.Application.Auth;
using StageFlow.Application.Schedule;
using StageFlow.Infrastructure.Identity;
using StageFlow.Infrastructure.Persistence;
using StageFlow.Infrastructure.Schedule;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
})
.AddRoles<IdentityRole<Guid>>()
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IPerformanceRepository, PerformanceRepository>();
builder.Services.AddScoped<CreatePerformanceService>();
builder.Services.AddScoped<GetPerformancesService>();
builder.Services.AddScoped<IUserAccountCreator, IdentityUserAccountCreator>();
builder.Services.AddScoped<RegisterUserService>();
builder.Services.AddScoped<IUserAuthenticator, IdentityUserAuthenticator>();
builder.Services.AddScoped<LoginUserService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("AllowReact");

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run();
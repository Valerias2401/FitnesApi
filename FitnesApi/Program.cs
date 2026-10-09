using FitnesApi.DatabaseContext;
using FitnesApi.Services;
using Microsoft.EntityFrameworkCore;
using FitnesApi.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorCors", policy =>
    {
        policy.WithOrigins("https://localhost:7243")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddDbContext<ContextDb>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AbonementService>();
builder.Services.AddScoped<UserAbonementService>();
builder.Services.AddScoped<CourtService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<SectionService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<AnalyticsService>();
builder.Services.AddHostedService<BookingCleanupService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("BlazorCors");
app.UseMiddleware<FitnesApi.Middleware.JwtMiddleware>(); // ← ВОТ ЭТО

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("/notificationHub");

app.Run();
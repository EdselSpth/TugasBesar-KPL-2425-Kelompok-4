using JadwalAPI.Configuration; // Tambahkan ini
using JadwalAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Tambahkan konfigurasi JadwalSettings
builder.Services.Configure<JadwalSettings>(
    builder.Configuration.GetSection("JadwalSettings"));

// Tambahkan service JadwalService
builder.Services.AddSingleton<IJadwalService, JadwalService>();

// Add services to the container.
builder.Services.AddControllers();
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
app.UseAuthorization();
app.MapControllers();
app.Run();

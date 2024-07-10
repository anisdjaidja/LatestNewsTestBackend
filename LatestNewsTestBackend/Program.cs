using LatestNewsTestBackend.DataAcess;
using LatestNewsTestBackend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
/// Configuration for the background fetching thread to enable smooth silent data fetching
builder.Services.Configure<HostOptions>(builder =>
{
    builder.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
    builder.ServicesStartConcurrently = true;
    builder.ServicesStopConcurrently = false;
});

// Add services to the container.
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddDbContextFactory<NewsContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLServer"));
});
builder.Services.AddHostedService<NewsFetchService>();
builder.Services.AddSingleton<NewsService>();

builder.Services.AddControllers();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
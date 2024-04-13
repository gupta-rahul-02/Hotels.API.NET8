using Hotels.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Adding connection
var connectionString = builder.Configuration.GetConnectionString("HotelsDbConnectionString");
builder.Services.AddDbContext<HotelsDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Adding CORS 
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", b => b.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});

//Adding logger
builder.Host.UseSerilog((context, lc) => lc.WriteTo.Console().ReadFrom.Configuration(context.Configuration));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//serilog middleware
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

//cors middleware
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();

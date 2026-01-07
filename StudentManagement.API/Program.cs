using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StudentManagement.BLL.Implementation;
using StudentManagement.BLL.Signature;
using StudentManagement.LOGGER;
using STUDENTMANAGEMENT.DAL.DbContexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Practice API",
        Version = "v1"
    });
});


builder.Services.AddDbContext<StudentManagementDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DB")));

builder.Services.AddScoped<IBLLCommon, BLLCommon>();
builder.Services.AddScoped<IApplicationLogger, ApplicationLogger>();


var app = builder.Build();

app.UseCors("corsApp");
_ = app.UseCors(builder =>
{
    _ = builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

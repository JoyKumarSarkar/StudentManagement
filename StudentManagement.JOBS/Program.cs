using Microsoft.EntityFrameworkCore;
using StudentManagement.BLL.Implementation;
using StudentManagement.BLL.Signature;
using StudentManagement.JOBS;
using StudentManagement.JOBS.WorkerServiceJobs;
using StudentManagement.LOGGER;
using STUDENTMANAGEMENT.DAL.DbContexts;

var builder = Host.CreateApplicationBuilder(args);
// ----------------------
// Configuration
// ----------------------
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables();

// ----------------------
// DbContext
// ----------------------
builder.Services.AddDbContext<StudentManagementDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DB")));

// ----------------------
// Application Services
// ----------------------
builder.Services.AddScoped<IApplicationLogger, ApplicationLogger>();
builder.Services.AddScoped<IBLLCommon, BLLCommon>();

// ----------------------
// Background Worker
// ----------------------
//builder.Services.AddHostedService<ApplicationLogCleanerJob>();
builder.Services.AddHostedService<InsertTStudentStageDataIntoTStudent>();

var host = builder.Build();
host.Run();

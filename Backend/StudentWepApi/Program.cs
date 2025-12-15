using StudentWepApi.Data;
using StudentWepApi.Data.StudentContext;
using Microsoft.EntityFrameworkCore;
using StudentWepApi.Utility;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StudentDbContext>(options =>
    options.UseSqlServer("Data Source=ACER\\SQLEXPRESS;Initial Catalog=JoyBB;Integrated Security=True;Trust Server Certificate=True"));
builder.Services.AddScoped<DbContext, StudentDbContext>();
builder.Services.AddScoped<IStudentDataAccess, StudentDataAccess>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddCors();
//builder.Services.AddCors(c => c.AddPolicy("corsApp", build => build.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader()));
var app = builder.Build();

app.UseCors("corsApp");
_ = app.UseCors(builder =>
{
    _ = builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

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




//using Microsoft.EntityFrameworkCore;
//using StudentWepApi.Data;
//using StudentWepApi.Data.StudentContext;

//var builder = WebApplication.CreateBuilder(args);

//// Read values from appsettings.json
//var dbHost = builder.Configuration["studentdbHost"];
//var dbName = builder.Configuration["studentdbName"];

//// Build SQL connection string
//var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};Integrated Security=True;Trust Server Certificate=True";

//builder.Services.AddDbContext<StudentDbContext>(options =>
//    options.UseSqlServer(connectionString));

//builder.Services.AddScoped<DbContext, StudentDbContext>();
//builder.Services.AddScoped<IStudentDataAccess, StudentDataAccess>();

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddCors(c =>
//{
//    c.AddPolicy("corsApp", build =>
//        build.AllowAnyOrigin()
//             .AllowAnyMethod()
//             .AllowAnyHeader());
//});

//var app = builder.Build();

//app.UseCors("corsApp");

//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.UseAuthorization();
//app.MapControllers();
//app.Run();




//using Microsoft.EntityFrameworkCore;
//using StudentWepApi.Data;
//using StudentWepApi.Data.StudentContext;

//var builder = WebApplication.CreateBuilder(args);

//// Read DB settings
//var dbHost = builder.Configuration["studentdbHost"];
//var studentDbName = builder.Configuration["studentdbName"];
//var userDbName = builder.Configuration["userDbName"];

//// Connection strings
//var studentDbConn = $"Data Source={dbHost};Initial Catalog={studentDbName};Integrated Security=True;Trust Server Certificate=True";
//var userDbConn = $"Data Source={dbHost};Initial Catalog={userDbName};Integrated Security=True;Trust Server Certificate=True";

//// Register DB context for Student DB (default)
//builder.Services.AddDbContext<StudentDbContext>(options =>
//    options.UseSqlServer(studentDbConn));

//// TODO: if you have another DbContext for login module, register here like:
//// builder.Services.AddDbContext<LoginDbContext>(options =>
////     options.UseSqlServer(userDbConn));

//builder.Services.AddScoped<DbContext, StudentDbContext>();
//builder.Services.AddScoped<IStudentDataAccess, StudentDataAccess>();

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//// CORS
//builder.Services.AddCors(c =>
//{
//    c.AddPolicy("corsApp", build =>
//        build.AllowAnyOrigin()
//             .AllowAnyMethod()
//             .AllowAnyHeader());
//});

//var app = builder.Build();
//app.UseCors("corsApp");

//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.UseAuthorization();
//app.MapControllers();
//app.Run();






//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using StudentWepApi.Data;
//using StudentWepApi.Data.StudentContext;
//using StudentWepApi.Data.UserContext;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//var configuration = builder.Configuration;
//// -------------------- DB Config ---------------------
//var studentDbConn = builder.Configuration.GetConnectionString("JOYDB");
//var loginDbConn = builder.Configuration.GetConnectionString("LOGINMODULEDB");

//// Student DB
//builder.Services.AddDbContext<StudentDbContext>(options =>
//    options.UseSqlServer(studentDbConn));

//// Login Module DB (uncomment when LoginDbContext exists)
//builder.Services.AddDbContext<UserDbContext>(options =>
//    options.UseSqlServer(loginDbConn));

//builder.Services.AddScoped<IStudentDataAccess, StudentDataAccess>();

////builder.Services.AddScoped<AuthService>();

//var jwtSection = configuration.GetSection("JwtSettings");
////var jwtSettings = jwtSection.Get<JwtSettings>();

////var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

//// ------------------ JWT Config ----------------------
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.RequireHttpsMetadata = false;
//    options.SaveToken = true;

//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        //ValidIssuer = jwtSettings.Issuer,

//        ValidateAudience = true,
//        //ValidAudience = jwtSettings.Audience,

//        ValidateIssuerSigningKey = true,
//        //IssuerSigningKey = new SymmetricSecurityKey(key),

//        ValidateLifetime = true,
//        ClockSkew = TimeSpan.FromSeconds(30)
//    };
//});

//// ------------------ Services ------------------------
//builder.Services.AddAuthorization();

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();

//// Swagger Support for JWT
//builder.Services.AddSwaggerGen();

////// CORS
//builder.Services.AddCors(c =>
//{
//    c.AddPolicy("corsApp", build =>
//        build.AllowAnyOrigin()
//             .AllowAnyMethod()
//             .AllowAnyHeader());
//});

//var app = builder.Build();
//app.UseCors("corsApp");

//// Middlewares
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//// Custom JWT validation middleware (runs early)
////app.UseMiddleware<JwtMiddleware>();

//app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();
//app.MapControllers();
//app.Run();

using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineCourse.Application.Mapster;
using OnlineCourse.Infrastructure.Contexts;
using OnlineCourse.Server.Configurations;
using OnlineCourse.Server.Filters;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using System.Text;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomExceptionFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>(); // BU MUHIM
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new Microsoft.AspNetCore.Mvc.Versioning.UrlSegmentApiVersionReader();
});

builder.Services.AddDbContext<OnlineCourseDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefualtConnection"));
});

builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    var config = builder.Configuration.GetSection("Jwt");
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = config["Issuer"],
        ValidateAudience = true,
        ValidAudience = config["Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["SecurityKey"]!))
    };
});


var columnWriters = new Dictionary<string, ColumnWriterBase>
{

    { "user_id", new SinglePropertyColumnWriter("UserId", PropertyWriteMethod.ToString) },
    { "target_table", new SinglePropertyColumnWriter("TargetTable", PropertyWriteMethod.Raw) },
    { "target_table_id", new SinglePropertyColumnWriter("TargetTableId", PropertyWriteMethod.ToString) }
};


Log.Logger = new LoggerConfiguration()
.WriteTo.Console()
.WriteTo.PostgreSQL(
    connectionString: builder.Configuration.GetConnectionString("DefualtConnection"),
    tableName: "logs",
    columnOptions: columnWriters
)
.CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddProjectServices();
builder.DiValidation();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});



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
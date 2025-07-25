using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineCourse.Application.Mapster;
using OnlineCourse.Application.Models.Minio;
using OnlineCourse.Infrastructure.Contexts;
using OnlineCourse.Server.Configurations;
using OnlineCourse.Server.Filters;
using OnlineCourse.Server.Middlewares;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using StackExchange.Redis;
using Stripe;
using System.Net;
using System.Net.Mail;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomExceptionFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
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

    //options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableDetailedErrors(true);
    options.EnableSensitiveDataLogging(true);
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
var smtpPortStr = builder.Configuration["EmailSettings:SmtpPort"];
int smtpPort = string.IsNullOrWhiteSpace(smtpPortStr) ? 5122 : int.Parse(smtpPortStr);

builder.Services
    .AddFluentEmail(builder.Configuration["EmailSettings:FromEmail"])
    .AddRazorRenderer()
    .AddSmtpSender(new SmtpClient(builder.Configuration["EmailSettings:SmtpHost"])
    {
        Port = smtpPort,
        Credentials = new NetworkCredential(
            builder.Configuration["EmailSettings:SmtpUser"],
            builder.Configuration["EmailSettings:SmtpPass"]),
        EnableSsl = true
    });



builder.Services.AddSingleton<IConnectionMultiplexer>
    (ConnectionMultiplexer
    .Connect("localhost:6379,abortConnect=false,connectTimeout=20000,syncTimeout=20000,defaultDatabase=0"));

builder.Services.Configure<MinioSettings>(builder.Configuration.GetSection("MinIO"));
builder.Services.AddSingleton(sp =>
sp.GetRequiredService<IOptions<MinioSettings>>().Value);


StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];


builder.Services.AddHttpContextAccessor();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OnlineCourseDbContext>();
    if (dbContext.Database.IsRelational())
    {
        dbContext.Database.Migrate();
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsProduction() || app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestTimingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Map("/health", appBuilder =>
{
    appBuilder.Run(async context =>
    {
        await context.Response.WriteAsync("Healthy");
    });
});

app.Run();
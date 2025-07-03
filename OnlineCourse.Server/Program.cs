using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineCourse.Infrastructure.Contexts;
using OnlineCourse.Server.Configurations;
using OnlineCourse.Server.Filters;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
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
builder.Services.AddDbContext<OnlineCourseDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefualtConnection"));
});

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

var columnOptions = new ColumnOptions();
columnOptions.Store.Remove(StandardColumn.Message);
columnOptions.Store.Remove(StandardColumn.Level);
columnOptions.Store.Remove(StandardColumn.Exception);
columnOptions.Store.Remove(StandardColumn.Properties);
columnOptions.Store.Remove(StandardColumn.TimeStamp);

columnOptions.AdditionalColumns = new Collection<SqlColumn>
{
    new SqlColumn { ColumnName = "user_id", DataType = SqlDbType.UniqueIdentifier, AllowNull = false },
    new SqlColumn { ColumnName = "target_table", DataType = SqlDbType.Int, AllowNull = false },
    new SqlColumn { ColumnName = "target_table_id", DataType = SqlDbType.NVarChar, DataLength = 200, AllowNull = false }
};

Log.Logger = new LoggerConfiguration()
.WriteTo.Console()
.WriteTo.MSSqlServer(
    connectionString: builder.Configuration.GetConnectionString("SerilogConnection"),
    sinkOptions: new MSSqlServerSinkOptions
    {
        TableName = "logs",
        AutoCreateSqlTable = true
    },
    columnOptions: columnOptions
)
.CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddProjectServices();
builder.DiValidation();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
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
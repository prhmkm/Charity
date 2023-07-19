using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using CharityManagementBackend.Core.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CharityManagementBackend.Core.Model.Base;
using CharityManagementBackend.Data.Base;
using CharityManagementBackend.Domain.Models;
using CharityManagementBackend.Service.Base;
using System.Net;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using NLog;
using Microsoft.AspNetCore;
using CharityManagementBackend.Domain.DTOs;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
Logger logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
WebHost.CreateDefaultBuilder(args).UseNLog();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("CharityManagement", new OpenApiInfo { Title = "CharityManagement", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
});
var xz = SecurityHelpers.DecryptString(builder.Configuration["ConnectionString:DB"], "Ch@r!t7");
//var xz = builder.Configuration["ConnectionString:DB"];
builder.Services.AddDbContext<charityContext>(options => options.UseSqlServer(xz), ServiceLifetime.Transient);
builder.Services.AddDbContext<SampleContext>(options => options.UseSqlServer(xz), ServiceLifetime.Transient);
ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

builder.Services.AddControllers();

builder.Services.AddMvc(option => option.EnableEndpointRouting = false);

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.TokenSecret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IServiceWrapper, ServiceWrapper>();
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseSwagger(c =>
{
    c.PreSerializeFilters.Add((swaggerDoc, httpReq) => httpReq.Scheme = httpReq.Host.Value);
});

app.UseRouting();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseSwaggerUI(c => c.SwaggerEndpoint("/api/swagger/CharityManagement/swagger.json", "CharityManagement v1"));
app.UseSwaggerUI(c => c.InjectStylesheet("/SwaggerHeader.css"));
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


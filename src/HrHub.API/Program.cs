using ConnectionProvider.Container.Bootstrappers;
using HrHub.Abstraction.BusinessRules;
using HrHub.Abstraction.Consts;
using HrHub.Abstraction.Data.Context;
using HrHub.Abstraction.Data.EfCore.Repository;
using HrHub.Abstraction.Data.MongoDb;
using HrHub.Abstraction.Domain;
using HrHub.API.Properties;
using HrHub.Application.Managers.TypeManagers;
using HrHub.Application.Mappers;
using HrHub.Application.Policies;
using HrHub.Container.Bootstrappers;
using HrHub.Core.Base;
using HrHub.Core.Data.UnitOfWork;
using HrHub.Core.Domain.Entity;
using HrHub.Core.Helpers;
using HrHub.Core.IoC;
using ServiceStack;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
AppSettingsHelper.AppSettingsHelperConfigure(builder.Configuration);
ResourceHelper.ConfigureResourceHelper<Resources>("HrHub.API.Properties.Resources");
builder.Services.AddDbContext();
builder.Services.AddMongoDb(builder.Configuration);
builder.Services.RegisterIdentity();
builder.Services.RegisterMapper<MapperProfile>();
//builder.Services.RegisterCache();
builder.Services.RegisterHrHubWorker();
builder.Services.RegisterImplementations<IUnitOfWork<DbContextBase>>("HrHub.Infrastructre");
builder.Services.RegisterImplementations<IRepository<IBaseEntity>>("HrHub.Infrastructre");
builder.Services.RegisterImplementations<IMongoRepository<MongoDbEntity>>("HrHub.Infrastructre");
builder.Services.RegisterImplementations<IBaseManager>("HrHub.Application");
builder.Services.RegisterImplementations<IBusinessRule>("HrHub.Application");
builder.Services.RegisterTypeManagers();
//builder.Services.RegisterManagers();
builder.Services.RegisterNotificationService();
builder.Services.RegisterValidator();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var corsOrigins = builder.Configuration["CorsSettings:AllowedOrigins"];

// 2. Eðer birden fazla adres varsa noktalý virgül ile ayýr (Array'e çevir)
var allowedOrigins = corsOrigins?.Split(";", StringSplitOptions.RemoveEmptyEntries);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            if (allowedOrigins != null && allowedOrigins.Length > 0)
            {
                policy.WithOrigins(allowedOrigins) // Okunan adresleri buraya veriyoruz
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            }
            else
            {
                // Güvenlik önlemi: Eðer appsettings boþsa en azýndan localhost:4200'e izin ver (Geliþtirme için)
                policy.WithOrigins("http://localhost:4200")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            }
        });
});


builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy(Policies.MainUser, policy =>
    {
        policy.RequireAssertion(context =>
        {
            if (context.User.IsInRole(Roles.User))
            {
                return context.User.HasClaim(c => c.Type == Policies.MainUser && c.Value.ToLower() == "true");
            }
            return true;
        });
    });


    opts.AddPolicy(Policies.Instructor, policy =>
    {
        policy.Requirements.Add(new InstractorRequirement());
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.IntegrationHelperConfig();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.AddWorkerDashboard();
app.MapControllers();
app.UseCors("AllowAngularApp");

app.Run();

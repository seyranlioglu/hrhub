using ConnectionProvider.Container.Bootstrappers;
using HrHub.Abstraction.Data.Context;
using HrHub.Abstraction.Data.EfCore.Repository;
using HrHub.Abstraction.Data.MongoDb;
using HrHub.Abstraction.Domain;
using HrHub.API.Properties;
using HrHub.Application.Mappers;
using HrHub.Container.Bootstrappers;
using HrHub.Core.Base;
using HrHub.Core.Domain.Entity;
using HrHub.Core.Helpers;
using HrHub.Core.IoC;
using HrHub.Core.Rules;
using HrHub.Application.Mappers;
using HrHub.Worker.IoC;
using ConnectionProvider.Container.Bootstrappers;
using HrHub.Core.Data.UnitOfWork;

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
builder.Services.RegisterTypeManagers();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("MainUser", policy =>
    {
        policy.RequireAssertion(context =>
        {
            if (context.User.IsInRole("admin"))
            {
                return true;
            }
            if (context.User.IsInRole("user"))
            {
                return context.User.HasClaim(c => c.Type == "IsMainUser" && c.Value.ToLower() == "true");
            }
            return false;
        });
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.AddWorkerDashboard();
app.MapControllers();

app.Run();

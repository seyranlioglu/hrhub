using HrHub.API.Properties;
using HrHub.Core.Helpers;
using HrHub.Container.Bootstrappers;
using HrHub.Abstraction.Data.Context;
using HrHub.Abstraction.Data.EfCore.Repository;
using HrHub.Abstraction.Data.MongoDb;
using HrHub.Abstraction.Domain;
using HrHub.Core.Base;
using HrHub.Core.Domain.Entity;
using HrHub.Core.IoC;
using HrHub.Core.BusinessRules;
using HrHub.Application.Mappers;
using HrHub.Worker.IoC;
using ConnectionProvider.Container.Bootstrappers;
using HrHub.Core.Data.UnitOfWork;
using HrHub.Abstraction.BusinessRules;

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
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.AddWorkerDashboard();

app.MapControllers();

app.Run();

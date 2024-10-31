namespace HrHub.Core.Data.UnitOfWork
{
    public static class DbContextExtentions
    {
        //public static ContainerBuilder AddDbContext<TContext>(this ContainerBuilder builder, ModuleInfo moduleInfo, string connectionKey = null)
        //    where TContext : DbContextBase
        //{
        //    builder
        //        .Register(c =>
        //        {
        //            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
        //            var loggerFactory = c.Resolve<ILoggerFactory>();
        //            optionsBuilder.UseLoggerFactory(loggerFactory).EnableDetailedErrors().EnableSensitiveDataLogging();

        //            var dbConnMng = c.Resolve<IDatabaseConnectionFactory>();
        //            var dbConnKey = connectionKey ?? moduleInfo.Name;
        //            var dbConn = dbConnMng.GetDbConnection(dbConnKey);

        //            var configuration = c.Resolve<IConfiguration>();
        //            var moduleConfiguration = configuration.GetConfigurationByModule(moduleInfo);
        //            var dbType = moduleConfiguration.GetSection($"DatabaseSettings:Databases:{dbConnKey}:Type")
        //                .Get<DatabaseType>();

        //            switch (dbType)
        //            {
        //                case DatabaseType.SqlServer:
        //                    optionsBuilder.UseSqlServer<TContext>(dbConn).AddInterceptors(
        //                        new LogDbCommandInterceptor(c.Resolve<IExecutionContextAccessor>(), loggerFactory, c.Resolve<IUserContextAccessor>()));
        //                    break;
        //                case DatabaseType.Oracle:
        //                    optionsBuilder.UseOracle<TContext>(dbConn).AddInterceptors(
        //                        new LogDbCommandInterceptor(c.Resolve<IExecutionContextAccessor>(), loggerFactory, c.Resolve<IUserContextAccessor>()));
        //                    optionsBuilder.ConfigureWarnings(wc => wc.Ignore(RelationalEventId.BoolWithDefaultWarning));
        //                    break;
        //                default:
        //                    throw new NotSupportedException($"Name:{moduleInfo.Name} Type:{dbType} is not supported");
        //            }

        //            return (TContext)Activator.CreateInstance(typeof(TContext), optionsBuilder.Options);
        //        })
        //        .AsSelf()
        //        .As<DbContext>()
        //        .As<DbContextBase>()
        //        .As<TContext>()
        //        .InstancePerLifetimeScope();

        //    return builder;
        //}
    }
}
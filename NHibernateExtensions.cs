using ASPNETAOP.Session;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP
{
    public static class NHibernateExtensions
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services, string connectionString)
        {
            var mapper = new ModelMapper();
            mapper.AddMappings(typeof(NHibernateExtensions).Assembly.ExportedTypes);
            HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            var configuration = new Configuration();
            configuration.DataBaseIntegration(c =>
            {
                c.Dialect<MsSql2012Dialect>();
                c.ConnectionString = connectionString;
                c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                c.SchemaAction = SchemaAutoAction.Validate;
                c.LogFormattedSql = true;
                c.LogSqlInConsole = true;
            });
            //new SchemaExport(configuration).Create(false, true);
            //I tried to add a schema because I get an exception that
            //An unhandled exception of type 'NHibernate.SchemaValidationException' occurred in NHibernate.dll
            //Schema validation failed: see list of validation errors
            //In given article they use schemas
            //https://www.c-sharpcorner.com/article/work-with-fluent-nhibernate-in-core-2-0/
            //but because they use postgresql and when I tried to change SessionFactoryBuilder method to
            //accept mssql database I get lot of errors I tried this code.
            //the link of this code: https://gunnarpeipman.com/aspnet-core-nhibernate/

            configuration.AddMapping(domainMapping);

            var sessionFactory = configuration.BuildSessionFactory();

            services.AddSingleton(sessionFactory);
            services.AddScoped(factory => sessionFactory.OpenSession());
            services.AddScoped<NHibernateMapperSession>();

            return services;
        }
    }
}

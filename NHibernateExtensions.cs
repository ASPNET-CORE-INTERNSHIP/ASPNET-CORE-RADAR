using ASPNETAOP.Session;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.SqlCommand;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            
            configuration.AddMapping(domainMapping);

            var sessionFactory = configuration.BuildSessionFactory();
            services.AddScoped(factory => {

#if (DEBUG)
                var interceptor = new SqlDebugOutputInterceptor();
                var session = sessionFactory.WithOptions()
                                            .Interceptor(interceptor)
                                            .OpenSession();
#else
    return sessionFactory.OpenSession();
#endif
                return session;

            });
            services.AddSingleton(sessionFactory);
            //services.AddScoped(factory => sessionFactory.OpenSession());
            services.AddScoped<NHibernateMapperSession>();

            return services;
        }
    }

    public class SqlDebugOutputInterceptor : EmptyInterceptor
    {
        public override SqlString OnPrepareStatement(SqlString sql)
        {
            Debug.Write("NHibernate: ");
            Debug.WriteLine(sql);

            return base.OnPrepareStatement(sql);
        }
    }
}

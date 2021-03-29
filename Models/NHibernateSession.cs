using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetCore.Models
{
    public class NHIbernateSession
    {
        /*public static ISession OpenSession()
        {

            var configuration = new Configuration();

            var configurationPath = HttpContext.Current.Server.MapPath(@"~\Models\Nhibernate\hibernate.cfg.xml");

            configuration.Configure(configurationPath);

            var employeeConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Models\NHibernate\Employee.hbm.xml");

            configuration.AddFile(employeeConfigurationFile);

            ISessionFactory sessionFactory = configuration.BuildSessionFactory();

            return sessionFactory.OpenSession();

        }*/
    }
}
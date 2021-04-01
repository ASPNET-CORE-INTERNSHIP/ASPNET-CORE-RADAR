using NHibernate;
using NHibernate.Cfg;
using ASPNETAOP.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace ASPNETAOAP.Models
{
    public class NHIbernateSession
    {
        /*
        public static ISession OpenSession()
        {

            var configuration = new Configuration();

            var configurationPath = HttpContext.Server.MapPath(@"~\Models\Nhibernate\hibernate.cfg.xml");

            configuration.Configure(configurationPath);

            var antennaConfigurationFile = AppHttpContext.Current.Server.MapPath(@"~\Models\NHibernate\AddAntenna.hbm.xml");

            configuration.AddFile(antennaConfigurationFile);

            var transmitterConfigurationFile = AppHttpContext.Current.Server.MapPath(@"~\Models\NHibernate\AddTransmitter.hbm.xml");

            configuration.AddFile(transmitterConfigurationFile);

            var receiverConfigurationFile = AppHttpContext.Current.Server.MapPath(@"~\Models\NHibernate\AddReceiver.hbm.xml");

            configuration.AddFile(receiverConfigurationFile);

            var antenna_scanConfigurationFile = AppHttpContext.Current.Server.MapPath(@"~\Models\NHibernate\AddAntennaScan.hbm.xml");

            configuration.AddFile(antenna_scanConfigurationFile);

            var locationConfigurationFile = AppHttpContext.Current.Server.MapPath(@"~\Models\NHibernate\AddLocation.hbm.xml");

            configuration.AddFile(locationConfigurationFile);

            var modeConfigurationFile = AppHttpContext.Current.Server.MapPath(@"~\Models\NHibernate\AddMode.hbm.xml");

            configuration.AddFile(modeConfigurationFile);

            var submodeConfigurationFile = AppHttpContext.Current.Server.MapPath(@"~\Models\NHibernate\AddSubmode.hbm.xml");

            configuration.AddFile(submodeConfigurationFile);

            var scanConfigurationFile = AppHttpContext.Current.Server.MapPath(@"~\Models\NHibernate\AddScan.hbm.xml");

            configuration.AddFile(scanConfigurationFile);

            var radarConfigurationFile = AppHttpContext.Current.Server.MapPath(@"~\Models\NHibernate\AddRadar.hbm.xml");

            configuration.AddFile(radarConfigurationFile);

            ISessionFactory sessionFactory = configuration.BuildSessionFactory();

            return sessionFactory.OpenSession();

        }
        */
    }
}
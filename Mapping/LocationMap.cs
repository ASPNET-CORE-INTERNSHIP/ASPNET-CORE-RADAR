using ASPNETAOP.Models;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Mapping
{
    public class LocationMap : ClassMapping<Location>
    {
        public LocationMap()
        {
            Id(x => x.ID, x =>
            {
                x.Generator(Generators.Guid);
                x.Type(NHibernateUtil.Guid);
                x.Column("ID");
                x.UnsavedValue(Guid.Empty);
            });

            Property(b => b.name, x =>
            {
                x.Length(500);
                x.Type(NHibernateUtil.StringClob);
                x.NotNullable(false);
            });

            Property(b => b.country, x =>
            {
                x.Type(NHibernateUtil.StringClob);
            });

            Property(b => b.city, x =>
            {
                x.Type(NHibernateUtil.StringClob);
            });

            Property(b => b.geographic_latitude, x =>
            {
                x.Type(NHibernateUtil.StringClob);
            });

            Property(b => b.geographic_longitude, x =>
            {
                x.Type(NHibernateUtil.StringClob);
            });

            Property(b => b.airborne, x =>
            {
                x.Type(NHibernateUtil.StringClob);
            });

            Table("Location");
        }
    }
}

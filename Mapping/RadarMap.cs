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
    public class RadarMap : ClassMapping<Radar>
    {
        public RadarMap()
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

            Property(b => b.system, x =>
            {
                x.Type(NHibernateUtil.StringClob);
            });

            Property(b => b.configuration, x =>
            {
                x.Type(NHibernateUtil.StringClob);
            });

            Property(b => b.transmitter_id, x =>
            {
                x.Type(NHibernateUtil.Guid);
            });

            Property(b => b.receiver_id, x =>
            {
                x.Type(NHibernateUtil.Guid);
            });

            Property(b => b.location_id, x =>
            {
                x.Type(NHibernateUtil.Guid);
            });

            Table("Radar");
        }
    }
}

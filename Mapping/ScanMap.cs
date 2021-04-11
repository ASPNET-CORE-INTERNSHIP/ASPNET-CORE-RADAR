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
    public class ScanMap : ClassMapping<Scan>
    {
        public ScanMap()
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

            Property(b => b.type, x =>
            {
                x.Type(NHibernateUtil.StringClob);
            });

             Property(b => b.main_aspect, x =>
            {
                x.Type(NHibernateUtil.StringClob);
            });

            Property(b => b.scan_angle, x =>
            {
                x.Type(NHibernateUtil.Double);
            });

            Property(b => b.scan_rate, x =>
            {
                x.Type(NHibernateUtil.Double);
            });

            Property(b => b.hits_per_scan, x =>
            {
                x.Type(NHibernateUtil.Int32);
            });

            Table("Scan");
        }
    }
}

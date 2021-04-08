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
    public class SubmodeMap : ClassMapping<Submode>
    {
        public SubmodeMap()
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

            Property(b => b.mode_id, x =>
            {
                x.Type(NHibernateUtil.Guid);
            });

            Property(b => b.PRI, x =>
            {
                x.Type(NHibernateUtil.Double);
            });

            Property(b => b.PW, x =>
            {
                x.Type(NHibernateUtil.Double);
            });

            Property(b => b.min_frequency, x =>
            {
                x.Type(NHibernateUtil.StringClob);
            });

            Property(b => b.max_frequency, x =>
            {
                x.Type(NHibernateUtil.Int32);
            });

            Property(b => b.scan_id, x =>
            {
                x.Type(NHibernateUtil.Guid);
            });

            Table("Submode");
        }
    }
}


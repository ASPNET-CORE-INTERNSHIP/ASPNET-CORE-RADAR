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
    public class TransmitterMap : ClassMapping<Transmitter>
    {
        public TransmitterMap()
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

            Property(b => b.modulation_type, x =>
            {
                x.Type(NHibernateUtil.StringClob);
            });

            Property(b => b.max_frequency, x =>
            {
                x.Type(NHibernateUtil.Int32);
            });

            Property(b => b.min_frequency, x =>
            {
                x.Type(NHibernateUtil.Int32);
            });

            Property(b => b.power, x =>
            {
                x.Type(NHibernateUtil.Int32);
            });

            Table("Transmitter");
        }
    }
}

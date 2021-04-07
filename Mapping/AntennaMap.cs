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
    public class AntennaMap : ClassMapping<Antenna>
    {
        public AntennaMap()
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

            Property(b => b.horizontal_beamwidth, x =>
            {
                x.Type(NHibernateUtil.Double);
            });

            Property(b => b.vertical_beamwidth, x =>
            {
                x.Type(NHibernateUtil.Double);
            });

             Property(b => b.polarization, x =>
            {
                x.Type(NHibernateUtil.StringClob);
            });

            Property(b => b.number_of_feed, x =>
            {
                x.Type(NHibernateUtil.Int32);
            });

            Property(b => b.horizontal_dimension, x =>
            {
                x.Type(NHibernateUtil.Double);
            });

            Property(b => b.vertical_dimension, x =>
            {
                x.Type(NHibernateUtil.Double);
            });

            Property(b => b.duty, x =>
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

            Property(b => b.location, x =>
            {
                x.Type(NHibernateUtil.StringClob);
            });

            Table("Antenna");
        }
    }
}

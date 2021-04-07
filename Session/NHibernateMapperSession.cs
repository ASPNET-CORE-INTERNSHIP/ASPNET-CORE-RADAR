using ASPNETAOP.Models;
using NHibernate;
using NHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Session
{
    public class NHibernateMapperSession 
    {
        private readonly ISession _session;
        private ITransaction _transaction;

        public NHibernateMapperSession(ISession session)
        {
            _session = session;
        }

        public void BeginTransaction()
        {
            _transaction = _session.BeginTransaction();
        }

        public async Task Commit()
        {
            await _transaction.CommitAsync();
        }

        public async Task Rollback()
        {
            await _transaction.RollbackAsync();
        }

        public void CloseTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public async Task SaveReceiver(Receiver entity)
        {
            ISQLQuery query = _session.CreateSQLQuery("INSERT INTO Receiver VALUES (:ID, :name, :listening_time, :rest_time, :recovery_time)");
            query.SetParameter("ID", entity.ID);
            query.SetParameter("name", entity.name);
            query.SetParameter("listening_time", entity.listening_time);
            query.SetParameter("rest_time", entity.rest_time);
            query.SetParameter("recovery_time", entity.recovery_time);
            query.ExecuteUpdate();
        }

        public async Task<int> GetReceiverNumber()
        {
            var num = _session.CreateSQLQuery("SELECT COUNT(*) FROM Receiver").UniqueResult<int>();
            return num;
        }

        public async Task DeleteReceiver(Receiver entity)
        {
            await _session.DeleteAsync(entity);
        }

        public async Task SaveTransmitter(Transmitter entity)
        {
            Console.WriteLine(entity.name + " " + entity.modulation_type + " " + entity.max_frequency + " " + entity.min_frequency+ "  " + entity.power);
            ISQLQuery query = _session.CreateSQLQuery("INSERT INTO Transmitter(:ID, :name, :modulation_type, :max_frequency, :min_frequency, :power)");
            query.SetParameter("ID", entity.ID);
            query.SetParameter("name", entity.name);
            query.SetParameter("modulation_type", entity.modulation_type);
            query.SetParameter("max_frequency", entity.max_frequency);
            query.SetParameter("min_frequency", entity.min_frequency);
            query.SetParameter("power", entity.power);
            query.ExecuteUpdate();
        }

        public async Task SaveAntenna(Antenna entity)
        {
            ISQLQuery query = _session.CreateSQLQuery("INSERT INTO Antenna VALUES (:ID, :name, :type, :horizontal_beamwidth, :vertical_beamwidth, :polarization, :number_of_feed, :horizontal_dimension, :vertical_dimension, :duty, :transmitter_id, :receiver_id, :location)");
            query.SetParameter("ID", entity.ID);
            query.SetParameter("name", entity.name);
            query.SetParameter("type", entity.type);
            query.SetParameter("horizontal_beamwidth", entity.horizontal_beamwidth);
            query.SetParameter("vertical_beamwidth", entity.vertical_beamwidth);
            query.SetParameter("polarization", entity.polarization);
            query.SetParameter("number_of_feed", entity.number_of_feed);
            query.SetParameter("horizontal_dimension", entity.horizontal_dimension);
            query.SetParameter("vertical_dimension", entity.vertical_dimension);
            query.SetParameter("duty", entity.duty);
            query.SetParameter("transmitter_id", entity.transmitter_id);
            query.SetParameter("receiver_id", entity.receiver_id);
            query.SetParameter("location", entity.location);
            query.ExecuteUpdate();
        }

        public async Task<String> SelectTransmitter(Guid ID)
        {
            var transmitter_name = _session.CreateSQLQuery("SELECT name FROM Transmitter WHERE ID = :ID").SetParameter("ID", ID).UniqueResult<string>();
            return transmitter_name;
        }

        public async Task<String> SelectReceiver(Guid ID)
        {
            var transmitter_name = _session.CreateSQLQuery("SELECT name FROM Receiver WHERE ID = :ID").SetParameter("ID", ID).UniqueResult<string>();
            return transmitter_name;
        }
    }
}

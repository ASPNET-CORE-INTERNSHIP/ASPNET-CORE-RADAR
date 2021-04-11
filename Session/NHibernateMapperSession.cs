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

        public async Task<String> SelectReceiver(Guid ID)
        {
            var transmitter_name = _session.CreateSQLQuery("SELECT name FROM Receiver WHERE ID = :ID").SetParameter("ID", ID).UniqueResult<string>();
            return transmitter_name;
        }

        internal void RenameReceiver(Guid id, String newName)
        {
            ISQLQuery query = _session.CreateSQLQuery("UPDATE Receiver SET name = :name WHERE ID = :ID");
            query.SetParameter("ID", id);
            query.SetParameter("name", newName);
            query.ExecuteUpdate();
        }


        public async Task SaveTransmitter(Transmitter entity)
        {
            ISQLQuery query = _session.CreateSQLQuery("INSERT INTO Transmitter VALUES(:ID, :name, :modulation_type, :max_frequency, :min_frequency, :power)");
            query.SetParameter("ID", entity.ID);
            query.SetParameter("name", entity.name);
            query.SetParameter("modulation_type", entity.modulation_type);
            query.SetParameter("max_frequency", entity.max_frequency);
            query.SetParameter("min_frequency", entity.min_frequency);
            query.SetParameter("power", entity.power);
            query.ExecuteUpdate();
        }

        public async Task<int> GetTransmitterNumber()
        {
            var num = _session.CreateSQLQuery("SELECT COUNT(*) FROM Transmitter").UniqueResult<int>();
            return num;
        }

        internal void RenameTransmitter(Guid id, String newName)
        {
            ISQLQuery query = _session.CreateSQLQuery("UPDATE Transmitter SET name = :name WHERE ID = :ID");
            query.SetParameter("ID", id);
            query.SetParameter("name", newName);
            query.ExecuteUpdate();
        }

        public async Task<String> SelectTransmitter(Guid ID)
        {
            var transmitter_name = _session.CreateSQLQuery("SELECT name FROM Transmitter WHERE ID = :ID").SetParameter("ID", ID).UniqueResult<string>();
            return transmitter_name;
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

        internal void RenameAntenna(Guid ID, string newName)
        {
            ISQLQuery query = _session.CreateSQLQuery("UPDATE Antenna SET name = :name WHERE ID = :ID");
            query.SetParameter("ID", ID);
            query.SetParameter("name", newName);
            query.ExecuteUpdate();
        }

        public List<Guid> SelectAntennasUsingReceiverOrTransmitter(Guid ID)
        {
            String sql = "SELECT ID FROM Antenna WHERE receiver_id = :id OR transmitter_id = :id";
            ISQLQuery query = _session.CreateSQLQuery(sql);
            query.SetParameter("ID", ID);
            List<Guid> results = (List<Guid>)query.List();
            return results;
        }

        public async Task SaveRadar(Radar entity)
        {
            ISQLQuery query = _session.CreateSQLQuery("INSERT INTO Radar VALUES(:ID, :name, :system, :configuration, :transmitter_id, :receiver_id, :location_id)");
            query.SetParameter("ID", entity.ID);
            query.SetParameter("name", entity.name);
            query.SetParameter("system", entity.system);
            query.SetParameter("configuration", entity.configuration);
            query.SetParameter("transmitter_id", entity.transmitter_id);
            query.SetParameter("receiver_id", entity.receiver_id);
            query.SetParameter("location_id", entity.location_id);
            query.ExecuteUpdate();
        }

        public async Task<int> GetRadarNumber()
        {
            var num = _session.CreateSQLQuery("SELECT COUNT(*) FROM Radar").UniqueResult<int>();
            return num;
        }

        public async Task SaveLocation(Location entity)
        {
            ISQLQuery query = _session.CreateSQLQuery("INSERT INTO Location VALUES(:ID, :name, :country, :city, :geographic_latitude, :geographic_longitude, :airborne)");
            query.SetParameter("ID", entity.ID);
            query.SetParameter("name", entity.name);
            query.SetParameter("country", entity.country);
            query.SetParameter("city", entity.city);
            query.SetParameter("geographic_latitude", entity.geographic_latitude);
            query.SetParameter("geographic_longitude", entity.geographic_longitude);
            query.SetParameter("airborne", entity.airborne);
            query.ExecuteUpdate();
        }

        public async Task<int> GetLocationName(String country, String city)
        {
            int number = _session.CreateSQLQuery("SELECT COUNT(*) FROM Location WHERE country = :country AND city = :city").SetParameter("country", country).SetParameter("city", city).UniqueResult<int>();
            return number;
        }

        public async Task SaveMode(Mode entity)
        {
            ISQLQuery query = _session.CreateSQLQuery("INSERT INTO Mode VALUES(:ID, :name, :radar_id)");
            query.SetParameter("ID", entity.ID);
            query.SetParameter("name", entity.name);
            query.SetParameter("radar_id", entity.radar_id);
            query.ExecuteUpdate();
        }

        public async Task SaveSubMode(Submode entity)
        {
            ISQLQuery query = _session.CreateSQLQuery("INSERT INTO Submode VALUES(:ID, :name, :mode_id, :PW, :PRI, :min_frequency, :max_frequency, :scan_id)");
            query.SetParameter("ID", entity.ID);
            query.SetParameter("name", entity.name);
            query.SetParameter("mode_id", entity.mode_id);
            query.SetParameter("PW", entity.PW);
            query.SetParameter("PRI", entity.PRI);
            query.SetParameter("min_frequency", entity.min_frequency);
            query.SetParameter("max_frequency", entity.max_frequency);
            query.SetParameter("scan_id", entity.scan_id);
            query.ExecuteUpdate();
        }

        public async Task SaveScan(Scan entity)
        {
            ISQLQuery query = _session.CreateSQLQuery("INSERT INTO Scan VALUES(:ID, :name, :type, :main_aspect, :scan_angle, :scan_rate, :hits_per_scan)");
            query.SetParameter("ID", entity.ID);
            query.SetParameter("name", entity.name);
            query.SetParameter("type", entity.type);
            query.SetParameter("main_aspect", entity.main_aspect);
            query.SetParameter("scan_angle", entity.scan_angle);
            query.SetParameter("scan_rate", entity.scan_rate);
            query.SetParameter("hits_per_scan", entity.hits_per_scan);
            query.ExecuteUpdate();
        }

        public async Task SaveAntennaScan(AntennaScan entity)
        {
            ISQLQuery query = _session.CreateSQLQuery("INSERT INTO AntennaScan VALUES(:antenna_id, :scan_id)");
            query.SetParameter("antenna_id", entity.antenna_id);
            query.SetParameter("scan_id", entity.scan_id);
            query.ExecuteUpdate();
        }

        public async Task DeleteAntennaScan(AntennaScan entity)
        {
            ISQLQuery query = _session.CreateSQLQuery("DELETE FROM AntennaScan WHERE antenna_id = :antenna_id AND scan_id = :scan_id");
            query.SetParameter("antenna_id", entity.antenna_id);
            query.SetParameter("scan_id", entity.scan_id);
            query.ExecuteUpdate();
        }
    }
}

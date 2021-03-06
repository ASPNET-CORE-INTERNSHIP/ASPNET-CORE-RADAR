﻿using ASPNETAOP.Models;
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
            if (_transaction != null)
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

        public IQueryable<Receiver> Receivers => _session.Query<Receiver>();

        public IQueryable<Transmitter> Transmitters => _session.Query<Transmitter>();

        public IQueryable<Radar> Radars => _session.Query<Radar>();

        public IQueryable<Location> Location => _session.Query<Location>();

        public IQueryable<Antenna> Antennas => _session.Query<Antenna>();

        public IQueryable<Mode> Modes => _session.Query<Mode>();

        public IQueryable<Submode> Submode => _session.Query<Submode>();

        public IQueryable<Scan> Scan => _session.Query<Scan>();

        public IQueryable<AntennaScan> AntennaScans => _session.Query<AntennaScan>();

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

        public async Task EditRadar(Guid id, String name, String system, String configuration)
        {
            ISQLQuery query = _session.CreateSQLQuery("UPDATE Radar SET name = :name, system = :system, configuration = :configuration WHERE ID = :ID");
            query.SetParameter("ID", id);
            query.SetParameter("name", name);
            query.SetParameter("system", system);
            query.SetParameter("configuration", configuration);
            query.ExecuteUpdate();
        }

        public async Task<int> GetRadarNumber()
        {
            var num = _session.CreateSQLQuery("SELECT COUNT(*) FROM Radar").UniqueResult<int>();
            return num;
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

        internal void RenameTransmitter(Guid id, String newName)
        {
            ISQLQuery query = _session.CreateSQLQuery("UPDATE Transmitter SET name = :name WHERE ID = :ID");
            query.SetParameter("ID", id);
            query.SetParameter("name", newName);
            query.ExecuteUpdate();
        }

        public async Task DeleteTransmitter(Guid ID)
        {
            ISQLQuery query = _session.CreateSQLQuery("DELETE FROM Transmitter WHERE ID = :ID");
            query.SetParameter("ID", ID);
            query.ExecuteUpdate();
        }

        public async Task EditTransmitter(Guid id, string name, string modulation_type, double max_frequency, double min_frequency, int power)
        {
            ISQLQuery query = _session.CreateSQLQuery("UPDATE Transmitter SET name = :name, modulation_type = :modulation_type, max_frequency = :max_frequency, min_frequency = :min_frequency, power = :power WHERE ID = :ID");
            query.SetParameter("ID", id);
            query.SetParameter("name", name);
            query.SetParameter("modulation_type", modulation_type);
            query.SetParameter("max_frequency", max_frequency);
            query.SetParameter("min_frequency", min_frequency);
            query.SetParameter("power", power);
            query.ExecuteUpdate();
        }

        public async Task<int> GetTransmitterNumber()
        {
            var num = _session.CreateSQLQuery("SELECT COUNT(*) FROM Transmitter").UniqueResult<int>();
            return num;
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

        internal void RenameReceiver(Guid id, String newName)
        {
            ISQLQuery query = _session.CreateSQLQuery("UPDATE Receiver SET name = :name WHERE ID = :ID");
            query.SetParameter("ID", id);
            query.SetParameter("name", newName);
            query.ExecuteUpdate();
        }

        public async Task DeleteReceiver(Guid ID)
        {
            ISQLQuery query = _session.CreateSQLQuery("DELETE FROM Receiver WHERE ID = :ID");
            query.SetParameter("ID", ID);
            query.ExecuteUpdate();
        }

        public async Task EditReceiver(Guid id, string name, double listening_time, double rest_time, double recovery_time)
        {
            ISQLQuery query = _session.CreateSQLQuery("UPDATE Receiver SET name = :name, listening_time = :listening_time, rest_time = :rest_time, recovery_time = :recovery_time WHERE ID = :ID");
            query.SetParameter("ID", id);
            query.SetParameter("name", name);
            query.SetParameter("listening_time", listening_time);
            query.SetParameter("rest_time", rest_time);
            query.SetParameter("recovery_time", recovery_time);
            query.ExecuteUpdate();
        }

        public async Task<int> GetReceiverNumber()
        {
            var num = _session.CreateSQLQuery("SELECT COUNT(*) FROM Receiver").UniqueResult<int>();
            return num;
        }

        public async Task SaveMode(Mode entity)
        {
            ISQLQuery query = _session.CreateSQLQuery("INSERT INTO Mode VALUES(:ID, :name, :radar_id)");
            query.SetParameter("ID", entity.ID);
            query.SetParameter("name", entity.name);
            query.SetParameter("radar_id", entity.radar_id);
            query.ExecuteUpdate();
        }

        public async Task DeleteMode(Guid id)
        {
            ISQLQuery query = _session.CreateSQLQuery("DELETE FROM Mode WHERE ID = :ID");
            query.SetParameter("ID", id);
            query.ExecuteUpdate();
        }

        public async Task EditMode(Mode mode)
        {
            ISQLQuery query = _session.CreateSQLQuery("UPDATE Mode SET name = :name WHERE ID = :ID");
            query.SetParameter("ID", mode.ID);
            query.SetParameter("name", mode.name);
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

        public async Task EditSubmode(Submode entity)
        {
            ISQLQuery query = _session.CreateSQLQuery("UPDATE Submode SET name = :name, PW = :PW, PRI = :PRI, min_frequency = :min_frequency, max_frequency = :max_frequency WHERE ID = :ID");
            query.SetParameter("ID", entity.ID);
            query.SetParameter("name", entity.name);
            query.SetParameter("PW", entity.PW);
            query.SetParameter("PRI", entity.PRI);
            query.SetParameter("min_frequency", entity.min_frequency);
            query.SetParameter("max_frequency", entity.max_frequency);
            query.ExecuteUpdate();
        }

        public async Task DeleteSubMode(Guid id)
        {
            ISQLQuery query = _session.CreateSQLQuery("DELETE FROM Submode WHERE ID = :ID");
            query.SetParameter("ID", id);
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

        public async Task EditScan(Scan entity)
        {
            ISQLQuery query = _session.CreateSQLQuery("UPDATE Scan SET name = :name, type = :type, main_aspect = :main_aspect, scan_angle = :scan_angle, scan_rate = :scan_rate, hits_per_scan = :hits_per_scan WHERE ID = :ID");
            query.SetParameter("ID", entity.ID);
            query.SetParameter("name", entity.name);
            query.SetParameter("type", entity.type);
            query.SetParameter("main_aspect", entity.main_aspect);
            query.SetParameter("scan_angle", entity.scan_angle);
            query.SetParameter("scan_rate", entity.scan_rate);
            query.SetParameter("hits_per_scan", entity.hits_per_scan);
            query.ExecuteUpdate();
        }

        public async Task DeleteScan(Guid id)
        {
            ISQLQuery query = _session.CreateSQLQuery("DELETE FROM Scan WHERE ID in " +
                "(SELECT ID FROM Submode WHERE mode_id IN( SELECT ID FROM Mode " +
                "WHERE radar_id = :ID)); ");
            query.SetParameter("ID", id);
            query.ExecuteUpdate();
        }

        public async Task DeleteScanbyID(Guid id)
        {
            ISQLQuery query = _session.CreateSQLQuery("DELETE FROM Scan WHERE ID = :ID");
            query.SetParameter("ID", id);
            query.ExecuteUpdate();
        }

        public async Task<Guid> GetScanID(Guid id)
        {
            Guid scan_id = _session.CreateSQLQuery("SELECT scan_id FROM Submode WHERE ID = :ID").SetParameter("ID", id).UniqueResult<Guid>();
            return scan_id;
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

        public async Task DeleteAntennaScanUsingScanID(Guid id)
        {
            ISQLQuery query = _session.CreateSQLQuery("DELETE FROM AntennaScan WHERE scan_id = :scan_id");
            query.SetParameter("scan_id", id);
            query.ExecuteUpdate();
        }

        public async Task<Guid> SelectAntennaScan(Guid id_antenna, Guid id_scan)
        {
            Guid id = _session.CreateSQLQuery("SELECT scan_id FROM AntennaScan WHERE scan_id = :scan_id AND antenna_id = :antenna_id").SetParameter("scan_id", id_scan).SetParameter("antenna_id", id_antenna).UniqueResult<Guid>();
            return id;
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

        public async Task EditAntenna(Guid id, string name, string type, double horizontal_beamwidth, double vertical_beamwidth, string polarization, int number_of_feed, double horizontal_dimension, double vertical_dimension, string location)
        {
            ISQLQuery query = _session.CreateSQLQuery("UPDATE Antenna SET name = :name, type = :type, horizontal_beamwidth = :horizontal_beamwidth, vertical_beamwidth = :vertical_beamwidth, polarization = :polarization, number_of_feed = :number_of_feed, horizontal_dimension = :horizontal_dimension, vertical_dimension = :vertical_dimension, location = :location  WHERE ID = :ID");
            query.SetParameter("ID", id);
            query.SetParameter("name", name);
            query.SetParameter("type", type);
            query.SetParameter("horizontal_beamwidth", horizontal_beamwidth);
            query.SetParameter("vertical_beamwidth", vertical_beamwidth);
            query.SetParameter("polarization", polarization);
            query.SetParameter("number_of_feed", number_of_feed);
            query.SetParameter("horizontal_dimension", horizontal_dimension);
            query.SetParameter("vertical_dimension", vertical_dimension);
            query.SetParameter("location", location);
            query.ExecuteUpdate();
        }

        internal void RenameAntenna(Guid ID, string newName)
        {
            ISQLQuery query = _session.CreateSQLQuery("UPDATE Antenna SET name = :name WHERE ID = :ID");
            query.SetParameter("ID", ID);
            query.SetParameter("name", newName);
            query.ExecuteUpdate();
        }

        public async Task DeleteAntenna(Guid id)
        {
            ISQLQuery query = _session.CreateSQLQuery("DELETE FROM Antenna WHERE ID = :ID");
            query.SetParameter("ID", id);
            query.ExecuteUpdate();
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

        public async Task<int> GetLocationName(string airborne)
        {
            int number = _session.CreateSQLQuery("SELECT COUNT(*) FROM Location WHERE airborne = :airborne").SetParameter("airborne", airborne).UniqueResult<int>();
            return number;
        }

        public async Task DeleteLocation(Guid id)
        {
            ISQLQuery query = _session.CreateSQLQuery("DELETE FROM Location WHERE ID = :ID");
            query.SetParameter("ID", id);
            query.ExecuteUpdate();
        }

        public async Task EditLocation(Location entity)
        {
            ISQLQuery query = _session.CreateSQLQuery("UPDATE Location SET name = :name, country = :country, city = :city, geographic_latitude = :geographic_latitude, geographic_longitude = :geographic_longitude, airborne = :airborne WHERE ID = :ID");
            query.SetParameter("ID", entity.ID);
            query.SetParameter("name", entity.name);
            query.SetParameter("country", entity.country);
            query.SetParameter("city", entity.city);
            query.SetParameter("geographic_latitude", entity.geographic_latitude);
            query.SetParameter("geographic_longitude", entity.geographic_longitude);
            query.SetParameter("airborne", entity.airborne);
            query.ExecuteUpdate();
        }

    }
}

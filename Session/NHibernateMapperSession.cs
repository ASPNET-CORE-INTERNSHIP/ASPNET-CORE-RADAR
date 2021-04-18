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



        //RADAR
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

        public void UpdateRadar(Guid ID, Guid transmitter_id, Guid receiver_id, Guid location_id)
        {
            ISQLQuery query = _session.CreateSQLQuery("UPDATE Radar SET transmitter_id = :transmitter_id, receiver_id = :receiver_id, location_id = :location_id WHERE ID = :ID");
            query.SetParameter("ID", ID);
            query.SetParameter("transmitter_id", transmitter_id);
            query.SetParameter("receiver_id", receiver_id);
            query.SetParameter("location_id", location_id);
            query.ExecuteUpdate();
        }

        public async Task DeleteRadar(Guid id)
        {
            ISQLQuery query = _session.CreateSQLQuery("DELETE FROM Radar WHERE ID = :ID");
            query.SetParameter("ID", id);
            query.ExecuteUpdate();
        }

        public List<Guid> GetRadarID()
        {
            String sql = "SELECT ID FROM Radar";
            ISQLQuery query = _session.CreateSQLQuery(sql);
            List<Guid> results = (List<Guid>)query.List<Guid>();
            Console.WriteLine("GetRadarID" + results.Count);
            return results;
        }

        public async Task<string> GetRadarName(int id)
        {
            var name = _session.CreateSQLQuery("SELECT name FROM Radar WHERE ID = :ID").SetParameter("ID", id).UniqueResult<string>();
            return name;
        }

        public async Task<RadarGeneral> GetRadarGeneralInfo(Guid id)
        {
            Radar radar = await GetRadarGeneral(id);
            Transmitter transmitter = await GetTransmitter(radar.transmitter_id);
            Receiver receiver = await GetReceiver(radar.receiver_id);
            Location location = await GetLocation(radar.location_id);

            RadarGeneral radarGeneral = new RadarGeneral(radar, transmitter, receiver, location);

            return radarGeneral;
        }

        public async Task<Radar> GetRadarGeneral(Guid id)
        {
            var name = _session.CreateSQLQuery("SELECT name FROM Radar WHERE ID = :ID").SetParameter("ID", id).UniqueResult<string>();
            var system = _session.CreateSQLQuery("SELECT system FROM Radar WHERE ID = :ID").SetParameter("ID", id).UniqueResult<string>();
            var configuration = _session.CreateSQLQuery("SELECT configuration FROM Radar WHERE ID = :ID").SetParameter("ID", id).UniqueResult<string>();

            var transmitter_id = _session.CreateSQLQuery("SELECT transmitter_id FROM Radar WHERE ID = :ID").SetParameter("ID", id).UniqueResult<Guid>();
            var receiver_id = _session.CreateSQLQuery("SELECT receiver_id FROM Radar WHERE ID = :ID").SetParameter("ID", id).UniqueResult<Guid>();
            var location_id = _session.CreateSQLQuery("SELECT location_id FROM Radar WHERE ID = :ID").SetParameter("ID", id).UniqueResult<Guid>();

            Radar radar = new Radar(name, system, configuration, transmitter_id, receiver_id, location_id);
            return radar;
        }

        public async Task<int> GetRadarNumber()
        {
            var num = _session.CreateSQLQuery("SELECT COUNT(*) FROM Radar").UniqueResult<int>();
            return num;
        }



        //TRANSMITTER
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

        public void UpdateTransmitter(string currentName, string newName, int max_frequency, int min_frequency, int power)
        {
            ISQLQuery query = _session.CreateSQLQuery("UPDATE Transmitter SET name = :newName, max_frequency = :max_frequency, min_frequency = :min_frequency, power = :power WHERE name = :currentName");
            query.SetParameter("newName", newName);
            query.SetParameter("currentName", currentName);
            query.SetParameter("max_frequency", max_frequency);
            query.SetParameter("min_frequency", min_frequency);
            query.SetParameter("power", power);
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

        public async Task<String> SelectTransmitter(Guid ID)
        {
            var transmitter_name = _session.CreateSQLQuery("SELECT name FROM Transmitter WHERE ID = :ID").SetParameter("ID", ID).UniqueResult<string>();
            return transmitter_name;
        }

        public async Task<Guid> GetTransmitterID(Guid id)
        {
            Guid transmitter_id = _session.CreateSQLQuery("SELECT transmitter_id FROM Radar WHERE ID = :ID").UniqueResult<Guid>();
            return transmitter_id;
        }

        public List<object> GetTransmitterName()
        {
            String sql = "SELECT name FROM Transmitter";
            ISQLQuery query = _session.CreateSQLQuery(sql);
            List<object> results = (List<object>)query.List();
            return results;
        }

        public async Task<Transmitter> GetTransmitter(Guid id)
        {
            var name = _session.CreateSQLQuery("SELECT name FROM Transmitter WHERE ID = :ID").SetParameter("ID", id).UniqueResult<string>();
            var modulation_type = _session.CreateSQLQuery("SELECT modulation_type FROM Transmitter WHERE ID = :ID").SetParameter("ID", id).UniqueResult<string>();
            int max_frequency = _session.CreateSQLQuery("SELECT max_frequency FROM Transmitter WHERE ID = :ID").SetParameter("ID", id).UniqueResult<int>();
            int min_frequency = _session.CreateSQLQuery("SELECT min_frequency FROM Transmitter WHERE ID = :ID").SetParameter("ID", id).UniqueResult<int>();
            int power = _session.CreateSQLQuery("SELECT power FROM Transmitter WHERE ID = :ID").SetParameter("ID", id).UniqueResult<int>();

            Transmitter transmitter = new Transmitter(name, modulation_type, max_frequency, min_frequency, power);
            return transmitter;
        }

        public async Task<int> GetTransmitterNumber()
        {
            var num = _session.CreateSQLQuery("SELECT COUNT(*) FROM Transmitter").UniqueResult<int>();
            return num;
        }



        //RECEIVER
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

        public void UpdateReceiver(string currentName, string newName, double listening_time, double rest_time, double recovery_time)
        {
            ISQLQuery query = _session.CreateSQLQuery("UPDATE Receiver SET name = :newName, listening_time = :listening_time, rest_time = :rest_time, recovery_time = :recovery_time WHERE name = :currentName");
            query.SetParameter("newName", newName);
            query.SetParameter("currentName", currentName);
            query.SetParameter("listening_time", listening_time);
            query.SetParameter("rest_time", rest_time);
            query.SetParameter("recovery_time", recovery_time);
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

        public async Task<String> SelectReceiver(Guid ID)
        {
            var transmitter_name = _session.CreateSQLQuery("SELECT name FROM Receiver WHERE ID = :ID").SetParameter("ID", ID).UniqueResult<string>();
            return transmitter_name;
        }

        public async Task<Guid> GetReceiverID(Guid id)
        {
            Guid receiver_id = _session.CreateSQLQuery("SELECT receiver_id FROM Radar WHERE ID = :ID").UniqueResult<Guid>();
            return receiver_id;
        }

        public async Task<Receiver> GetReceiver(Guid id)
        {

            var rec_name = _session.CreateSQLQuery("SELECT name FROM Receiver WHERE ID = :ID").SetParameter("ID", id).UniqueResult<string>();
            double listening_time = _session.CreateSQLQuery("SELECT listening_time FROM Receiver WHERE ID = :ID").SetParameter("ID", id).UniqueResult<double>();
            double rest_time = _session.CreateSQLQuery("SELECT rest_time FROM Receiver WHERE ID = :ID").SetParameter("ID", id).UniqueResult<double>();
            double recovery_time = _session.CreateSQLQuery("SELECT recovery_time FROM Receiver WHERE ID = :ID").SetParameter("ID", id).UniqueResult<double>();

            Receiver receiver = new Receiver(rec_name, listening_time, rest_time, recovery_time);
            return receiver;
        }

        public async Task<int> GetReceiverNumber()
        {
            var num = _session.CreateSQLQuery("SELECT COUNT(*) FROM Receiver").UniqueResult<int>();
            return num;
        }



        //MODE
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



        //SUBMODE
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

        public async Task DeleteSubMode(Guid id)
        {
            ISQLQuery query = _session.CreateSQLQuery("DELETE FROM Submode WHERE ID = :ID");
            query.SetParameter("ID", id);
            query.ExecuteUpdate();
        }



        //SCAN
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

        public async Task DeleteScan(Guid id)
        {
            ISQLQuery query = _session.CreateSQLQuery("DELETE FROM Scan WHERE EXISTS " +
                "(SELECT ID FROM Submode WHERE mode_id IN( SELECT ID FROM Mode " +
                "WHERE radar_id = :ID)); ");
            query.SetParameter("ID", id);
            query.ExecuteUpdate();
        }

        public async Task<Guid> GetScanID(Guid id)
        {
            Guid scan_id = _session.CreateSQLQuery("SELECT scan_id FROM Submode WHERE ID = :ID").SetParameter("ID", id).UniqueResult<Guid>();
            return scan_id;
        }



        //ANTENNA SCAN
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



        //ANTENNA
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



        //LOCATION
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

        public void UpdateLocation(string currentName, string newName, string country, string city, string geographic_latitude, string geographic_longitude, string airborne)
        {
            ISQLQuery query = _session.CreateSQLQuery("UPDATE Location SET name = :newName, country = :country, city = :city, geographic_latitude = :geographic_latitude, geographic_longitude = :geographic_longitude, airborne = :airborne WHERE name = :currentName");
            query.SetParameter("newName", newName);
            query.SetParameter("currentName", currentName);
            query.SetParameter("country", country);
            query.SetParameter("city", city);
            query.SetParameter("geographic_latitude", geographic_latitude);
            query.SetParameter("geographic_longitude", geographic_longitude);
            query.SetParameter("airborne", airborne);
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

        public async Task<Location> GetLocation(Guid id)
        {
            var def_name = _session.CreateSQLQuery("SELECT name FROM Location WHERE ID = :ID").SetParameter("ID", id).UniqueResult<string>();
            var country = _session.CreateSQLQuery("SELECT country FROM Location WHERE ID = :ID").SetParameter("ID", id).UniqueResult<string>();
            var city = _session.CreateSQLQuery("SELECT city FROM Location WHERE ID = :ID").SetParameter("ID", id).UniqueResult<string>();
            var geographic_latitude = _session.CreateSQLQuery("SELECT geographic_latitude FROM Location WHERE ID = :ID").SetParameter("ID", id).UniqueResult<string>();
            var geographic_longitude = _session.CreateSQLQuery("SELECT geographic_longitude FROM Location WHERE ID = :ID").SetParameter("ID", id).UniqueResult<string>();
            var airborne = _session.CreateSQLQuery("SELECT airborne FROM Location WHERE ID = :ID").SetParameter("ID", id).UniqueResult<string>();


            Location location = new Location(def_name, country, city, geographic_latitude, geographic_longitude, airborne);
            return location;
        }  
    }
}

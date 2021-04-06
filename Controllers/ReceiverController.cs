using ASPNETAOP.Models;
using ASPNETAOP.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace ASPNETAOP.Controllers
{
    public class ReceiverController : Controller
    {
       
        private IConfiguration _configuration;
        public ReceiverController(IConfiguration Configuration) { _configuration = Configuration; }
       

        /*private readonly NHibernateMapperSession _session;

        public ReceiverController(NHibernateMapperSession session)
        {
            _session = session;
        }*/

        [Route("Home/Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewReceiver()
        {
            return View();
        }


        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> NewReceiverAsync(Receiver receiver)
        {
            //If the receiver name is null we give a default name that specifies its number
            String rec_name = null;
            if (String.IsNullOrEmpty(receiver.name))
            {
                string stmt = "SELECT COUNT(*) FROM Receiver";
                int count = 0;

                using (SqlConnection thisConnection = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
                {
                    using (SqlCommand cmdCount = new SqlCommand(stmt, thisConnection))
                    {
                        thisConnection.Open();
                        count = (int)cmdCount.ExecuteScalar();
                    }
                }
                count = count + 1;
                rec_name = "Receiver " + count;
            }
            else
            {
                rec_name = receiver.name;
            }

            Guid key = Guid.NewGuid();
            //await _session.Save(book);
            /*Receiver r = new Receiver(key, rec_name, receiver.listening_time, receiver.rest_time, receiver.recovery_time);
            Datas.Receiver = r;
            Datas.Transmitter = new Transmitter();
            Datas.Submode = new Submode();
            Datas.Scan = new Scan();
            Datas.Radar = new Radar();

            Datas.newProgram = "yes";
            await _session.Save(r);*/
             using (SqlConnection con = new SqlConnection(@"Server=localhost;Database=RADAR;Trusted_Connection=True;MultipleActiveResultSets=true"))
             {
                 using (SqlCommand cmd = new SqlCommand())
                 {
                     cmd.Connection = con;
                     cmd.CommandType = CommandType.Text;
                     cmd.CommandText = @"INSERT INTO Receiver(ID, name, listening_time, rest_time, recovery_time) 
                             VALUES(@ID, @name, @listening_time, @rest_time, @recovery_time)";
                     cmd.Parameters.AddWithValue("@ID", key);
                     cmd.Parameters.AddWithValue("@name", rec_name);
                     cmd.Parameters.AddWithValue("@listening_time", receiver.listening_time);
                     cmd.Parameters.AddWithValue("@rest_time", receiver.rest_time);
                     cmd.Parameters.AddWithValue("@recovery_time", receiver.recovery_time);

                     Receiver r = new Receiver(key, rec_name, receiver.listening_time, receiver.rest_time, receiver.recovery_time);
                     Datas.Receiver = r;
                     Datas.Transmitter = new Transmitter();
                     Datas.Submode = new Submode();
                     Datas.Scan = new Scan();
                     Datas.Radar = new Radar();

                     Datas.newProgram = "yes";

                     try
                     {
                         con.Open();
                         int i = cmd.ExecuteNonQuery();
                         if (i != 0)
                             ViewData["Message"] = "New Receiver added";
                         con.Close();
                         if (ModelState.IsValid)
                         {
                             return RedirectToAction("NewAntenna", "Antenna");
                         }
                     }
                     catch (SqlException e)
                     {
                         ViewData["Message"] = e.Message.ToString() + " Error";
                     }

                 }
             }
            return View(receiver);
            
        }
    }
}

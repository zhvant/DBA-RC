using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.IO;
using DbaReports.Models;


namespace DbaReports.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {


        //[HttpGet("{Server}")]
        public IEnumerable<Session> Get(string Server)
        {
            var connectionStringActivity = $"Server={Server};Initial Catalog=msdb;Integrated Security=True";

            //string connectionString = "Server=localhost;Initial Catalog=AppDbContextTest;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionStringActivity))
            {

                var Sessions = new List<Session>(); // Инициализация списка сессий
                connection.Open();
                string sqlExpression = "sp_ss";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        var session = new Session(); //создаем объект сессии и заполняем его поля
                        session.Duration = reader.GetValue(0).ToString();
                        session.DatabaseName = reader.GetValue(1).ToString();
                        session.SessionId = reader.GetValue(2).ToString();
                        session.HostName = reader.GetValue(3).ToString();
                        session.LoginName = reader.GetValue(4).ToString();
                        session.BlockingSessionId = reader.GetValue(5).ToString();
                        session.BlockedSessionCount = reader.GetValue(6).ToString();
                        session.WaitInfo = reader.GetValue(7).ToString();
                        session.ProgramName = reader.GetValue(8).ToString();
                        session.CPU = reader.GetValue(9).ToString();
                        session.Reads = reader.GetValue(10).ToString();
                        session.PhysicalReads = reader.GetValue(11).ToString();
                        session.Writes = reader.GetValue(12).ToString();
                        session.UsedMemory = reader.GetValue(13).ToString();
                        session.SqlCommand = reader.GetValue(14).ToString();
                        session.SqlText = reader.GetValue(15).ToString();
                        session.QueryPlan = reader.GetValue(16).ToString();
                        //session.SqlCommand = "Test SqlCommand!";
                        //session.SqlText = "Test SqlText!";
                        //session.QueryPlan = "Test QueryPlan!";
                        session.Status = reader.GetValue(17).ToString();
                        session.PercentComplete = reader.GetValue(18).ToString();
                        Sessions.Add(session); // Добавление сессии в список

                        //Убрать лишние теги в начале и конце запроса
                        try
                        {
                            session.SqlCommand = session.SqlCommand.Substring(10, session.SqlCommand.Length - 14);
                        }
                        catch {;};
                        try
                        { 
                            session.SqlText = session.SqlText.Substring(10, session.SqlText.Length - 14);
                        } catch {;}
                        
                    }
                    reader.Close();
                }
                return Sessions;
            }
        }
    }


    [Route("[controller]")]
    public class SessionsController : Controller
    {
        
        static List<Session> sessions = new List<Session>();
        

        //[HttpGet("{Server}")]
        public IActionResult Sessions(string Server)
        {
            sessions.Clear();
            //var connectionStringActivity = $"Server={Server};Initial Catalog=msdb;Integrated Security=True";

            ////string connectionString = "Server=localhost;Initial Catalog=AppDbContextTest;Integrated Security=True";
            //SqlConnection connection = new SqlConnection(connectionStringActivity);
            //    //var Sessions = new List<Session>(); // Инициализация списка сессий
            //    connection.Open();
            //    string sqlExpression = "sp_ss";
            //    SqlCommand command = new SqlCommand(sqlExpression, connection);
            //    SqlDataReader reader = command.ExecuteReader();
            //    if (reader.HasRows) // если есть данные
            //    {
            //        while (reader.Read()) // построчно считываем данные
            //        {
            //            var session = new Session(); //создаем объект сессии и заполняем его поля
            //            session.Duration = reader.GetValue(0).ToString();
            //            session.DatabaseName = reader.GetValue(1).ToString();
            //            session.SessionId = reader.GetValue(2).ToString();
            //            session.HostName = reader.GetValue(3).ToString();
            //            session.LoginName = reader.GetValue(4).ToString();
            //            session.BlockingSessionId = reader.GetValue(5).ToString();
            //            session.BlockedSessionCount = reader.GetValue(6).ToString();
            //            session.WaitInfo = reader.GetValue(7).ToString();
            //            session.ProgramName = reader.GetValue(8).ToString();
            //            session.CPU = reader.GetValue(9).ToString();
            //            session.Reads = reader.GetValue(10).ToString();
            //            session.PhysicalReads = reader.GetValue(11).ToString();
            //            session.Writes = reader.GetValue(12).ToString();
            //            session.SqlCommand = reader.GetValue(13).ToString();
            //            session.SqlText = reader.GetValue(14).ToString();
            //            session.QueryPlan = reader.GetValue(15).ToString();
            //            session.Status = reader.GetValue(16).ToString();
            //            session.PercentComplete = reader.GetValue(17).ToString();
            //            sessions.Add(session); // Добавление сессии в список

            //            //Убрать лишние теги в начале и конце запроса
            //            session.SqlCommand = session.SqlCommand.Substring(10, session.SqlCommand.Length - 14);
            //            session.SqlText = session.SqlText.Substring(10, session.SqlText.Length - 14);
            //        }
            //        reader.Close();
            //    }
                return View(sessions);            
        }

        //public ActionResult ShowSqlCommand(String SessionId)
        //{

        //    Session s = sessions.FirstOrDefault(ses => ses.SessionId == SessionId);
        //    if (s != null)
        //        return PartialView(s);
        //    return HttpNotFound();
        //}

        //private ActionResult HttpNotFound()
        //{
        //    throw new NotImplementedException();
        //}
    }
}

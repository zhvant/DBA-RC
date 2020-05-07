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

    //[ApiController]
    [Route("[controller]")]
    public class KillController : ControllerBase
    {

        //[HttpGet("{Server}"), {Sid}]
        public IActionResult Get(string Server, string Sid)
        {
            var connectionString = $"Server={Server};Initial Catalog=msdb; Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlExpression = $"kill {Sid}";
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.ExecuteNonQuery();
            }
            return Redirect($"~/sessions?server={Server}");
        }
    }
}

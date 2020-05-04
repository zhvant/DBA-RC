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
using WebApplication8.Models;


namespace WebApplication8.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class KillController : ControllerBase
    {

        //[HttpGet("{Server}"), {Sid}]
        public void Get(string Server, string Sid)
        {
            var connectionString = $"Server={Server};Initial Catalog=msdb; Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlExpression = $"kill {Sid}";
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.ExecuteNonQuery();
            }                         
        }
    }
}

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
    public class DatabaseController : ControllerBase
    {


        //[HttpGet("{Server}")]
        public IEnumerable<Database> Get(string Server)
        {
            var connectionString = $"Server={Server};Initial Catalog=msdb;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var Databases = connection.Query<Database>("select Name from sys.databases");
                return Databases;
            }
        }
    }
}

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


    [Route("[controller]")]
    public class InstanceErrorsController : Controller
    {


        //[HttpGet("{Server}")]
        public IActionResult InstanceErrors(string Server)
        {
            var connectionStringInstanceError = $"Server={Server}; Initial Catalog=msdb;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionStringInstanceError))
            {
                var InstanceErrors = connection.Query<InstanceError>("sp_GetInstanceErrors");
                return View(InstanceErrors);
            }
        }
    }


    [ApiController]
    [Route("api/[controller]")]
    public class InstanceErrorController : ControllerBase
    {


        //[HttpGet("{Server}")]
        public IEnumerable<InstanceError> Get(string Server)
        {
            var connectionStringInstanceError = $"Server={Server}; Initial Catalog=msdb;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionStringInstanceError))
            {      
                var InstanceErrors = connection.Query<InstanceError>("sp_GetInstanceErrors");
                return InstanceErrors;
            }
        }
    }
}

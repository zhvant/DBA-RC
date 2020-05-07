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
using Microsoft.AspNetCore.Mvc.Filters;
//using System.Web.Http;

namespace DbaReports.Controllers
{
    //[ApiController]
    [Route("[controller]")]
    public class BackupsController : Controller //ControllerBase
    {
        //[HttpGet("{Server}")]
        public IActionResult Backups(string Server)
        {
            var connectionStringBackup = $"Server={Server};Initial Catalog=msdb;Integrated Security=True";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStringBackup))
                {
                    var Backups = connection.Query<Backup>("sp_ShowBackups");
                    return View(Backups);
                    //return Backups;
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("_Error");
            }
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class BackupController : ControllerBase 
    {
        //[HttpGet("{Server}")]
        public IEnumerable<Backup> Get(string Server)
        {
            var connectionStringBackup = $"Server={Server};Initial Catalog=msdb;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionStringBackup))
            {
                var Backups = connection.Query<Backup>("sp_ShowBackups");
                return Backups;
            }
        }
    }
}

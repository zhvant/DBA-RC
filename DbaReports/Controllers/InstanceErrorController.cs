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
using DbaRC.Models;


namespace DbaRC.Controllers
{
    public static class InstanceErrorsSql
    {
        public static string SqlText =
            @"
            begin tran 
                declare @t table (
                LogDate datetime, 
                ProcessInfo varchar(50), 
                Text nvarchar(max)
              ) 
              insert into @t exec master.dbo.xp_readerrorlog  
              insert into @t exec master.dbo.xp_readerrorlog @p1 = 1  
              
              select 
                @@servername ServerName, 
                * 
              from 
                @t 
              where 
                1 = 1 
                and Text not like '%SSPI%' 
                and Text not like '%0 errors%' 
                and Text not in (
                  'Error: 18452, Severity: 14, State: 1.', 
                  'Error: 17806, Severity: 20, State: 14.', 
                  'FILESTREAM: effective level = 0, configured level = 0, file system access share name = ''MSSQLSERVER''.', 
                  'Configuration option ''user options'' changed from 0 to 0. Run the RECONFIGURE statement to install.'
                ) 
                and Text not like 'Log was backed up.%' 
                and Text not like 'Database backed up.%' 
                and Text not like 'Database differential changes were backed up.%' 
                and Text not like 'This instance of SQL Server has been using a process ID%' 
                and Text not like '%This is an informational message only%' 
                and LogDate > getdate()-5 
              order by 
                LogDate desc 
            commit
            ";
    }

    [Route("[controller]")]
    public class InstanceErrorsController : Controller
    {
        private readonly SettingsContext db;
        public InstanceErrorsController(SettingsContext context) => db = context;

        //[HttpGet("{Server}")]
        public IActionResult InstanceErrors(string Server)
        {
            var connectionString = db.Setting.FirstOrDefault(m => m.ServerName == Server).ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var InstanceErrors = connection.Query<InstanceError>(InstanceErrorsSql.SqlText);
                    return View(InstanceErrors);
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
    public class InstanceErrorController : ControllerBase
    {
        private readonly SettingsContext db;
        public InstanceErrorController(SettingsContext context) => db = context;

        //[HttpGet("{Server}")]
        public IEnumerable<InstanceError> Get(string Server)
        {
            var connectionString = db.Setting.FirstOrDefault(m => m.ServerName == Server).ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {      
                var InstanceErrors = connection.Query<InstanceError>(InstanceErrorsSql.SqlText);
                return InstanceErrors;
            }
        }
    }
}

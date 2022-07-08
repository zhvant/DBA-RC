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
    public static class AgentJobSql
    {
        public static string SqlText = @"
  SELECT 
  	@@SERVERNAME AS ServerName
  	,sj.name AS JobName 
  	,convert(varchar(10),cast(cast(JH.run_date as nvarchar(8)) as date),104)  AS StartDate
  	,STUFF(STUFF(RIGHT('000000' + CAST(MAX(JH.run_time) AS VARCHAR(6)), 6), 5, 0, ':'), 3, 0, ':') AS StartTime
  	,STUFF(STUFF(RIGHT('000000' + CAST(MAX(JH.run_duration) AS VARCHAR(6)), 6), 5, 0, ':'), 3, 0, ':') AS Duration
    ,JH.message 
  FROM msdb.dbo.sysjobhistory AS JH
  FULL OUTER JOIN msdb.dbo.sysjobs AS sj ON JH.job_id = sj.job_id
  WHERE (JH.run_time IS NOT NULL)
  and
  run_status=0 --Ошибка
  and 
  sj.name not in ('system - mail test','UTP_SignAgreementRole','job_ParseNotificationEF','DBA_session_monitor.data_collection','DBA_session_monitor.clear_table') 
  and
  JH.run_date>(SELECT CONVERT(VARCHAR(10), getdate()-7 --Difference in days from current date: 1=today, 2=yesterday, etc.    
  , 112)) -- date format
  and
  step_id>0 --job steps only
  GROUP BY sj.name
  	,JH.run_date
  	,JH.run_time
  	,JH.run_status
  	,JH.job_id
  	,JH.message
 order by JH.run_date desc , 4 desc";                 
    }



    //[ApiController]
    [Route("[controller]")]
    public class AgentJobsController  : Controller
    {
        private readonly SettingsContext db;
        public AgentJobsController(SettingsContext context) => db = context;
        public IActionResult AgentJobs(string Server)
        {
            var connectionString = db.Setting.FirstOrDefault(m => m.ServerName == Server).ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {                  
                    var AgentJobs = connection.Query<AgentJob>(AgentJobSql.SqlText);
                    return View(AgentJobs);
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
    public class AgentJobController : ControllerBase
    {
        private readonly SettingsContext db;
        public AgentJobController(SettingsContext context) => db = context;
        public IEnumerable<AgentJob> Get(string Server)
        {
            var connectionString = db.Setting.FirstOrDefault(m => m.ServerName == Server).ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {              
                var AgentJobs = connection.Query<AgentJob>(AgentJobSql.SqlText);
                return AgentJobs;
            }
        }
    }
}

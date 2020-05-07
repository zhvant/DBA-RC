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
    public class AgentJobsController  : Controller
    {


        //[HttpGet("{Server}")]
        public IActionResult AgentJobs(string Server)
        {
            var connectionStringAgentJob = $"Server={Server}; Initial Catalog=msdb; Integrated Security=True";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStringAgentJob))
                {
                    string SqlText = @"
                                   SELECT 
                               	@@SERVERNAME AS ServerName
                               	,sj.NAME AS Job 
                               	,JH.run_date AS StartDate
                               	,STUFF(STUFF(RIGHT('000000' + CAST(MAX(JH.run_time) AS VARCHAR(6)), 6), 5, 0, ':'), 3, 0, ':') AS StartTime
                               	,STUFF(STUFF(RIGHT('000000' + CAST(MAX(JH.run_duration) AS VARCHAR(6)), 6), 5, 0, ':'), 3, 0, ':') AS Duration
                                   ,jh.Message 
                               FROM dbo.sysjobhistory AS JH
                               FULL OUTER JOIN dbo.sysjobs AS sj ON JH.job_id = sj.job_id
                               WHERE (JH.run_time IS NOT NULL)
                               and
                               run_status=0 --Ошибка
                               and 
                               sj.NAME not in ('system - mail test','UTP_SignAgreementRole','job_ParseNotificationEF','DBA_session_monitor.data_collection','DBA_session_monitor.clear_table') --Исключить спам джобы
                               and
                               JH.run_date>(SELECT CONVERT(VARCHAR(10), getdate()-7 --Разница в днях от текущей даты: 1=сегодня, 2=вчера итд.     
                               , 112)) -- Формат даты
                               and
                               step_id>0 --Только шаги джоба
                               GROUP BY sj.NAME
                               	,JH.run_date
                               	,JH.run_time
                               	,JH.run_status
                               	,JH.job_id
                               	,jh.message";
                    var AgentJobs = connection.Query<AgentJob>(SqlText);
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


        //[HttpGet("{Server}")]
        public IEnumerable<AgentJob> Get(string Server)
        {
            var connectionStringAgentJob = $"Server={Server}; Initial Catalog=msdb; Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionStringAgentJob))
            {
                string SqlText = @"
                                   SELECT 
                               	@@SERVERNAME AS ServerName
                               	,sj.NAME AS Job 
                               	,JH.run_date AS StartDate
                               	,STUFF(STUFF(RIGHT('000000' + CAST(MAX(JH.run_time) AS VARCHAR(6)), 6), 5, 0, ':'), 3, 0, ':') AS StartTime
                               	,STUFF(STUFF(RIGHT('000000' + CAST(MAX(JH.run_duration) AS VARCHAR(6)), 6), 5, 0, ':'), 3, 0, ':') AS Duration
                                   ,jh.Message 
                               FROM dbo.sysjobhistory AS JH
                               FULL OUTER JOIN dbo.sysjobs AS sj ON JH.job_id = sj.job_id
                               WHERE (JH.run_time IS NOT NULL)
                               and
                               run_status=0 --Ошибка
                               and 
                               sj.NAME not in ('system - mail test','UTP_SignAgreementRole','job_ParseNotificationEF','DBA_session_monitor.data_collection','DBA_session_monitor.clear_table') --Исключить спам джобы
                               and
                               JH.run_date>(SELECT CONVERT(VARCHAR(10), getdate()-7 --Разница в днях от текущей даты: 1=сегодня, 2=вчера итд.     
                               , 112)) -- Формат даты
                               and
                               step_id>0 --Только шаги джоба
                               GROUP BY sj.NAME
                               	,JH.run_date
                               	,JH.run_time
                               	,JH.run_status
                               	,JH.job_id
                               	,jh.message"; 
                var AgentJobs = connection.Query<AgentJob>(SqlText);
                return AgentJobs;
            }
        }
    }
}

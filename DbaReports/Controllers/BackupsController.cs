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
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace DbaRC.Controllers
{
    //[ApiController]
    [Route("[controller]")]
    public class BackupsController : Controller //ControllerBase
    {
        private readonly SettingsContext db;
        public BackupsController(SettingsContext context) => db = context;


        //[HttpGet("{Server}")]      
        public IActionResult Backups(string Server)
        {
            
            var connectionString = db.Setting.FirstOrDefault(m => m.ServerName == Server).ConnectionString;
            //var connectionStringBackup = $"Server={Server};Initial Catalog=msdb;Integrated Security=True";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var Backups = connection.Query<Backup>(BackupsSql.SqlText);
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
        private readonly SettingsContext db;
        public BackupController(SettingsContext context) => db = context;

        //[HttpGet("{Server}")]
        public IEnumerable<Backup> Get(string Server)
        {
            var connectionString = db.Setting.FirstOrDefault(m => m.ServerName == Server).ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var Backups = connection.Query<Backup>(BackupsSql.SqlText);
                return Backups;
            }
        }
    }

    public static class BackupsSql
    {
        public static string SqlText =
                @"
                select 
                  allbakdate.DBName, 
                  DateOfLastBackup, 
                  allbak.backup_type, 
                  Physical_Device_name 
                from 
                  (
                    SELECT 
                      @@Servername AS ServerName, 
                      d.Name AS DBName, 
                      b.Backup_finish_date, 
                      CASE b.[type] WHEN 'D' THEN 'FULL' WHEN 'I' THEN 'DIFF' WHEN 'L' THEN 'TRAN' END as backup_type, 
                      bmf.Physical_Device_name 
                    FROM 
                      sys.databases d 
                      LEFT JOIN msdb..backupset b ON b.database_name = d.name 
                      AND b.[type] in ('D', 'I', 'L') 
                      LEFT JOIN msdb.dbo.backupmediafamily bmf ON b.media_set_id = bmf.media_set_id
                  ) allbak 
                  join (
                    select 
                      DBName, 
                      [backup_type], 
                      max(Backup_finish_date) as 'DateOfLastBackup' 
                    from 
                      (
                        SELECT 
                          @@Servername AS ServerName, 
                          d.Name AS DBName, 
                          b.Backup_finish_date, 
                          CASE b.[type] WHEN 'D' THEN 'FULL' WHEN 'I' THEN 'DIFF' WHEN 'L' THEN 'TRAN' END as backup_type, 
                          bmf.Physical_Device_name 
                        FROM 
                          sys.databases d 
                          LEFT JOIN msdb..backupset b ON b.database_name = d.name 
                          AND b.[type] in ('D', 'I', 'L') 
                          LEFT JOIN msdb.dbo.backupmediafamily bmf ON b.media_set_id = bmf.media_set_id
                      ) allbak 
                    group by 
                      DBName, 
                      [backup_type]
                  ) allbakdate on allbakdate.DateOfLastBackup = allbak.Backup_finish_date 
                where 
                  datediff(
                    day, 
                    DateOfLastBackup, 
                    getdate()
                  )< 90 
                order by 
                  1, 
                  2
                ";
    }
}

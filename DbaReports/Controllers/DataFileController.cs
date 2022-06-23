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

    //[ApiController]
    [Route("[controller]")]
    public class DataFilesController : Controller
    {
        private readonly SettingsContext db;
        public DataFilesController(SettingsContext context) => db = context;

        //[HttpGet("{Server}/{Database}")]
        public IActionResult DataFiles(string Server, string Database)
        {
            var connectionString = db.Setting.FirstOrDefault(m => m.ServerName == Server).ConnectionString;
            var builder = new SqlConnectionStringBuilder(connectionString);
            builder.InitialCatalog = Database;
            connectionString = builder.ConnectionString;
            //var connectionString = $"Server={Server};Initial Catalog={Database};Integrated Security=True";

            var Sql = @"
        SET NOCOUNT ON  
        
        declare @drives table
        (drive char(1)
        , MBFree bigint
        )
        
        
        declare @filegroups table
        (filegroupname nvarchar(255)
        , size bigint
        , max_size bigint
        , AvailablePages bigint
        , physical_name nvarchar(255)
        , drive char(1))
        
        
        declare @FilesReport table
        (DB nvarchar(255)
        , FileGroupName nvarchar(255)
        , Drive char(1)
        , FileName nvarchar(255)
        , SizeMB bigint
        , MaxSizeMB  bigint
        , AvailableMB bigint)
        
        
        
        declare @FilegroupsReport table
        (DB nvarchar(255)
        , FileGroupName nvarchar(255)
        , AvailableMB bigint
        , AvailableGB int)
        
        --Свободное место на дисках
        insert into @drives
        execute xp_fixeddrives
        
        ---------------------------- Отчет по группам------------------------------------------ -
        
        insert into @filegroups
        SELECT
        case  
        when ds.name is null then 'TRANSACTION_LOG'
        else ds.name
        end as filegroupname
        ,size
        ,case when growth = 0 then size else max_size end as max_size
        ,case   
        when max_size<>-1 then max_size-size
        when max_size = -1 and growth = 0 then 0
        else max_size
        end as 'AvailablePages'
        ,physical_name
        ,upper(substring(physical_name, 0, 2)) AS 'drive'
        FROM sys.database_files df LEFT OUTER JOIN sys.data_spaces ds
        ON df.data_space_id = ds.data_space_id
        order by 1
        
        
        insert into @FilesReport
        SELECT DB_NAME() as DB,FileGroupName,  
        f.Drive,  
        physical_name as FileName,  
        size * 8 / 1024 as SizeMB,   
        max_size * 8 / 1024 as MaxSizeMB,  
        case  
        when AvailablePages = -1 then MBFree
        when AvailablePages*8 / 1024 > MBFree then MBFree
        else AvailablePages * 8 / 1024
        end as AvailableMB
        FROM @filegroups f join @drives d on f.drive = d.drive
        
        
        --Отчет по группам
        insert into @FilegroupsReport
        SELECT
        DB
        , FileGroupName
        ,sum(AvailableMB) AvailableMB
        ,sum(AvailableMB) / 1024 AvailableGB
          FROM @FilesReport
          group by DB, FileGroupName
        
        
        
        select 
        FileGroupName		
        ,FileName as Name	
        ,SizeMB	as UsedSpace
        ,MaxSizeMB	as MaxSpace
        ,AvailableMB as AvailableSpace
        from @FilesReport";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var DataFiles = connection.Query<DataFile>($"{Sql}");
                return View(DataFiles);
            }
        }
    }





    [ApiController]
    [Route("api/[controller]")]
    public class DataFileController : ControllerBase
    {
        private readonly SettingsContext db;
        public DataFileController(SettingsContext context) => db = context;

        //[HttpGet("{Server}/{Database}")]
        public IEnumerable<DataFile> Get(string Server, string Database)
        {
            var connectionString = db.Setting.FirstOrDefault(m => m.ServerName == Server).ConnectionString;
            var builder = new SqlConnectionStringBuilder(connectionString);
            builder.InitialCatalog = Database;
            connectionString = builder.ConnectionString;
       //var connectionString = $"Server={Server};Initial Catalog={Database};Integrated Security=True";

       var Sql = @"
        SET NOCOUNT ON  
        
        declare @drives table
        (drive char(1)
        , MBFree bigint
        )
        
        
        declare @filegroups table
        (filegroupname nvarchar(255)
        , size bigint
        , max_size bigint
        , AvailablePages bigint
        , physical_name nvarchar(255)
        , drive char(1))
        
        
        declare @FilesReport table
        (DB nvarchar(255)
        , FileGroupName nvarchar(255)
        , Drive char(1)
        , FileName nvarchar(255)
        , SizeMB bigint
        , MaxSizeMB  bigint
        , AvailableMB bigint)
        
        
        
        declare @FilegroupsReport table
        (DB nvarchar(255)
        , FileGroupName nvarchar(255)
        , AvailableMB bigint
        , AvailableGB int)
        
        --Free space on disks
        insert into @drives
        execute xp_fixeddrives
        
        ---------------------------- Report by file groups ------------------------------------------ -
        
        insert into @filegroups
        SELECT
        case  
        when ds.name is null then 'TRANSACTION_LOG'
        else ds.name
        end as filegroupname
        ,size
        ,case when growth = 0 then size else max_size end as max_size
        ,case   
        when max_size<>-1 then max_size-size
        when max_size = -1 and growth = 0 then 0
        else max_size
        end as 'AvailablePages'
        ,physical_name
        ,upper(substring(physical_name, 0, 2)) AS 'drive'
        FROM sys.database_files df LEFT OUTER JOIN sys.data_spaces ds
        ON df.data_space_id = ds.data_space_id
        order by 1
        
        
        insert into @FilesReport
        SELECT DB_NAME() as DB,FileGroupName,  
        f.Drive,  
        physical_name as FileName,  
        size * 8 / 1024 as SizeMB,   
        max_size * 8 / 1024 as MaxSizeMB,  
        case  
        when AvailablePages = -1 then MBFree
        when AvailablePages*8 / 1024 > MBFree then MBFree
        else AvailablePages * 8 / 1024
        end as AvailableMB
        FROM @filegroups f join @drives d on f.drive = d.drive
        
        
        -- Report by file groups
        insert into @FilegroupsReport
        SELECT
        DB
        , FileGroupName
        ,sum(AvailableMB) AvailableMB
        ,sum(AvailableMB) / 1024 AvailableGB
          FROM @FilesReport
          group by DB, FileGroupName
        
        
        
        select 
        FileGroupName		
        ,FileName as Name	
        ,SizeMB	as UsedSpace
        ,MaxSizeMB	as MaxSpace
        ,AvailableMB as AvailableSpace
        from @FilesReport";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var DataFiles = connection.Query<DataFile>($"{Sql}");
                return DataFiles;
            }
        }
    }
}

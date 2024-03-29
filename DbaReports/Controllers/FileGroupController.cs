﻿using System;
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
    class SqlScript
    {
        public string Text = @"
        SET NOCOUNT ON  
        
        declare @drives table
        (drive char(1)
        , MBFree bigint
        )
        
        declare @platform varchar(10) = 'Windows'
        if  @@VERSION like '%Linux%' set @platform = 'Linux' 

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
        if @platform='Linux' update @drives set drive = '/'

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
        SELECT DB_NAME() as DB,filegroupname,  
        f.drive,  
        physical_name as FileName,  
        size * 8 / 1024 as SizeMB,   
        max_size * 8 / 1024 as MaxSizeMB,  
        case  
        when AvailablePages = -1 then MBFree
        when AvailablePages*8 / 1024 > MBFree then MBFree
        else AvailablePages * 8 / 1024
        end as AvailableMB
        FROM @filegroups f join @drives d on f.drive = d.drive
        
		update @FilesReport set AvailableMB=MaxSizeMB-SizeMB
		where AvailableMB>MaxSizeMB
		and FileGroupName='TRANSACTION_LOG'
        
        insert into @FilegroupsReport
		SELECT DB
        ,FileGroupName
		,sum(AvailableMB)
		,sum(AvailableGB)
		FROM (
        SELECT
        DB
        ,FileGroupName
        ,case  
        when sum(AvailableMB)> MBFree then MBFree 
		else sum(AvailableMB)
		end AvailableMB
        ,case  
        when sum(AvailableMB)> MBFree then MBFree/1024 
		else sum(AvailableMB)/1024
		end AvailableGB
          FROM @FilesReport f join @drives d on f.drive = d.drive
          group by DB, FileGroupName, f.drive,MBFree
		  ) t
        group by DB, FileGroupName
                

		select FileGroupName, AvailableMB as AvailableSpace from @FilegroupsReport";
    }


    [Route("[controller]")]
    public class FileGroupsController : Controller
    {
        private readonly SettingsContext db;
        public FileGroupsController(SettingsContext context) => db = context;
        //[HttpGet("{Server}")]
        public IActionResult FileGroups(string Server, string Database)
        {
            var connectionString = db.Setting.FirstOrDefault(m => m.ServerName == Server).ConnectionString;
            var builder = new SqlConnectionStringBuilder(connectionString);
            builder.InitialCatalog = Database;
            connectionString = builder.ConnectionString;
            //var connectionString = $"Server={Server};Initial Catalog={Database};Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var FileGroupsAll = new List<FileGroup>();
                var Sql = new SqlScript();
                if (Database is null)
                {
                    var Databases = connection.Query<Database>($"select Name from sys.databases");
                    foreach (var database in Databases)
                    {
                        var connectionStringDatabases = db.Setting.FirstOrDefault(m => m.ServerName == Server).ConnectionString;
                        var builderDatabases = new SqlConnectionStringBuilder(connectionString);
                        builderDatabases.InitialCatalog = database.Name;
                        connectionString = builderDatabases.ConnectionString;
                        //var connectionStringDatabases = $"Server={Server};Initial Catalog={database.Name};Integrated Security=True";

                        using (SqlConnection connection2 = new SqlConnection(connectionStringDatabases))
                        {
                            var fileGroups = connection2.Query<FileGroup>($"{Sql.Text}");
                            foreach (var fg in fileGroups)
                            {
                                FileGroupsAll.Add(fg);
                            }
                        }
                    }
                    return View(FileGroupsAll);
                }
                else
                {
                    var FileGroups = connection.Query<FileGroup>($"{Sql.Text}");
                    return View(FileGroups);
                }

            }

        }
    }


    [ApiController]
    [Route("api/[controller]")]
    public class FileGroupController : ControllerBase
    {
        private readonly SettingsContext db;
        public FileGroupController(SettingsContext context) => db = context;
        //[HttpGet("{Server}")]
        public IEnumerable<FileGroup> Get(string Server, string Database)
        {
            var connectionString = db.Setting.FirstOrDefault(m => m.ServerName == Server).ConnectionString;
            var builder = new SqlConnectionStringBuilder(connectionString);
            builder.InitialCatalog = Database;
            connectionString = builder.ConnectionString;
            //var connectionString = $"Server={Server};Initial Catalog={Database};Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var FileGroupsAll = new List<FileGroup>();
                var Sql = new SqlScript();
                if (Database is null)
                {
                    var Databases = connection.Query<Database>($"select Name from sys.databases");
                    foreach (var database in Databases)
                    {
                        var connectionStringDatabases = db.Setting.FirstOrDefault(m => m.ServerName == Server).ConnectionString;
                        var builderDatabases = new SqlConnectionStringBuilder(connectionString);
                        builderDatabases.InitialCatalog = database.Name;
                        connectionString = builderDatabases.ConnectionString;
                        //var connectionStringDatabases = $"Server={Server};Initial Catalog={database.Name};Integrated Security=True";

                        using (SqlConnection connection2 = new SqlConnection(connectionStringDatabases))
                        {
                            var fileGroups = connection2.Query<FileGroup>($"{Sql.Text}");
                            foreach (var fg in fileGroups)
                            {
                                FileGroupsAll.Add(fg);
                            }
                        }
                    }
                    return FileGroupsAll;
                }
                else
                {
                    var FileGroups = connection.Query<FileGroup>($"{Sql.Text}");
                    return FileGroups;
                }

            }
            
        }
    }
}

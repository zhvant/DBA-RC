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
using DbaReports.Models;


namespace DbaReports.Controllers
{
    //[ApiController]
    [Route("[controller]")]
    public class DatabasesController : Controller
    {


        //[HttpGet("{Server}")]
        public IActionResult Databases(string Server)
        {
            var connectionString = $"Server={Server};Initial Catalog=msdb;Integrated Security=True";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var Databases = connection.Query<Database>("select Name from sys.databases");
                    ViewBag.Server = Server;
                    return View(Databases);
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

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

namespace DbaReportsWebApi.Controllers
{
    [Route("[controller]")]
    public class StorageController : Controller
    {
        public IActionResult Storage()
        {
            return View();
        }
    }
}
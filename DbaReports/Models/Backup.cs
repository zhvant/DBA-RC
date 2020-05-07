using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace DbaReports.Models
{
    public class Backup
    {
        public string DBName { get; set; }
        public string DateOfLastBackup { get; set; }
        public string backup_type { get; set; }
        public string Physical_Device_name { get; set; }

    }
}

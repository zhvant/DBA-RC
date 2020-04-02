using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplication8.Models
{
    public class Backup
    {
        public string DBName { get; set; }
        public string DateOfLastBackup { get; set; }
        public string backup_type { get; set; }
        public string Physical_Device_name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DbaRC.Models
{
    public class InstanceError
    {
        public string ServerName { get; set; }
        public string LogDate { get; set; }
        public string ProcessInfo { get; set; }
        public string Text { get; set; }
    }
}

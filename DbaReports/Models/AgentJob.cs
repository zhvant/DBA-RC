using System;
using System.Collections.Generic;
using System.Text;

namespace DbaRC.Models
{
    public class AgentJob
    {
        public string ServerName { get; set; }
        public string JobName { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string Duration { get; set; }
        public string Message { get; set; }
    }
}

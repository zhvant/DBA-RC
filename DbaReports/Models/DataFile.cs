using System;
using System.Collections.Generic;
using System.Text;

namespace DbaReports.Models
{
    public class DataFile
    {
        public string FileGroupName { get; set; }
        public string Name { get; set; }
        public string UsedSpace { get; set; }
        public string MaxSpace { get; set; }
        public string AvailableSpace { get; set; }
    }
}

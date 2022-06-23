using System;
using System.Collections.Generic;
using System.Text;

namespace DbaRC.Models
{
    public class DataFile
    {
        public string FileGroupName { get; set; }
        public string Name { get; set; }
        public decimal UsedSpace { get; set; }
        public decimal MaxSpace { get; set; }
        public decimal AvailableSpace { get; set; }
    }
}

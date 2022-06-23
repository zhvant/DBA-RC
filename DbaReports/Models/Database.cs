using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DbaRC.Models
{
    public class Database
    {
        public string Name { get; set; }
        public decimal Size { get; set; } // Размер в МБ

        //public string ConvertToGb (decimal Size)
        //{
        //    Size = Size / 1024;
        //    return 
        //}

        //public string ConvertToTb(decimal Size)
        //{
            
        //    return Size/1024/1024;
        //}
    }
}

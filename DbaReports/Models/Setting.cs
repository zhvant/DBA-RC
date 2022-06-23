using System.ComponentModel.DataAnnotations;

namespace DbaRC.Models
{
    public class Setting
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string ServerName { get; set; }
        [Display(Name = "Connector")]
        public string ConnectionString { get; set; }
        public string? Type { get; set; }
    }
}

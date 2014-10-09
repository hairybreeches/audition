using System.ComponentModel.DataAnnotations;

namespace Sage50
{
    public class Sage50LoginDetails
    {        
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string DataDirectory { get; set; }
    }
}
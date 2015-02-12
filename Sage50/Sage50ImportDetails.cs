using System.ComponentModel.DataAnnotations;

namespace Sage50
{
    public class Sage50ImportDetails
    {        
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string DataDirectory { get; set; }        
    }
}
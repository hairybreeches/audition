using System.ComponentModel.DataAnnotations;

namespace Audition.Controllers
{
    public class XeroVerificationCode
    {
        [Required]
        public string Code { get; set; }
    }
}
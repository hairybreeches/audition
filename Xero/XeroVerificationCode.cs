using System.ComponentModel.DataAnnotations;

namespace Xero
{
    public class XeroVerificationCode
    {
        [Required]
        public string Code { get; set; }
    }
}
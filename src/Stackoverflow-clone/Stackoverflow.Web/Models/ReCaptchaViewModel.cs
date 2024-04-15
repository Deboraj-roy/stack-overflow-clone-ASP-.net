using System.ComponentModel.DataAnnotations;

namespace Stackoverflow.Web.Models
{
    public class ReCaptchaViewModel
    {
        [Required]
        public string Captcha { get; set; }
    }
}

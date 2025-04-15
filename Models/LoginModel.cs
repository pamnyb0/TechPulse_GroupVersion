using System.ComponentModel.DataAnnotations;

namespace TechPulse.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Du måste ange e-post eller användarnamn")]
        [Display(Name = "E-post eller användarnamn")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Du måste ange lösenord")]
        [Display(Name = "Lösenord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Håll mig inloggad")]
        public bool RememberMe { get; set; }
    }
}

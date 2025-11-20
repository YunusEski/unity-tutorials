using System.ComponentModel.DataAnnotations;

namespace UnityTutorialSite.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;
    }
}
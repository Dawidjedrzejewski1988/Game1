using System.ComponentModel.DataAnnotations;

namespace Game.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu e-mail.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Hasło musi zawierać co najmniej 8 znaków.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).*$", ErrorMessage = "Hasło musi zawierać przynajmniej jedną dużą literę i jedną cyfrę.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Potwierdzenie hasła jest wymagane.")]
        [Compare("Password", ErrorMessage = "Hasła muszą być identyczne.")]
        public string ConfirmPassword { get; set; }
    }
}

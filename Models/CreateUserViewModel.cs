using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

public class CreateUserViewModel
{
    [Required(ErrorMessage = "Email jest wymagany.")]
    [EmailAddress(ErrorMessage = "Podaj poprawny adres email.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Imię jest wymagane.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nazwisko jest wymagane.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Hasło jest wymagane.")]
    [MinLength(6, ErrorMessage = "Hasło musi mieć co najmniej 6 znaków.")]
    public string Password { get; set; } = string.Empty;

    public List<string> SelectedRoles { get; set; } = new();
    public List<string> AllRoles { get; set; } = new();
}

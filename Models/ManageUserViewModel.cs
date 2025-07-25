using System.ComponentModel.DataAnnotations;

public class ManageUserViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    public string? CurrentPassword { get; set; }

    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Nowe hasło musi mieć przynajmniej 6 znaków.")]
    public string? NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Hasła nie są zgodne.")]
    public string? ConfirmPassword { get; set; }
}

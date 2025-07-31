using System.ComponentModel.DataAnnotations;

public class ChangeEmailViewModel
{
    public string UserId { get; set; }

    [Display(Name = "Obecny adres e-mail")]
    public string CurrentEmail { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Nowy adres e-mail")]
    public string NewEmail { get; set; }
}

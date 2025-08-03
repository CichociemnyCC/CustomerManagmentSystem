using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRM_Duo_Creative.Models
{
    public class Lead : IValidatableObject
    {
        public int Id { get; set; }
        [Required]
        public string ClientName { get; set; } = string.Empty;
        [Required]
        public string? Source { get; set; } = string.Empty;
        public string? Services { get; set; } = string.Empty;
        public string? Packages { get; set; } = string.Empty;
        public string? ContractNumber { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactData { get; set; }
        public string? AccessGrantedBy { get; set; }
        public string? MetaAccount { get; set; }
        public string? MetaAgreement { get; set; }
        public string? LocalData { get; set; }
        public string? SocialWebGoogleDrive { get; set; }
        public string? Notes { get; set; }
        public DateTime? ServiceStartDate { get; set; }
        public DateTime? ServiceEndDate { get; set; }

        public bool IsConverted { get; set; } = false;
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ServiceStartDate.HasValue && ServiceEndDate.HasValue)
            {
                if (ServiceEndDate < ServiceStartDate)
                {
                    yield return new ValidationResult(
                        "Data zakończenia nie może być wcześniejsza niż data rozpoczęcia.",
                        new[] { nameof(ServiceEndDate) });
                }
            }
        }
    }
}

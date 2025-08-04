using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_Duo_Creative.Models
{
    public class RealizationStatus
    {
        public int Id { get; set; }

        // <-- TO jest poprawny klucz obcy
        public int RealizationId { get; set; }

        [ForeignKey("RealizationId")]
        public Realization Realization { get; set; }

        public DateTime Month { get; set; }

        public string Status { get; set; } = "Brak Danych";
    }
}

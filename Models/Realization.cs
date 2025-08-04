using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_Duo_Creative.Models
{
    public class Realization
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
        public DateTime Month { get; set; }
        public string Status { get; set; } = "Available next week";
        public string? AssignedTo { get; set; }
        public bool IsArchived { get; set; } = false;

        [ForeignKey("AssignedTo")]
        public ApplicationUser? AssignedUser { get; set; }
        public List<RealizationStatus> Statuses { get; set; } = new();


    }


}

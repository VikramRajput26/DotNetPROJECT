using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProjectJWT.Models
{
    public class Vaccines
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VaccineId { get; set; }

        [Required]
        [MaxLength(100)]  // Adjust the length as needed
        public string VaccineName { get; set; }

        [MaxLength(500)]  // Adjust the length as needed
        public string Description { get; set; }

        public int RecommendedAge { get; set; }

        [MaxLength(500)]  // Adjust the length as needed
        public string SideEffects { get; set; }
    }
}

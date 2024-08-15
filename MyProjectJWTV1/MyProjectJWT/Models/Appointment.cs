using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProjectJWT.Models
{
    [Table("Appointments")]
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentId { get; set; }

        [Required]
        [MaxLength(100)] // Adjust length as needed
        public string Reason { get; set; }

        [Required]
        [MaxLength(20)] // Adjust length as needed
        public string Status { get; set; }

        // Foreign key for Child
        [Required]
        [ForeignKey(nameof(Child))]
        public int ChildId { get; set; }

        public Child Child { get; set; }

        // Foreign key for User
        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public User User { get; set; }

        // Foreign key for Vaccines
        [Required]
        [ForeignKey(nameof(Vaccine))]
        public int VaccineId { get; set; }

        public Vaccines Vaccine { get; set; }

        public DateTime AppointmentDate { get; set; }
    }
}

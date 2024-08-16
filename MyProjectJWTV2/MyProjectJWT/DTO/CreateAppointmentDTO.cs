namespace MyProjectJWT.DTO
{
    public class CreateAppointmentDTO
    {
        public string Reason { get; set; }

        public string Status { get; set; }

        public int ChildId { get; set; }

        public int DoctorId { get; set; }

        public int VaccineId { get; set; }

        public DateTime AppointmentDate { get; set; }
        public TimeOnly AppointmentTime { get; set; }
    }
}

using MyProjectJWT.Context;
using MyProjectJWT.DTO;
using MyProjectJWT.Interfaces;
using MyProjectJWT.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MyProjectJWT.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly JwtContext _context;

        public AppointmentService(JwtContext context)
        {
            _context = context;
        }

        public AppointmentDTO AddAppointment(CreateAppointmentDTO createAppointment)
        {
            var appointment = new Appointment
            {
                Reason = createAppointment.Reason,
                Status = createAppointment.Status,
                ChildId = createAppointment.ChildId,
                DoctorId = createAppointment.DoctorId,
                VaccineId = createAppointment.VaccineId,
                AppointmentDate = createAppointment.AppointmentDate,
                AppointmentTime = createAppointment.AppointmentTime
            };

            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            return MapToDTO(appointment);
        }

        public bool DeleteAppointment(int id)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment == null)
            {
                return false;
            }

            _context.Appointments.Remove(appointment);
            _context.SaveChanges();
            return true;
        }

        public AppointmentDTO GetAppointmentById(int id)
        {
            var appointment = _context.Appointments
                .Where(a => a.AppointmentId == id)
                .FirstOrDefault();

            return appointment != null ? MapToDTO(appointment) : null;
        }

        public List<AppointmentDTO> GetAppointmentDetails()
        {
            return _context.Appointments
                .Select(a => MapToDTO(a))
                .ToList();
        }

        public AppointmentDTO UpdateAppointment(AppointmentDTO appointmentDTO)
        {
            var appointment = _context.Appointments.Find(appointmentDTO.AppointmentId);
            if (appointment == null)
            {
                return null;
            }

            appointment.Reason = appointmentDTO.Reason;
            appointment.Status = appointmentDTO.Status;
            appointment.ChildId = appointmentDTO.ChildId;
            appointment.DoctorId = appointmentDTO.DoctorId;
            appointment.VaccineId = appointmentDTO.VaccineId;
            appointment.AppointmentDate = appointmentDTO.AppointmentDate;
            appointment.AppointmentTime = appointmentDTO.AppointmentTime;

            _context.Appointments.Update(appointment);
            _context.SaveChanges();

            return MapToDTO(appointment);
        }

        private AppointmentDTO MapToDTO(Appointment appointment)
        {
            return new AppointmentDTO
            {
                AppointmentId = appointment.AppointmentId,
                Reason = appointment.Reason,
                Status = appointment.Status,
                ChildId = appointment.ChildId,
                DoctorId = appointment.DoctorId,
                VaccineId = appointment.VaccineId,
                AppointmentDate = appointment.AppointmentDate,
                AppointmentTime = appointment.AppointmentTime
            };
        }
    }
}

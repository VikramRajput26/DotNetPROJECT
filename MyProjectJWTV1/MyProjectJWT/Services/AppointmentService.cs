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

        // Get all appointment details
        public List<AppointmentDTO> GetAppointmentDetails()
        {
            return _context.Appointments
                .Select(a => new AppointmentDTO
                {
                    AppointmentId = a.AppointmentId,
                    Reason = a.Reason,
                    Status = a.Status,
                    ChildId = a.ChildId,
                    UserId = a.UserId,
                    VaccineId = a.VaccineId,
                    AppointmentDate = a.AppointmentDate
                })
                .ToList();
        }

        // Get appointment by Id
        public AppointmentDTO GetAppointmentById(int id)
        {
            var appointment = _context.Appointments
                .Where(a => a.AppointmentId == id)
                .Select(a => new AppointmentDTO
                {
                    AppointmentId = a.AppointmentId,
                    Reason = a.Reason,
                    Status = a.Status,
                    ChildId = a.ChildId,
                    UserId = a.UserId,
                    VaccineId = a.VaccineId,
                    AppointmentDate = a.AppointmentDate
                })
                .FirstOrDefault();

            return appointment;
        }

        // Add a new appointment
        public AppointmentDTO AddAppointment(CreateAppointmentDTO createAppointment)
        {
            var appointment = new Appointment
            {
                Reason = createAppointment.Reason,
                Status = createAppointment.Status,
                ChildId = createAppointment.ChildId,
                UserId = createAppointment.UserId,
                VaccineId = createAppointment.VaccineId,
                AppointmentDate = createAppointment.AppointmentDate
            };

            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            return new AppointmentDTO
            {
                AppointmentId = appointment.AppointmentId,
                Reason = appointment.Reason,
                Status = appointment.Status,
                ChildId = appointment.ChildId,
                UserId = appointment.UserId,
                VaccineId = appointment.VaccineId,
                AppointmentDate = appointment.AppointmentDate
            };
        }

        // Update an existing appointment
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
            appointment.UserId = appointmentDTO.UserId;
            appointment.VaccineId = appointmentDTO.VaccineId;
            appointment.AppointmentDate = appointmentDTO.AppointmentDate;

            _context.Appointments.Update(appointment);
            _context.SaveChanges();

            return appointmentDTO;
        }

        // Delete an appointment by Id
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
    }
}

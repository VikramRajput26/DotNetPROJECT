using Microsoft.AspNetCore.Mvc;
using MyProjectJWT.DTO;
using MyProjectJWT.Interfaces;
using MyProjectJWT.Models;
using System.Collections.Generic;

namespace MyProjectJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // GET: api/Appointment/getall
        [HttpGet("getall")]
        public ActionResult<IEnumerable<AppointmentDTO>> GetAll()
        {
            var appointments = _appointmentService.GetAppointmentDetails();
            return Ok(appointments);
        }

        // GET: api/Appointment/getbyid/5
        [HttpGet("getbyid/{id}")]
        public ActionResult<AppointmentDTO> GetById(int id)
        {
            var appointment = _appointmentService.GetAppointmentById(id);

            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }

        // POST: api/Appointment/add
        [HttpPost("add")]
        public ActionResult<AppointmentDTO> Add([FromBody] CreateAppointmentDTO createAppointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointment = _appointmentService.AddAppointment(createAppointment);
            return CreatedAtAction(nameof(GetById), new { id = appointment.AppointmentId }, appointment);
        }

        // PUT: api/Appointment/update/5
        [HttpPut("update/{id}")]
        public ActionResult<AppointmentDTO> Update(int id, [FromBody] AppointmentDTO appointmentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointment = _appointmentService.GetAppointmentById(id);

            if (appointment == null)
            {
                return NotFound();
            }

            appointmentDTO.AppointmentId = id; // Ensure the ID is set to the correct value

            var updatedAppointment = _appointmentService.UpdateAppointment(appointmentDTO);

            if (updatedAppointment == null)
            {
                return NotFound();
            }

            return Ok(updatedAppointment);
        }

        // DELETE: api/Appointment/delete/5
        [HttpDelete("delete/{id}")]
        public ActionResult Delete(int id)
        {
            var appointment = _appointmentService.GetAppointmentById(id);

            if (appointment == null)
            {
                return NotFound();
            }

            var result = _appointmentService.DeleteAppointment(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

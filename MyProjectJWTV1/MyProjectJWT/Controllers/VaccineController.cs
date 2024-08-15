using Microsoft.AspNetCore.Mvc;
using MyProjectJWT.DTO;
using MyProjectJWT.Interfaces;
using MyProjectJWT.Models;
using System.Collections.Generic;

namespace MyProjectJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineController : ControllerBase
    {
        private readonly IVaccineService _vaccineService;

        public VaccineController(IVaccineService vaccineService)
        {
            _vaccineService = vaccineService;
        }

        // GET: api/Vaccine/getall
        [HttpGet("getall")]
        public ActionResult<IEnumerable<VaccineDTO>> GetAll()
        {
            var vaccines = _vaccineService.GetAllVaccines();
            return Ok(vaccines);
        }

        // GET: api/Vaccine/getbyid/5
        [HttpGet("getbyid/{id}")]
        public ActionResult<VaccineDTO> GetById(int id)
        {
            var vaccine = _vaccineService.GetVaccineById(id);

            if (vaccine == null)
            {
                return NotFound();
            }

            return Ok(vaccine);
        }

        // POST: api/Vaccine/addvaccine
        [HttpPost("addvaccine")]
        public ActionResult<VaccineDTO> AddVaccine([FromBody] CreateVaccineDTO createVaccine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vaccine = _vaccineService.AddVaccine(createVaccine);
            return CreatedAtAction(nameof(GetById), new { id = vaccine.VaccineId }, vaccine);
        }

        // PUT: api/Vaccine/update/5
        [HttpPut("update/{id}")]
        public ActionResult<VaccineDTO> UpdateVaccine(int id, [FromBody] VaccineDTO vaccineDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingVaccine = _vaccineService.GetVaccineById(id);

            if (existingVaccine == null)
            {
                return NotFound();
            }

            vaccineDTO.VaccineId = id; // Ensure the ID is set correctly
            var updatedVaccine = _vaccineService.UpdateVaccine(vaccineDTO);

            if (updatedVaccine == null)
            {
                return NotFound();
            }

            return Ok(updatedVaccine);
        }

        // DELETE: api/Vaccine/delete/5
        [HttpDelete("delete/{id}")]
        public ActionResult DeleteVaccine(int id)
        {
            var vaccine = _vaccineService.GetVaccineById(id);

            if (vaccine == null)
            {
                return NotFound();
            }

            var result = _vaccineService.DeleteVaccine(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

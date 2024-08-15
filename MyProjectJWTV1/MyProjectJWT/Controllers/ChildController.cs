using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProjectJWT.DTO;
using MyProjectJWT.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;

namespace MyProjectJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildController : ControllerBase
    {
        private readonly IChildService _childService;

        public ChildController(IChildService childService)
        {
            _childService = childService;
        }

        // GET: api/Child
        [HttpGet]
        [Authorize(Roles = "USER,ADMIN")]
        public ActionResult<List<ChildDTO>> GetAllChildren()
        {
            var children = _childService.GetChildDetails();
            return Ok(children);
        }

        // GET api/Child/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "USER,ADMIN")]
        public ActionResult<ChildDTO> GetChildById(int id)
        {
            var child = _childService.GetChildById(id);
            if (child == null)
                return NotFound("Child not found");

            return Ok(child);
        }

        // POST api/Child
        [HttpPost]
        [Authorize(Roles = "USER,ADMIN")]
     
        public IActionResult AddChild([FromBody] CreateChildDTO createChildDto)
        {
            // Extract UserId from the JWT token
            var userIdClaim = User.FindFirstValue("Id");
            if (userIdClaim == null)
            {
                return Unauthorized("UserId claim not found.");
            }

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return BadRequest("Invalid UserId.");
            }

            createChildDto.UserId = userId;

            // Call the service to add the child
            var childDto = _childService.AddChild(createChildDto);

            return Ok(childDto);
        }


        // PUT api/Child/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "USER,ADMIN")]
        public ActionResult<ChildDTO> UpdateChild(int id, [FromBody] ChildDTO childDto)
        {
            if (childDto == null || id != childDto.ChildId)
                return BadRequest("Child data is invalid");

            var updatedChild = _childService.UpdateChild(childDto);
            if (updatedChild == null)
                return NotFound("Child not found");

            return Ok(updatedChild);
        }

        // DELETE api/Child/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public ActionResult DeleteChild(int id)
        {
            var result = _childService.DeleteChild(id);
            if (!result)
                return NotFound("Child not found");

            return NoContent();
        }

        [HttpGet("GetUserClaims")]
        public IActionResult GetUserClaims()
        {
            var allClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

            return Ok(new
            {
                 AllClaims = allClaims
            });
        }

    }
}

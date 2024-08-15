using MyProjectJWT.Context;
using MyProjectJWT.Interfaces;
using MyProjectJWT.Models;
using MyProjectJWT.DTO;
using System.Linq;
using System.Collections.Generic;

namespace MyProjectJWT.Services
{
    public class RoleService : IRoleService
    {
        private readonly JwtContext _context;

        public RoleService(JwtContext context)
        {
            _context = context;
        }

        public RoleDTO AddRole(RoleDTO roleDto)
        {
            if (roleDto == null)
                throw new ArgumentNullException(nameof(roleDto));

            var role = new Role
            {
                Name = roleDto.Name
            };

            var addedRole = _context.Roles.Add(role);
            _context.SaveChanges();

            // Convert to DTO before returning
            return new RoleDTO
            {
                RoleId = addedRole.Entity.RoleId,
                Name = addedRole.Entity.Name
            };
        }

        public RoleDTO GetRoleById(int id)
        {
            var role = _context.Roles.SingleOrDefault(r => r.RoleId == id);
            if (role == null)
                throw new Exception("Role not found");

            // Convert to DTO
            return new RoleDTO
            {
                RoleId = role.RoleId,
                Name = role.Name
            };
        }

        public IEnumerable<RoleDTO> GetAllRoles()
        {
            var roles = _context.Roles.ToList();

            // Convert to DTO
            return roles.Select(role => new RoleDTO
            {
                RoleId = role.RoleId,
                Name = role.Name
            }).ToList();
        }

        public RoleDTO UpdateRole(int id, RoleDTO roleDto)
        {
            var role = _context.Roles.SingleOrDefault(r => r.RoleId == id);
            if (role == null)
                throw new Exception("Role not found");

            role.Name = roleDto.Name;
            _context.Roles.Update(role);
            _context.SaveChanges();

            // Convert to DTO
            return new RoleDTO
            {
                RoleId = role.RoleId,
                Name = role.Name
            };
        }

        public bool DeleteRole(int id)
        {
            var role = _context.Roles.SingleOrDefault(r => r.RoleId == id);
            if (role == null)
                throw new Exception("Role not found");

            _context.Roles.Remove(role);
            _context.SaveChanges();
            return true;
        }
    }
}

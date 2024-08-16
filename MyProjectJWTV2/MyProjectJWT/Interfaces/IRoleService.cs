using MyProjectJWT.Models;
using MyProjectJWT.DTO;

namespace MyProjectJWT.Interfaces
{
    public interface IRoleService
    {
        RoleDTO AddRole(RoleDTO roleDto);
        RoleDTO GetRoleById(int id);
        IEnumerable<RoleDTO> GetAllRoles();
        RoleDTO UpdateRole(int id, RoleDTO roleDto);
        bool DeleteRole(int id);
    }
}

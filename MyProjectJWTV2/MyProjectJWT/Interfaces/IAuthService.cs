using MyProjectJWT.DTO;
using MyProjectJWT.Models;

namespace MyProjectJWT.Interfaces
{
    public interface IAuthService
    {
        UserDTO AddUser(CreateUserDTO userDto);
        UserDTO GetUserById(int id);
        IEnumerable<UserDTO> GetAllUsers();
        UserDTO UpdateUser(int id, UpdateUserDTO userDto);
        bool DeleteUser(int id);
        LoginResponse Login(LoginRequest loginRequest);
        Role AddRole(Role role);
        bool AssignRoleToUser(AddUserRole obj);
    }
}

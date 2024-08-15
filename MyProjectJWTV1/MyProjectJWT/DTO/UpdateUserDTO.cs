namespace MyProjectJWT.DTO
{
    public class UpdateUserDTO
    {
        public int UserId { get; set; } // Id is required for update
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Optional, include only if updating the password
        public RoleDTO Role { get; set; }
    }
}

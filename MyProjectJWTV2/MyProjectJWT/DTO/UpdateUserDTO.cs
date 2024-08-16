namespace MyProjectJWT.DTO
{
    public class UpdateUserDTO
    {
        public int UserId { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ContactNumber { get; set; }
        public RoleDTO Role { get; set; }
    }
}

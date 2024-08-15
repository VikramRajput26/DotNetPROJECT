﻿namespace MyProjectJWT.DTO
{
    public class CreateUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public CreateRoleDTO Role { get; set; }
    }
}

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using MyProjectJWT.Context;
using MyProjectJWT.Interfaces;
using MyProjectJWT.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MyProjectJWT.DTO;
using System.Linq;

namespace MyProjectJWT.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtContext _context;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public AuthService(JwtContext context, IConfiguration configuration, EmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

        public Role AddRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            var addedRole = _context.Roles.Add(role);
            _context.SaveChanges();
            return addedRole.Entity;
        }

        public UserDTO AddUser(CreateUserDTO userDto)
        {
            if (userDto == null)
                throw new ArgumentNullException(nameof(userDto));

            // Create a new User object using the DTO data
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = HashPassword(userDto.Password) // Hash the password before saving
            };

            // Add the user to the context
            _context.Users.Add(user);
            _context.SaveChanges(); // Save the user first to get the UserId

            // Handle the role assignment
            var role = _context.Roles.SingleOrDefault(r => r.Name == userDto.Role.Name);
            if (role == null)
            {
                // If the role doesn't exist, create it
                role = new Role { Name = userDto.Role.Name };
                _context.Roles.Add(role);
                _context.SaveChanges();
            }

            // Assign the role to the user
            _context.UserRoles.Add(new UserRole { UserId = user.UserId, RoleId = role.RoleId });
            _context.SaveChanges();

            // Convert the user entity to a UserDTO
            var userDtoResult = new UserDTO
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = new RoleDTO { Name = role.Name }
            };

            // Send registration success email
            string subject = "Registration Successful";
            string body = $"Hello {user.FirstName},<br/><br/>" +
                          "Thank you for registering. Your registration was successful!<br/><br/>" +
                          "Best regards,<br/>The Team";
            _emailService.SendEmail(user.Email, subject, body);

            // Return the UserDTO
            return userDtoResult;
        }

        public UserDTO GetUserById(int id)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserId == id);
            if (user == null)
                throw new Exception("User not found");

            var role = _context.UserRoles
                        .Where(ur => ur.UserId == user.UserId)
                        .Select(ur => _context.Roles.SingleOrDefault(r => r.RoleId == ur.RoleId))
                        .FirstOrDefault();

            return new UserDTO
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = new RoleDTO { Name = role?.Name }
            };
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            return _context.Users.ToList().Select(user => new UserDTO
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = _context.UserRoles
                               .Where(ur => ur.UserId == user.UserId)
                               .Select(ur => ur.RoleId)
                               .Select(roleId => _context.Roles.SingleOrDefault(r => r.RoleId == roleId))
                               .Select(r => new RoleDTO { RoleId = r.RoleId, Name = r.Name }) // Added Role Id to DTO
                               .FirstOrDefault()
            });
        }

        public UserDTO UpdateUser(int id, UpdateUserDTO userDto)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserId == id);
            if (user == null)
                throw new Exception("User not found");

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                user.Password = HashPassword(userDto.Password); // Hash the new password before saving
            }

            // Update user roles if needed
            if (userDto.Role != null)
            {
                var role = _context.Roles.SingleOrDefault(r => r.Name == userDto.Role.Name);
                if (role == null)
                {
                    role = new Role { Name = userDto.Role.Name };
                    _context.Roles.Add(role);
                    _context.SaveChanges();
                }

                var existingUserRole = _context.UserRoles.SingleOrDefault(ur => ur.UserId == user.UserId);
                if (existingUserRole != null)
                {
                    _context.UserRoles.Remove(existingUserRole);
                }

                _context.UserRoles.Add(new UserRole { UserId = user.UserId, RoleId = role.RoleId });
            }

            _context.Users.Update(user);
            _context.SaveChanges();

            var updatedRole = _context.UserRoles
                        .Where(ur => ur.UserId == user.UserId)
                        .Select(ur => _context.Roles.SingleOrDefault(r => r.RoleId == ur.RoleId))
                        .FirstOrDefault();

            return new UserDTO
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = new RoleDTO { Name = updatedRole?.Name }
            };
        }

        public bool DeleteUser(int id)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserId == id);
            if (user == null)
                throw new Exception("User not found");

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }

        public bool AssignRoleToUser(AddUserRole obj)
        {
            if (obj == null || obj.RoleIds == null || obj.RoleIds.Count == 0)
                throw new ArgumentNullException(nameof(obj));

            var user = _context.Users.SingleOrDefault(s => s.UserId == obj.UserId);
            if (user == null)
                throw new Exception("User is not valid");

            var existingRoles = _context.UserRoles.Where(ur => ur.UserId == user.UserId).ToList();
            _context.UserRoles.RemoveRange(existingRoles);

            var newRoles = obj.RoleIds.Select(roleId => new UserRole
            {
                UserId = user.UserId,
                RoleId = roleId
            }).ToList();

            _context.UserRoles.AddRange(newRoles);
            _context.SaveChanges();
            return true;
        }

        public LoginResponse Login(LoginRequest loginRequest)
        {
            if (loginRequest == null)
                throw new ArgumentNullException(nameof(loginRequest));

            if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
                throw new Exception("Credentials are not valid");

            var user = _context.Users.SingleOrDefault(s => s.Email == loginRequest.Username);
            if (user == null || !VerifyPassword(user.Password, loginRequest.Password))
                throw new Exception("Invalid credentials");

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            new Claim("UserId", user.UserId.ToString()),
            new Claim("UserName", $"{user.FirstName} {user.LastName}")
        };

            var userRoles = _context.UserRoles.Where(u => u.UserId == user.UserId).ToList();
            var roleIds = userRoles.Select(s => s.RoleId).ToList();
            var roles = _context.Roles.Where(r => roleIds.Contains(r.RoleId)).ToList();
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn
            );

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserId = user.UserId
            };
        }

        private static string HashPassword(string password)
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            var hashed = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            );

            // Store salt and hash together, separated by a dot
            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hashed)}";
        }

        private static bool VerifyPassword(string storedPassword, string providedPassword)
        {
            var parts = storedPassword.Split('.');
            if (parts.Length != 2)
                throw new InvalidOperationException("Invalid password format");

            var storedSalt = Convert.FromBase64String(parts[0]);
            var storedHash = Convert.FromBase64String(parts[1]);

            var providedHash = KeyDerivation.Pbkdf2(
                password: providedPassword,
                salt: storedSalt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            );

            return providedHash.SequenceEqual(storedHash);
        }
    }
}

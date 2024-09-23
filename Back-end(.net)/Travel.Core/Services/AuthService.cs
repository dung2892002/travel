using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Travel.Core.DTOs;
using Travel.Core.Entities;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Services
{
    public class AuthService(IUnitOfWork unitOfWork, IJwtService jwtService) : IAuthService, IService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IJwtService _jwtService = jwtService;
        public async Task<LoginResponse?> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _unitOfWork.Users.GetUserByUsernameAsync(loginRequest.Username);

            if (user == null || !VerifyPassword(user.Password, loginRequest.Password)) throw new UnauthorizedAccessException("Invalid username or password.");

            var userDto = new UserDTO
            {
                Id = user.Id,
                Fullname = user.Fullname,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DisplayName = user.DisplayName,
                AvatarImage = user.AvatarImage,
                Roles = user.UserRole.Select(ur => new RoleDTO
                {
                    Id = ur.Role.Id,
                    Name = ur.Role.Name,
                    RoleValue = ur.Role.RoleValue
                }).ToList()
            };

            var token = _jwtService.GenerateToken(userDto);

            var loginResponse = new LoginResponse
            {
                Token = token,
                User = userDto
            };
            return loginResponse;
        }

        public async Task<bool> RegisterAsync(RegisterRequest registerRequest)
        {
            var existinguser = await _unitOfWork.Users.GetUserByUsernameAsync(registerRequest.Username);
            if (existinguser != null) return false;
            var user = new User
            {
                Id = Guid.NewGuid(),
                Fullname = registerRequest.Fullname,
                Email = registerRequest.Email,
                PhoneNumber = registerRequest.PhoneNumber,
                Username = registerRequest.Username,
                Password = HashPassword(registerRequest.Password),
                DisplayName = registerRequest.Username,
                AvatarImage = "avatar-default.jpg",
            };

            var role = await _unitOfWork.Roles.GetRoleByRoleValueAsync(2);

            if (role == null) return false;

            var userRole = new UserRole
            {
                User = user,
                Role = role,
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.UserRoles.AddAsync(userRole);

            var result = await _unitOfWork.CompleteAsync();

            return result > 0;
        }


        private string HashPassword(string password)
        {
            var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string storedHash, string password)
        {
            return storedHash == HashPassword(password);
        }

        public async Task<bool> CreateAdminAccount(string jwt, RegisterRequest registerRequest)
        {
            var existinguser = await _unitOfWork.Users.GetUserByUsernameAsync(registerRequest.Username);
            if (existinguser != null) return false;

            var principal = _jwtService.ValidateToken(jwt);
            var username = principal.Identity.Name;
            var roles = principal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            if (!roles.Contains("Admin"))
            {
                throw new UnauthorizedAccessException("You do not have permission to create an admin account.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Fullname = registerRequest.Fullname,
                Email = registerRequest.Email,
                PhoneNumber = registerRequest.PhoneNumber,
                Username = registerRequest.Username,
                Password = HashPassword(registerRequest.Password),
                DisplayName = registerRequest.Username,
                AvatarImage = "avatar-default.jpg",
            };

            var role = await _unitOfWork.Roles.GetRoleByRoleValueAsync(2);

            if (role == null) return false;

            var userRole = new UserRole
            {
                User = user,
                Role = role,
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.UserRoles.AddAsync(userRole);

            var result = await _unitOfWork.CompleteAsync();

            return result > 0;
        }
    }
}

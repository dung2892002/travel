using System.Text.RegularExpressions;
using Travel.Core.DTOs;
using Travel.Core.Entities;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Services
{
    public class UserService(IUnitOfWork unitOfWork, IJwtService jwtService, IFirebaseStorageService firebaseStorageService) : IUserService, IService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IFirebaseStorageService _firebaseStorageService = firebaseStorageService;
        public async Task<LoginResponse?> Login(LoginRequest loginRequest)
        {
            var user = await _unitOfWork.Users.GetUserByUsername(loginRequest.Username);
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

        public async Task<bool> Register(RegisterRequest registerRequest)
        {

            var user = await ValidateAndCreateUser(registerRequest);
            if (user == null) return false;

            return await CreateUserAndAssignRole(user, 1);
        }

        public async Task<bool> CreateAdminAccount(RegisterRequest registerRequest)
        {
            var user = await ValidateAndCreateUser(registerRequest);
            if (user == null) return false;

            return await CreateUserAndAssignRole(user, 10);
        }

        public async Task<bool> CreateHotelAccount(RegisterRequest registerRequest)
        {
            var user = await ValidateAndCreateUser(registerRequest);
            if (user == null) return false;

            return await CreateUserAndAssignRole(user, 5);
        }

        public async Task<bool> CreateTourAccount(RegisterRequest registerRequest)
        {
            var user = await ValidateAndCreateUser(registerRequest);
            if (user == null) return false;

            return await CreateUserAndAssignRole(user, 6);
        }

        public async Task<bool> ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            if (!ValidatePassword(changePasswordRequest.NewPassword)) 
                throw new InvalidDataException("Password requires at least 8 characters, at least 1 lowercase letter, 1 uppercase letter, 1 number and 1 special character");

            if (changePasswordRequest.NewPassword != changePasswordRequest.ReNewPassword)
                throw new InvalidDataException("NewPassword and ReNewPassword must be the same");

            var user = await _unitOfWork.Users.GetUserByUsername(changePasswordRequest.Username);
            if (user == null || !VerifyPassword(user.Password, changePasswordRequest.Password)) throw new UnauthorizedAccessException("Invalid username or password.");

            user.Password = HashPassword(changePasswordRequest.NewPassword);

            var result = await _unitOfWork.CompleteAsync();

            return result > 0;
        }

        public async Task<bool> UpdateInfo( Guid id, User user)
        {
            if (!ValidateEmail(user.Email))
                throw new InvalidDataException("Please enter email in correct format");
            if (!ValidatePhoneNumber(user.PhoneNumber))
                throw new InvalidDataException("Please enter phone number in correct format");

            var existingUser = await _unitOfWork.Users.GetUserById(id);
            if (existingUser == null) throw new ArgumentException("User not found");

            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.DisplayName = user.DisplayName;
            existingUser.Fullname = user.Fullname;

            var result = await _unitOfWork.CompleteAsync();

            return result > 0;

        }

        public async Task<bool> ChangeAvatar(Guid id, Stream fileStream, string fileName)
        {
            var existingUser = await _unitOfWork.Users.GetUserById(id);
            if (existingUser == null) throw new ArgumentException("User not found");

            fileName = DateTime.Now.ToString("dd-MM-yyyy") + "_" + Guid.NewGuid().ToString() + "_" + fileName;
            var uploadSuccess = await _firebaseStorageService.UploadFile(fileStream, fileName);
            if (!uploadSuccess) return false;

            existingUser.AvatarImage = fileName;
            var result = await _unitOfWork.CompleteAsync();

            return result > 0;
        }

        private async Task<bool> CreateUserAndAssignRole(User user, int roleValue)
        {
            var role = await _unitOfWork.Roles.GetRoleByRoleValue(roleValue);
            if (role == null) return false;

            var userRole = new UserRole
            {
                User = user,   
                Role = role,
            };

            await _unitOfWork.Users.Add(user);
            await _unitOfWork.UserRoles.Add(userRole);

            if (roleValue != 1)
            {
                var baseRole = await _unitOfWork.Roles.GetRoleByRoleValue(1);
                if (baseRole == null) return false;
                var baseUserRole = new UserRole
                {
                    User = user,
                    Role = baseRole,
                };
                await _unitOfWork.UserRoles.Add(baseUserRole);
            }
            var result = await _unitOfWork.CompleteAsync();

            return result > 0;
        }

        private async Task<User?> ValidateAndCreateUser(RegisterRequest registerRequest)
        {
            if (!ValidatePassword(registerRequest.Password))
                throw new InvalidDataException("Password requires at least 8 characters, at least 1 lowercase letter, 1 uppercase letter, 1 number and 1 special character");
            if (!ValidateEmail(registerRequest.Email))
                throw new InvalidDataException("Please enter email in correct format");
            if (!ValidatePhoneNumber(registerRequest.PhoneNumber))
                throw new InvalidDataException("Please enter phone number in correct format");

            if (registerRequest.Password != registerRequest.RePassword)
                throw new InvalidDataException("Password and RePassword must be the same");

            var existingUser = await _unitOfWork.Users.GetUserByUsername(registerRequest.Username);
            if (existingUser != null) return null;

            var user = new User
            {
                Id = Guid.NewGuid(),
                Fullname = registerRequest.Fullname,
                Email = registerRequest.Email,
                PhoneNumber = registerRequest.PhoneNumber,
                Username = registerRequest.Username,
                Password = HashPassword(registerRequest.Password),
                DisplayName = registerRequest.Fullname,
                AvatarImage = "avatar-default.jpg",
            };

            return user;
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private static bool VerifyPassword(string storedHash, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        private static bool ValidatePassword(string password)
        {
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            return Regex.IsMatch(password, passwordPattern);
        }

        private static bool ValidateEmail(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, emailPattern);
        }

        private static bool ValidatePhoneNumber(string phoneNumber)
        {
            string phonePattern = @"^(0[3|5|7|8|9])+([0-9]{8})$";
            return Regex.IsMatch(phoneNumber, phonePattern);
        }

        
    }
}

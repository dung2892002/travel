﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Core.DTOs;
using Travel.Core.Interfaces.IServices;

namespace Travel.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var response = await _userService.Register(request);
                if (!response)
                    return StatusCode(409, "Username existing");
                return StatusCode(201, "Register user successfully");
            }
            catch (InvalidDataException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _userService.Login(request);
                return StatusCode(200, response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(401, ex.Message);
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdminAccount([FromBody] RegisterRequest request)
        {
            try
            {
                var result = await _userService.CreateAdminAccount(request);
                if (!result) return StatusCode(409, "Username existing");

                return StatusCode(201, "Created admin account successfully");
            }
            catch (InvalidDataException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("create-tour-account")]
        public async Task<IActionResult> CreateTourAccount([FromBody] RegisterRequest request)
        {
            try
            {
                var result = await _userService.CreateTourAccount(request);

                if (!result) return StatusCode(409, "Username existing");

                return StatusCode(201, "Created tour account successfully");
            }
            catch (InvalidDataException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("create-hotel-account")]
        public async Task<IActionResult> CreateHotelAccount([FromBody] RegisterRequest request)
        {
            try
            {
                var result = await _userService.CreateHotelAccount(request);

                if (!result) return StatusCode(409, "Username existing");

                return StatusCode(201, "Created hotel account successfully");
            }
            catch (InvalidDataException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var result = await _userService.ChangePassword(request);
                if (!result) return StatusCode(400, "Failed to change password");
                return StatusCode(201, "Change password successfully");
            }
            catch (InvalidDataException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "User")]
        [HttpPut("update-info")]
        public async Task<IActionResult> UpdateInfo([FromQuery] Guid id, [FromBody] UserDTO user)
        {
            try
            {
                var response = await _userService.UpdateInfo(id, user);

                if (!response) return StatusCode(400, "No information has been changed");

                var userResponse = await _userService.GetDetailUser(id);

                return StatusCode(200, userResponse);
            }
            catch (InvalidDataException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(404, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [Authorize(Policy = "User")]
        [HttpPut("change-avatar")]
        public async Task<IActionResult> ChangeAvatar([FromQuery] Guid id, IFormFile file)
        {
            try
            {
                if (file == null)
                {
                    return StatusCode(400, "No file upload");
                }

                using Stream stream = file.OpenReadStream();
                var fileName = file.FileName;
                var result = await _userService.ChangeAvatar(id, stream, fileName);

                if (!result) return StatusCode(400, "No information has been changed");

                var userResponse = await _userService.GetDetailUser(id);

                return StatusCode(200, userResponse);

            }
            catch (ArgumentException ex)
            {
                return StatusCode(404, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUser([FromQuery] string? keyword, [FromQuery] int pageSize, [FromQuery] int pageNumber )
        {
            try
            {
                var result = await _userService.GetUser(keyword, pageSize, pageNumber);

                return StatusCode(200, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Authorize(Policy = "Admin")]
        [HttpPatch("lock")]
        public async Task<IActionResult> LockUserAccount([FromBody] Guid id)
        {
            try
            {
                var response = await _userService.LockUser(id);

                if (!response) return StatusCode(400, "No information has been changed");

                return StatusCode(200, "Lock account successfully");
            }
            catch (InvalidDataException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [Authorize(Policy = "Admin")]
        [HttpPatch("unlock")]
        public async Task<IActionResult> UnlockUserAccount([FromBody] Guid id)
        {
            try
            {
                var response = await _userService.UnlockUser(id);
                if (!response) return StatusCode(400, "No information has been changed");

                return StatusCode(200, "Unlock account successfully");
            }
            catch (InvalidDataException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

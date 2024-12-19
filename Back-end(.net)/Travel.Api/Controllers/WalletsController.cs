using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IServices;

namespace Travel.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WalletsController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        
        [HttpPost]
        public async Task<IActionResult> CreateWallet([FromBody] Wallet wallet)
        {
            try
            {
                await _userService.CreateWallet(wallet);
                return StatusCode(201, "create wallet successfully");
            }
            catch (InvalidDataException ex)
            {
                return StatusCode(409, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetWallet(Guid userId)
        {
            try
            {
                var wallet = await _userService.GetWallet(userId);
                return StatusCode(200, wallet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetWalletPositiveBalance([FromQuery] int pageNumber)
        {
            try
            {
                var wallets = await _userService.GetWalletsWithPositiveBalance(pageNumber);
                return StatusCode(200, wallets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateWallet(Guid userId, [FromBody] Wallet wallet)
        {
            try
            {
                var result = await _userService.UpdateWallet(userId, wallet);
                return StatusCode(200, "update successfuly");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpPatch]
        public async Task<IActionResult> PaymentWallet([FromBody] Guid id)
        {

            try
            {
                var result = await _userService.PaymentWallet(id);
                if (!result) return StatusCode(400, "Payment failed");
                return StatusCode(200, "payment successfully");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(409, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

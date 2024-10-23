using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel.Core.DTOs;
using Travel.Core.Interfaces;

namespace Travel.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentsController(IPaymentService paymentService) : ControllerBase
    {
        private readonly IPaymentService _paymentService = paymentService;

        [HttpPost("pay-room")]
        public async Task<IActionResult> PayForRoom([FromBody] PaymentRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid payment request.");
            }

            request.BookingType = "Room";
            var paymentUrl = await _paymentService.ProcessPaymentForRoom(request);
            return Ok(new { Url = paymentUrl });
        }

        [HttpPost("pay-tour")]
        public async Task<IActionResult> PayForTour([FromBody] PaymentRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid payment request.");
            }

            request.BookingType = "Tour";
            var paymentUrl = await _paymentService.ProcessPaymentForTour(request);
            return Ok(new { Url = paymentUrl });
        }

        [HttpGet("payment-response/room")]
        public async Task<IActionResult> PaymentResponseForRoom()
        {
            var vnpayData = HttpContext.Request.Query;

            if (vnpayData == null)
            {
                return BadRequest("Invalid payment response.");
            }

            var result = await _paymentService.PaymentExecute(vnpayData, 0);
            if (!result)
            {
                return BadRequest("Payment failed.");
            }
            return Ok("Payment successful.");
        }

        [HttpGet("payment-response/tour")]
        public async Task<IActionResult> PaymentResponseForTour()
        {
            var vnpayData = HttpContext.Request.Query;

            if (vnpayData == null)
            {
                return BadRequest("Invalid payment response.");
            }

            var result = await _paymentService.PaymentExecute(vnpayData, 1);
            if (!result)
            {
                return BadRequest("Payment failed.");
            }
            return Ok("Payment successful.");
        }
    }
}

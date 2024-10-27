using Microsoft.EntityFrameworkCore;
using Travel.Core.Authorization;
using Travel.Core.Entities;
using Travel.Core.Interfaces;
using Travel.Infrastructure.Data;

namespace Travel.Api.Middleware
{
    public class HotelOwnershipMiddleware(RequestDelegate requestDelegate)
    {
        private readonly RequestDelegate _requestDelegate = requestDelegate;

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/v1/Hotels/edit"))
            {
                if (!Guid.TryParse(context.Request.Query["hotelId"], out var hotelId))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Invalid hotelId.");
                    return;
                }
                var jwt = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                
                using (var scope = context.RequestServices.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<TravelDbContext>();
                    var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();

                    var userId = jwtService.GetUserId(jwt);

                    var hotel = await dbContext.Hotel.SingleOrDefaultAsync(h => h.Id == hotelId);
                                        
                    if (hotel == null || hotel.UserId.ToString() != userId)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("You do not have permission to add images for this hotel.");
                        return;
                    }
                }
            }

            await _requestDelegate(context);
        }
    }
}

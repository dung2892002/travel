using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using Travel.Core.Entities;
using Travel.Core.Interfaces;
using Travel.Infrastructure.Data;

namespace Travel.Api.Middleware
{
    public class RoomOwnershipMiddleware(RequestDelegate requestDelegate)
    {
        private readonly RequestDelegate _requestDelegate = requestDelegate;

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/v1/Rooms"))
            {
                if (context.Request.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase) || context.Request.Path.ToString().Contains("edit"))
                {
                    Console.WriteLine("Put");
                    if (!Guid.TryParse(context.Request.Query["roomId"], out var roomId))
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync("Invalid roomId");
                        return;
                    }

                    var jwt = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                    using (var scope = context.RequestServices.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<TravelDbContext>();
                        var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();

                        var userId = jwtService.GetUserId(jwt);
                        var room = await dbContext.Room.Include(r => r.Hotel).SingleOrDefaultAsync(r => r.Id == roomId);

                        Console.WriteLine($"usser login id: {userId}");
                        Console.WriteLine($"room Id: {roomId}");
                        if (room == null || room.Hotel.UserId.ToString() != userId)
                        {                            
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            await context.Response.WriteAsync("You do not have permission to add images for this hotel.");
                            return;
                        }
                    }
                }

                if (context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Post");
                    context.Request.EnableBuffering();
                    var jwt = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                    using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
                    {
                        var body = await reader.ReadToEndAsync();
                        context.Request.Body.Position = 0;

                        var room = JsonConvert.DeserializeObject<Room>(body);
                        if (room == null)
                        {
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync("Invalid HotelId in request body");
                            return;
                        }
                        var hotelId = room.HotelId;

                        using (var scope = context.RequestServices.CreateScope())
                        {
                            var dbContext = scope.ServiceProvider.GetRequiredService<TravelDbContext>();
                            var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();

                            var userId = jwtService.GetUserId(jwt);
                            Console.WriteLine($"usser login id: {userId}");
                            var hotel = await dbContext.Hotel.SingleOrDefaultAsync(h => h.Id == hotelId);
                            Console.WriteLine($"user Hotel Id: {hotel.UserId}");

                            if (hotel == null || hotel.UserId.ToString() != userId)
                            {
                                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                                await context.Response.WriteAsync("You do not have permission to add images for this hotel.");
                                return;
                            }
                        }
                    }
                }
            }

            await _requestDelegate(context);
        }
    }
}

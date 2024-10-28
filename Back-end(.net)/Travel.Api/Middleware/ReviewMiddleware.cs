using Microsoft.EntityFrameworkCore;
using Travel.Core.Interfaces;
using Travel.Infrastructure.Data;

namespace Travel.Api.Middleware
{
    public class ReviewMiddleware(RequestDelegate requestDelegate)
    {
        private readonly RequestDelegate _requestDelegate = requestDelegate;

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/v1/Reviews/delete"))
            {
                if (!int.TryParse(context.Request.Query["id"], out var reviewId))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Invalid reviewId.");
                    return;
                }
                var jwt = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                using (var scope = context.RequestServices.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<TravelDbContext>();
                    var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();

                    var userId = jwtService.GetUserId(jwt);

                    var review = await dbContext.Review.SingleOrDefaultAsync(r => r.Id == reviewId);

                    if (review == null || review.UserId.ToString() != userId)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("You do not have permission to add images for this hotel.");
                        return;
                    }
                }
            }
        }
    }
}

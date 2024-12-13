using MediQueue.Domain.Entities.Responses;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected ActionResult HandleError(Exception ex, string? customMessage = null)
        {
            var message = ex.InnerException != null
                ? $"{ex.Message}. Inner exception: {ex.InnerException.Message}"
                : ex.Message;

            return Ok(CreateErrorResponse(customMessage ?? message));
        }

        // Helper method for consistent success response
        protected ReturnResponse CreateSuccessResponse(string message)
        {
            return new ReturnResponse
            {
                Code = 0,
                Success = true,
                Message = message
            };
        }

        // Helper method for consistent error response
        protected ReturnResponse CreateErrorResponse(string message)
        {
            return new ReturnResponse
            {
                Code = -1,
                Success = false,
                Message = message
            };
        }
    }
}

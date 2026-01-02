using Microsoft.AspNetCore.Mvc;

namespace OfferService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Health check endpoint for container orchestration
        /// </summary>
        /// <returns>Health status of the service</returns>
        [HttpGet]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok(new
            {
                status = "Healthy",
                timestamp = DateTime.UtcNow,
                service = "offer-service",
                version = "1.0.0"
            });
        }
    }
}
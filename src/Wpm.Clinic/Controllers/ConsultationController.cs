using Microsoft.AspNetCore.Mvc;
using Wpm.Management.Api.DataAccess;

namespace Wpm.Clinic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsultationController : ControllerBase
    {
        private readonly ManagementDbContext _dbContext;
        private readonly ILogger<ConsultationController> _logger;

        public ConsultationController(ManagementDbContext dbContext, ILogger<ConsultationController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpPost("/start")]
        public async Task<IActionResult> Start() 
        { 
            return Ok();
        }
    }
}
public record StartConsultationCommand(int PatientId)
{

}

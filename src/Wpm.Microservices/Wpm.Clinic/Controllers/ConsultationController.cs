using Microsoft.AspNetCore.Mvc;
using Wpm.Clinic.Application;
using Wpm.Clinic.DataAccess;

namespace Wpm.Clinic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultationController : ControllerBase
    {
        private readonly ClinicDbContext _dbContext;
        private readonly ClinicApplicationService _applicationService;
        private readonly ILogger<ConsultationController> _logger;

        public ConsultationController(
        ClinicDbContext dbContext,
        ILogger<ConsultationController> logger,
        ClinicApplicationService applicationService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _applicationService = applicationService;
        }

        [HttpPost("/start")]
        public async Task<IActionResult> Start(StartConsultationCommand command)
        {
            var result = await _applicationService.Handle(command);
            return Ok(result);
        }
    }
}
public record StartConsultationCommand(int PatientId);


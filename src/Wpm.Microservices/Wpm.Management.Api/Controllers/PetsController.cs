using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Wpm.Management.Api.DataAccess;

namespace Wpm.Management.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly ManagementDbContext _dbContext;
        private readonly ILogger<PetsController> _logger;
        public PetsController(ManagementDbContext dbContext,
                                ILogger<PetsController> logger)
        { 
            _dbContext = dbContext;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var all = await _dbContext.Pets.Include(p => p.Breed).ToListAsync();
            return all != null ? Ok(all) : NotFound();
        }
        [HttpGet("{id}", Name = nameof(GetPetById))]
        public async Task<IActionResult> GetPetById(int id)
        {
            var pet = await _dbContext.Pets.Include(p =>p.Breed)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
            return pet != null ? Ok(pet) : NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Create(NewPet newPet)
        {
            try
            {
                var pet = newPet.ToPet();
                await _dbContext.Pets.AddAsync(pet);
                await _dbContext.SaveChangesAsync();

                return CreatedAtRoute(nameof(GetPetById), new {id = pet.Id }, newPet);
            }
            catch (Exception ex) 
            {
                _logger?.LogError(ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
public record NewPet(string Name, int Age, int BreedId)
{
    public Pet ToPet()
    {
        return new Pet() { Name = Name, Age = Age, BreedId = BreedId };
    }
}

using Microsoft.EntityFrameworkCore;

namespace Wpm.Clinic.DataAccess
{
    public class ClinicDbContext(DbContextOptions<ClinicDbContext> options) : DbContext(options)
    {

    }
}

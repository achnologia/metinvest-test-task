using Microsoft.EntityFrameworkCore;

namespace Metinvest.Infrastructure.EFCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        :base(options)
    { }
}
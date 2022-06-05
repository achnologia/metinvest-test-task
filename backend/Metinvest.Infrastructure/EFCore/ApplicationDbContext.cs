using System.Reflection;
using MediatR;
using Metinvest.Application.Shared;
using Metinvest.Domain.Base;
using Metinvest.Domain.Entities;
using Metinvest.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Metinvest.Infrastructure.EFCore;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator)
        :base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Holiday> Holidays => Set<Holiday>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(builder);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
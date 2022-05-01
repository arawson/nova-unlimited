
using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NovaUnlimited.Core.Entities;

namespace NovaUnlimited.Infrastructure.Data;

/// <summary>
/// The database context for nova-unlimited's sqlite/memory implementation.
/// TODO: Most of this code was copied. Take time to understand it.
/// </summary>
public class AppDbContext : DbContext
{
    // TODO: learn more about MediatR : https://github.com/jbogard/MediatR
    private readonly IMediator? _mediator;

    public AppDbContext(DbContextOptions<AppDbContext> options, 
        IMediator? mediator)
    : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Game> Games => Set<Game>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        int result = await base.SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);
        
        // ignore events if no dispatcher provided
        if (_mediator == null) return result;

        // dispatch events only if the save was successful
        var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();

        foreach (var entity in entitiesWithEvents)
        {
            var events = entity.DomainEvents.ToArray();
            entity.DomainEvents.Clear();
            foreach (var domainEvent in events)
            {
                await _mediator.Publish(domainEvent).ConfigureAwait(false);
            }
        }

        return result;
    }

    public override int SaveChanges()
    {
        return SaveChangesAsync().GetAwaiter().GetResult();
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NovaUnlimited.Core.Entities;
using NovaUnlimited.Infrastructure.Data;

namespace NovaUnlimited.IntegrationTests.Data;

public abstract class BaseEfRepoTestFixture
{
    protected AppDbContext dbContext;

    protected BaseEfRepoTestFixture()
    {
        var options = CreateNewContextOptions();
        var mockMediator = new Mock<IMediator>();

        dbContext = new AppDbContext(options, mockMediator.Object);
    }

    protected static DbContextOptions<AppDbContext> CreateNewContextOptions()
    {
        // Create a fresh service provider -> fresh InMemory DB instance
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();
        
        // create a new options instance, tell context to use inmem db
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseInMemoryDatabase("novaunlimited")
            .UseInternalServiceProvider(serviceProvider);
        
        return builder.Options;
    }

    protected EfRepository<Game> GetRepository()
    {
        return new EfRepository<Game>(dbContext);
    }
}


using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NovaUnlimited.Infrastructure.Data;

namespace NovaUnlimited.Infrastructure;

public static class StartupSetup
{
    public static void AddDbContext(
        this IServiceCollection services,
        string connectionString
    )
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connectionString));
    }
}

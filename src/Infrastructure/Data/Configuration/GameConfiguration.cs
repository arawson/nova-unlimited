using Microsoft.EntityFrameworkCore;
using NovaUnlimited.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NovaUnlimited.Infrastructure.Data.Config;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        // TODO: does non-nullability imply the following?
        // builder.Property(x => x.Map).IsRequired();
    }
}

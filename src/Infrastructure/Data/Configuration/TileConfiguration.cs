
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovaUnlimited.Core.Entities;

namespace NovaUnlimited.Infrastructure.Data.Config;

public class TileConfiguration : IEntityTypeConfiguration<Tile>
{
    public void Configure (EntityTypeBuilder<Tile> builder)
    {
        builder.OwnsOne(t => t.Star);
    }
}

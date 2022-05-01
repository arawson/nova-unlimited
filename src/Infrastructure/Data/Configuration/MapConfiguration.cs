
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovaUnlimited.Core.Entities;

namespace NovaUnlimited.Infrastructure.Data.Config;

public class MapConfiguration : IEntityTypeConfiguration<Map>
{
    public void Configure (EntityTypeBuilder<Map> builder)
    {
        builder.Ignore(m => m.Tiles);

        builder.OwnsMany(m => m._tileCollection);
    }
}

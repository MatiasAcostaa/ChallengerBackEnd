using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Domain.Entities;

namespace WebAPI.Infrastructure.Persistence.Configurations;

public class MovieCharacterConfiguration : IEntityTypeConfiguration<MovieCharacter>
{
    public void Configure(EntityTypeBuilder<MovieCharacter> builder)
    {
        builder.HasKey(p => new {p.CharacterId, p.MovieId});
    }
}
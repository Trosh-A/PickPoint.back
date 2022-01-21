using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickPoint.back.Models.ParcelAutomatModel;

namespace PickPoint.back.EFCore.Configurations;

public class ParcelAutomatConfiguration : IEntityTypeConfiguration<ParcelAutomat>
{
  public void Configure(EntityTypeBuilder<ParcelAutomat> builder)
  {
    builder.HasKey(pa => pa.Id);
    builder.HasIndex(pa => pa.Index).IsUnique();
  }
}
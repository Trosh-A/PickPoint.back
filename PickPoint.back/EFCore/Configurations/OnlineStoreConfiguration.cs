using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickPoint.back.Models.CustomServiceUserModel;

namespace PickPoint.back.EFCore.Configurations;

public class CustomServiceUserConfiguration : IEntityTypeConfiguration<CustomServiceUser>
{
  public void Configure(EntityTypeBuilder<CustomServiceUser> builder)
  {
    //builder.Property(ost => ost.CompactName).HasMaxLength(OnlineStoreConstants.COMPACT_NAME_MAX_LEGTH);
    //builder.Property(ost => ost.FullName).HasMaxLength(OnlineStoreConstants.FULL_NAME_MAX_LEGTH);
  }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickPoint.back.Models.GoodModel;

namespace PickPoint.back.EFCore.Configurations;

public class GoodConfiguration : IEntityTypeConfiguration<Good>
{
  public void Configure(EntityTypeBuilder<Good> builder)
  {
    builder.HasKey(g => g.Id);
    builder.HasOne(g => g.Order).WithMany(o => o.Goods).HasForeignKey(g => g.OrderId);
  }
}

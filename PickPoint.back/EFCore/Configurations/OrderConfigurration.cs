using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PickPoint.back.Models.OrderModel;

namespace PickPoint.back.EFCore.Configurations;

public class OrderConfigurration : IEntityTypeConfiguration<Order>
{
  public void Configure(EntityTypeBuilder<Order> builder)
  {
    builder.HasKey(o => o.Id);
    builder.HasMany(o => o.Goods).WithOne(g => g.Order).HasForeignKey(g => g.OrderId);
  }
}

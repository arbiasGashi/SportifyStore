using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).ValueGeneratedOnAdd();

        builder.Property(o => o.UserName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<int>();

        // Address (Owned Value Object)
        builder.OwnsOne(o => o.ShippingAddress, address =>
        {
            address
            .Property(a => a.FirstName)
            .HasColumnName("FirstName")
            .IsRequired()
            .HasMaxLength(100);

            address
            .Property(a => a.LastName)
            .HasColumnName("LastName")
            .IsRequired()
            .HasMaxLength(100);

            address
            .Property(a => a.Email)
            .HasColumnName("Email")
            .IsRequired()
            .HasMaxLength(100);

            address
            .Property(a => a.AddressLine)
            .HasColumnName("AddressLine")
            .IsRequired()
            .HasMaxLength(300);

            address
            .Property(a => a.Country)
            .HasColumnName("Country")
            .IsRequired()
            .HasMaxLength(100);

            address
            .Property(a => a.State)
            .HasColumnName("State")
            .IsRequired()
            .HasMaxLength(100);

            address
            .Property(a => a.ZipCode)
            .HasColumnName("ZipCode")
            .IsRequired()
            .HasMaxLength(20);
        });

        // Payment (Owned Value Object) - optional (Payment?)
        builder.OwnsOne(o => o.Payment, payment =>
        {
            payment
            .Property(p => p.Method)
            .HasColumnName("PaymentMethod")
            .HasConversion<int>();

            payment
            .Property(p => p.PaymentReference)
            .HasColumnName("PaymentReference")
            .IsRequired()
            .HasMaxLength(200);
        });

        // OrderItems relationship
        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey("OrderId")
            .OnDelete(DeleteBehavior.Cascade);

        // Important for rich domain: map using field access for the Items navigation
        var navigation = builder.Metadata.FindNavigation(nameof(Order.Items));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}

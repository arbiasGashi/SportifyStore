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
        builder.Property(o => o.Id)
            .ValueGeneratedOnAdd();

        builder.Property(o => o.Buyer)
            .IsRequired()
            .HasMaxLength(256);

        builder.ToTable(t =>
            t.HasCheckConstraint(
                "CK_Orders_Buyer_NotEmpty",
                "LEN(LTRIM(RTRIM([Buyer]))) > 0")
            );

        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<int>();

        // Address (Owned Value Object)
        builder.OwnsOne(o => o.ShippingAddress, address =>
        {
            address.Property(a => a.FirstName)
            .HasColumnName("FirstName")
            .IsRequired()
            .HasMaxLength(100);

            address.Property(a => a.LastName)
            .HasColumnName("LastName")
            .IsRequired()
            .HasMaxLength(100);

            address.Property(a => a.Email)
            .HasColumnName("Email")
            .IsRequired()
            .HasMaxLength(100);

            address.Property(a => a.AddressLine)
            .HasColumnName("AddressLine")
            .IsRequired()
            .HasMaxLength(300);

            address.Property(a => a.Country)
            .HasColumnName("Country")
            .IsRequired()
            .HasMaxLength(100);

            address.Property(a => a.State)
            .HasColumnName("State")
            .IsRequired()
            .HasMaxLength(100);

            address.Property(a => a.ZipCode)
            .HasColumnName("ZipCode")
            .IsRequired()
            .HasMaxLength(20);
        });

        builder.Navigation(o => o.ShippingAddress)
            .IsRequired();

        // Payment (Owned Value Object) - optional (Payment?)
        builder.OwnsOne(o => o.Payment, payment =>
        {
            payment.Property(p => p.Method)
            .HasColumnName("PaymentMethod")
            .HasConversion<int>();

            payment.Property(p => p.PaymentReference)
            .HasColumnName("PaymentReference")
            .HasMaxLength(200);
        });

        // OrderItems relationship
        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey("OrderId")
            .OnDelete(DeleteBehavior.Cascade);

        // Always load aggregate members with the root
        builder.Navigation(o => o.Items)
            .AutoInclude();

        // Important for rich domain: map using field access for the Items navigation
        builder.Navigation(o => o.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

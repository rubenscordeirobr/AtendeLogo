namespace AtendeLogo.Persistence.Identity.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(ValidationConstants.NameMaxLength);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(ValidationConstants.EmailMaxLength);

        builder.Property(x => x.FiscalCode)
            .IsRequired()
            .HasMaxLength(ValidationConstants.FiscalCodeMaxLength);

        //builder.Property(x => x.PhoneNumber)
        //    .IsRequired()
        //    .HasMaxLength(ValidationConstants.PhoneNumberMaxLength);

        //builder.Property(x => x.BusinessType)
        //    .IsRequired();

        //builder.Property(x => x.TenantState)
        //    .IsRequired();

        //builder.Property(x => x.TenantStatus)
        //    .IsRequired();

        //builder.Property(x => x.TenantType)
        //    .IsRequired();

        builder.HasOne(x => x.Address)
            .WithOne()
            .HasForeignKey<Tenant>(x => x.Address_Id)
            .IsRequired(false);

        builder.HasOne(x => x.Owner)
            .WithOne()
            .HasForeignKey<Tenant>(x => x.Owner_Id)
            .IsRequired(false);

        builder.HasMany(x => x.Users)
            .WithOne(x => x.Tenant)
            .HasForeignKey(x => x.Tenant_Id)
            .IsRequired();


        builder.HasIndex(x => x.FiscalCode)
            .IsUnique();

        builder.HasIndex(x => x.Email)
            .IsUnique();

    }
}

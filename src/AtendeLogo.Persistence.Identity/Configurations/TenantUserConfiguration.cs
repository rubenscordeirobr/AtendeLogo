namespace AtendeLogo.Persistence.Identity.Configurations;

public class TenantUserConfiguration : IEntityTypeConfiguration<TenantUser>
{
    public void Configure(EntityTypeBuilder<TenantUser> builder)
    {
        builder.HasBaseType<User>();

        builder.Property(x => x.TenantUserRole)
            .IsRequired();

        builder.HasOne(x => x.Tenant)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.Tenant_Id)
            .IsRequired();
    }
}

namespace AtendeLogo.Persistence.Identity.Configurations;

public class AdminUserConfiguration : IEntityTypeConfiguration<AdminUser>
{
    public void Configure(EntityTypeBuilder<AdminUser> builder)
    {
        Guard.NotNull(builder);

        builder.HasBaseType<User>();
    }
}

namespace AtendeLogo.Persistence.Identity.Configurations;

public class SystemUserConfiguration : IEntityTypeConfiguration<SystemUser>
{
    public void Configure(EntityTypeBuilder<SystemUser> builder)
    {
        builder.HasBaseType<User>();
    }
}

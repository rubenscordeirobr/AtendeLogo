namespace AtendeLogo.Persistence.Identity.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(ValidationConstants.NameMaxLength);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(ValidationConstants.EmailMaxLength);
         
        builder.HasIndex(x => x.Email)
            .IsUnique();
 
    }
}

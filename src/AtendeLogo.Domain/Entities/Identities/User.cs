using AtendeLogo.Domain.Exceptions;

namespace AtendeLogo.Domain.Entities.Identities;

public abstract class User : EntityBase, IUser, ISoftDeletableEntity, IAscendingSortable, IEventAggregate
{
    private readonly List<IDomainEvent> _events = new();
    public string Name { get; protected set; }
    public string Email { get; protected set; }
    public string? ProfilePictureUrl { get; protected set; }
    public UserRole Role { get; protected set; }
    public Language Language { get; protected set; }
    public UserState UserState { get; protected set; }
    public UserStatus UserStatus { get; protected set; }
    public VerificationState EmailVerificationState { get; protected set; }
    public VerificationState PhoneNumberVerificationState { get; protected set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public Password Password { get; private set; }
    public List<UserSession> Sessions { get; } = [];
     
    protected User(
        string name,
        string email,
        string? profilePictureUrl,
        Language language,
        UserRole role,
        UserState userState,
        UserStatus userStatus,
        VerificationState emailVerificationState,
        VerificationState phoneNumberVerificationState,
        PhoneNumber phoneNumber,
        Password password)
    {
        Guard.NotNullOrWhiteSpace(name);
        Guard.NotNullOrWhiteSpace(email);
        Guard.NotNull(phoneNumber);

        Name = name;
        Email = email;
        ProfilePictureUrl = profilePictureUrl;
        Language = language;
        UserState = userState;
        UserStatus = userStatus;
        EmailVerificationState = emailVerificationState;
        PhoneNumberVerificationState = phoneNumberVerificationState;
        PhoneNumber = phoneNumber;
        Password = password;
    }

    public void ChangePassword(Password password)
    {
        Guard.NotNull(password);

        if (password.Strength < PasswordStrength.Medium)
            throw new DomainException("Password must be strong");

        Password = password;
        _events.Add(new PasswordChangedEvent(this));
    }

    #region IEntityDeleted, IOrderableEntity, IDomainEventAggregate
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public Guid? DeletedSession_Id { get; private set; }
    public double? SortOrder { get; private set; }

    IReadOnlyList<IDomainEvent> IEventAggregate.DomainEvents
        => _events;

    #endregion

}

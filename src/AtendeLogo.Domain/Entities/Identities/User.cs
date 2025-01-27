using AtendeLogo.Domain.Domain;
using AtendeLogo.Domain.Domain.Interfaces;

namespace AtendeLogo.Domain.Entities.Identities;

public abstract class User : EntityBase, IUser, IEntityDeleted
{
    public string Name { get; protected set; }
    public string Password { get; protected set; }
    public string Email { get; protected set; }
    public string PhoneNumber { get; protected set; }
    public UserState UserState { get; protected set; }
    public UserStatus UserStatus { get; protected set; }
    public PasswordStrength PasswordStrength { get; protected set; }
    public UserSession? DeletedSession { get; protected set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedSessionId { get; set; }

    public User(string name,
        string email,
        string phoneNumber,
        Password password,
        UserState userState,
        UserStatus userStatus)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        UserState = userState;
        UserStatus = userStatus;
        Password = password.Value;
        PasswordStrength = password.Strength;
    }

}

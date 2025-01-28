namespace AtendeLogo.Shared.Interfaces.Identities;

public interface IUser
{
    string Name { get; }
    string Email { get; }
    string PhoneNumber { get; }
}

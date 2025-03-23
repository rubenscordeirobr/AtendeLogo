namespace AtendeLogo.TestCommon.Mocks;

public interface IRoleProvider
{
    UserRole UserRole { get; }
}
public class AnonymousRole : IRoleProvider
{
    public UserRole UserRole => UserRole.Anonymous;
}

public class SystemAdminRole : IRoleProvider
{
    public UserRole UserRole => UserRole.SystemAdmin;
}

public class TenantOwnerRole : IRoleProvider
{
    public UserRole UserRole => UserRole.Owner;
}

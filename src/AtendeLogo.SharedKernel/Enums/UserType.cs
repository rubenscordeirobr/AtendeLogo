namespace AtendeLogo.Shared.Enums;

public enum UserType
{
    [UndefinedValue]
    Undefined = 0,
    Anonymous, 
    SystemUser,    
    TenantUser,
    AdminUser
}
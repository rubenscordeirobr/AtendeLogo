
namespace AtendeLogo.Application.Contracts.Persistence.Identities;

public interface ITenantUserRepository : IUserRepository<TenantUser>
{
    Task<bool> EmailExistsAsync(
        Guid currentTenantUser_Id, 
        string email,
        CancellationToken token);
   
    Task<bool> EmailOrPhoneNumberExits(
        string emailOrPhoneNumber, 
        CancellationToken token);

    Task<bool> PhoneNumberExitsAsync(
        string phoneNumber, 
        CancellationToken token);
    
    Task<bool> PhoneNumberExitsAsync(
        Guid currentTenantUser_Id, 
        string phoneNumber, 
        CancellationToken token);
}

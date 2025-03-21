
namespace AtendeLogo.Application.Contracts.Persistence.Identities;

public interface ITenantUserRepository : IUserRepository<TenantUser>
{
    Task<bool> EmailExistsAsync(
        Guid currentTenantUser_Id, 
        string email,
        CancellationToken cancellationToken = default);
   
    Task<bool> EmailOrPhoneNumberExits(
        string emailOrPhoneNumber, 
        CancellationToken cancellationToken = default);

    Task<bool> PhoneNumberExitsAsync(
        string phoneNumber, 
        CancellationToken cancellationToken = default);
    
    Task<bool> PhoneNumberExitsAsync(
        Guid currentTenantUser_Id, 
        string phoneNumber, 
        CancellationToken cancellationToken = default);
}

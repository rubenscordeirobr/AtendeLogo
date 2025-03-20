namespace AtendeLogo.UseCases.Contracts.Identities;

public interface ITenantUserValidationService : IValidationService
{
    Task<bool> IsEmailUniqueAsync(
       string email,
       CancellationToken token = default);

    Task<bool> IsEmailUniqueAsync(
        Guid currentUser_Id,
        string email,
        CancellationToken token = default);

    Task<bool> IsPhoneNumberUniqueAsync(
        string phoneNumber, 
        CancellationToken token = default);

    Task<bool> IsPhoneNumberUniqueAsync(
        Guid currentUser_Id,
        string number,
        CancellationToken token = default);
}

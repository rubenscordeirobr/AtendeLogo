namespace AtendeLogo.UseCases.Mappers.Identities;

internal static class UserMapper
{
    internal static UserResponse ToResponse(User user)
    {
        Guard.NotNull(user);

        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Culture = user.Culture,
            Role = user.Role,
            UserType = user.UserType,
            EmailVerificationState = user.EmailVerificationState,
            PhoneNumberVerificationState = user.PhoneNumberVerificationState,
            PhoneNumber = user.PhoneNumber
        };
    }
}

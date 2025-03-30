﻿namespace AtendeLogo.UseCases.Identities.Authentications.Commands;

public record TenantUserLoginCommand : CommandRequest<TenantUserLoginResponse>
{
    public required string EmailOrPhoneNumber { get; init; }
    public required string Password { get; init; }
    public required bool KeepSession { get; init; }
}


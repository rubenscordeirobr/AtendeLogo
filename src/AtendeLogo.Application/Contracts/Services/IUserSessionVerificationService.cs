﻿
namespace AtendeLogo.Application.Contracts.Services;

public interface IUserSessionVerificationService
{
    Task<IUserSession> VerifyAsync();
}

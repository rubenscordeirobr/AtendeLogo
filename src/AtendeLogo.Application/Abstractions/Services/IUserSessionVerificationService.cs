﻿
namespace AtendeLogo.Application.Abstractions.Services;

public interface IUserSessionVerificationService: IApplicationService
{
    Task<IUserSession> VerifyAsync();
}


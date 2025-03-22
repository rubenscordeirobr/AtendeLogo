﻿namespace AtendeLogo.Shared.Enums;

public enum UserNotificationType
{
    [UndefinedValue]
    Undefined = 0,
    NewUser = 1,
    PasswordReset,
    AutehnticationFailed
}

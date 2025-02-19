﻿using AtendeLogo.UseCases.Identities.Users.Shared;

namespace AtendeLogo.UseCases.Identities.Users.AdminUsers.Queries;

public sealed record AdminUserResponse : UserResponse
{
    public required AdminUserRole Role { get; init; }
}

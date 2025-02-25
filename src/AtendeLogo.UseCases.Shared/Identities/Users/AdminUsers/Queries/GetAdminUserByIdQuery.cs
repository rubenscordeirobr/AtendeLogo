﻿using AtendeLogo.UseCases.Common;

namespace AtendeLogo.UseCases.Identities.Users.AdminUsers.Queries;

public record GetAdminUserByIdQuery(Guid Id) 
    : GetEntityByIdQuery<AdminUserResponse>(Id);

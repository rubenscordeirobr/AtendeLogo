﻿namespace AtendeLogo.UseCases.Identities.Users.TenantUsers.Commands;

public record DeleteTenantUserCommand(Guid Id) : CommandRequest<OperationResponse>;

public class DeleteTenantUserCommandValidator : CommandValidator<DeleteTenantUserCommand>
{
    public DeleteTenantUserCommandValidator(
        IJsonStringLocalizer<ValidationMessages> localizer)
        : base(localizer)
    {
        Guard.NotNull(localizer);

        RuleFor(x => x.Id)
            .NotEmptyGuid()
            .WithMessage(localizer["TenantUser.IdRequired", "Id is required."]);
    }
}

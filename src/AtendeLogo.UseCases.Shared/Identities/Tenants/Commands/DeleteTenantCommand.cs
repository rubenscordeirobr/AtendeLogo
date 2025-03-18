namespace AtendeLogo.UseCases.Identities.Tenants.Commands;

public record DeleteTenantCommand(Guid Id) : CommandRequest<OperationResponse>;
public class DeleteTenantCommandValidator : CommandValidator<DeleteTenantCommand>
{
    public DeleteTenantCommandValidator(
        IJsonStringLocalizer<ValidationMessages> localizer)
        : base(localizer)
    {
        RuleFor(x => x.Id)
            .NotEmptyGuid()
            .WithMessage(localizer["Tenant.IdRequired", "Id is required."]);
    }
}

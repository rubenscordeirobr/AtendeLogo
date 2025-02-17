using AtendeLogo.Application.Exceptions;
using AtendeLogo.Common.Extensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.Application.Mediatores;

internal class CommandValidatorExecutor<TResponse>
    where TResponse : IResponse
{
    private readonly IServiceProvider _serviceProvider;
    private ICommandRequest<TResponse> _command;
    private readonly ILogger<RequestMediator> _logger;

    internal CommandValidatorExecutor(
        IServiceProvider serviceProvider,
        ILogger<RequestMediator> logger,
        ICommandRequest<TResponse> request )
    {
        _serviceProvider = serviceProvider;
        _command = request;
        _logger = logger;
    }

    public async Task<Result<bool>> ValidateAsync(CancellationToken cancellationToken)
    {
        var commandValidatorType = typeof(IValidator<>).MakeGenericType(_command.GetType());

        var commandValidator = _serviceProvider.GetRequiredService(commandValidatorType) as IValidator;

        if(commandValidator is null)
        {
            var commandTypeName = _command.GetType().GetQualifiedName();
            var mensagem = "Command Validator not found for command {CommandTypeName}";
            _logger.LogError(mensagem, commandTypeName);

            throw new CommandValidatorNotFoundException(mensagem, commandTypeName);
        }
             
        var validationContext = CreateValidationContext();
        var result = await commandValidator.ValidateAsync(validationContext, cancellationToken);

        if (result.IsValid)
        {
            return Result.Success(true);
        }

        var commandType = _command.GetType();
        var validationFailure = result.Errors.FirstOrDefault() 
            ?? throw new InvalidOperationException($"Validation Error not found for command {_command.GetType()}");

        var errorCode = ErrorCodeFactory.CreateInvalidCodeFor(
            commandType, 
            validationFailure.PropertyName);

        var error = new ValidationError(
            Code: errorCode,
            Message: validationFailure.ErrorMessage);

        return Result.Failure<bool>(error);
    }

    private IValidationContext CreateValidationContext()
    {
        var type = typeof(ValidationContext<>).MakeGenericType(_command.GetType());
        var instance = Activator.CreateInstance(type, _command);

        if (instance is null)
            throw new InvalidOperationException($"Validation Context not created for command {_command.GetType()}");

        return (IValidationContext)instance;
    }
}

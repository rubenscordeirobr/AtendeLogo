using AtendeLogo.Application.Abstractions.Mediators;

namespace AtendeLogo.TestCommon.Extensions;

public static class CommandMediatorExtensions
{
    public static async Task<ResultTest<TCommand>> TestRunAsync<TCommand>(
        this ICommandMediator mediator,
        TCommand command )
    {
        var result = await mediator.RunAsync(command! as dynamic);
        return new ResultTest<TCommand>(result.ConvertTo<IResponse>());
    }
     
}

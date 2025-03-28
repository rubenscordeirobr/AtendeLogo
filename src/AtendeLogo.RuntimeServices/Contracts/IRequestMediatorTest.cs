using AtendeLogo.Application.Contracts.Handlers;

namespace AtendeLogo.RuntimeServices.Contracts;

internal interface IRequestMediatorTest
{
    IRequestHandler<TResponse> GetRequestHandler<TResponse>(
         IRequest<TResponse> request)
         where TResponse : IResponse;
}

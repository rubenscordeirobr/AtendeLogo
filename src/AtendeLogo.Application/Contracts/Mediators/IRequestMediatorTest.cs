using AtendeLogo.Application.Contracts.Handlers;

namespace AtendeLogo.Application.Contracts.Mediators;

internal interface IRequestMediatorTest : IRequestMediator
{
    IRequestHandler<TResponse> GetRequestHandler<TResponse>(
         IRequest<TResponse> request)
         where TResponse : IResponse;
}


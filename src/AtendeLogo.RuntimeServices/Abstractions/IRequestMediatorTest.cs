using AtendeLogo.Application.Abstractions.Handlers;

namespace AtendeLogo.RuntimeServices.Abstractions;

internal interface IRequestMediatorTest
{
    IRequestHandler<TResponse> GetRequestHandler<TResponse>(
         IRequest<TResponse> request)
         where TResponse : IResponse;
}

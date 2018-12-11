using MediatR;

namespace Uni.Api.Infrastructure.Interfaces.CQRS.Commands
{
    public interface ICommand : IRequest
    {
    }

    public interface ICommand<out TResult> : IRequest<TResult>
    {
    }
}

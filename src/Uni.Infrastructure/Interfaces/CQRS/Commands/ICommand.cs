using MediatR;

namespace Uni.Infrastructure.Interfaces.CQRS.Commands
{
    public interface ICommand : IRequest
    {
    }

    public interface ICommand<out TResult> : IRequest<TResult>
    {
    }
}

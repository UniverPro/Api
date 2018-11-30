using MediatR;

namespace Uni.Infrastructure.Interfaces.CQRS.Queries
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}

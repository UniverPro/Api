using MediatR;

namespace Uni.Api.Infrastructure.Interfaces.CQRS.Queries
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}

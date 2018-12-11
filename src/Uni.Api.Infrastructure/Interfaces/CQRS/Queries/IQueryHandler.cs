using MediatR;

namespace Uni.Api.Infrastructure.Interfaces.CQRS.Queries
{
    public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
    }
}

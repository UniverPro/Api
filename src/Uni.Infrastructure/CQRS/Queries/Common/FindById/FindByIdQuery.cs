using JetBrains.Annotations;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Common.FindById
{
    [UsedImplicitly]
    public class FindByIdQuery<T> : IQuery<T> where T : class, ITableObject
    {
        public FindByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
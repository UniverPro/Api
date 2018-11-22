using System.Collections.Generic;
using JetBrains.Annotations;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Common.FindAll
{
    [UsedImplicitly]
    public class FindAllQuery<T> : IQuery<List<T>> where T : class, ITableObject
    {
    }
}

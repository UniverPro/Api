using System;
using JetBrains.Annotations;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Common.Create
{
    [UsedImplicitly]
    public class CreateCommand<T> : ICommand<T> where T : class, ITableObject
    {
        public CreateCommand([NotNull] T entity)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
        }

        public T Entity { get; }
    }
}

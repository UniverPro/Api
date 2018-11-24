using System;
using JetBrains.Annotations;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Common.Delete
{
    [UsedImplicitly]
    public class DeleteCommand<T> : ICommand where T : class, ITableObject
    {
        public DeleteCommand([NotNull] T entity)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
        }

        public T Entity { get; }
    }
}

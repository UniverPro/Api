using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Uni.DataAccess.Data;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Common.Delete
{
    [UsedImplicitly]
    public class DeleteCommandHandler<T> : ICommandHandler<DeleteCommand<T>> where T : class, ITableObject
    {
        private readonly UniDbContext _context;

        public DeleteCommandHandler([NotNull] UniDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Unit> Handle(DeleteCommand<T> request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var requestEntity = request.Entity;
                    var tracked = _context.Set<T>().Remove(requestEntity);

                    await _context.SaveChangesAsync(cancellationToken);
                    transaction.Commit();

                    return Unit.Value;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}

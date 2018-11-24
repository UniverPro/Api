using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Uni.DataAccess.Data;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Common.Create
{
    [UsedImplicitly]
    public class CreateCommandHandler<T> : ICommandHandler<CreateCommand<T>, T> where T : class, ITableObject
    {
        private readonly UniDbContext _context;

        public CreateCommandHandler([NotNull] UniDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<T> Handle(CreateCommand<T> request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var requestEntity = request.Entity;
                    var tracked = await _context.Set<T>().AddAsync(requestEntity, cancellationToken);

                    await _context.SaveChangesAsync(cancellationToken);
                    transaction.Commit();

                    return tracked.Entity;
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
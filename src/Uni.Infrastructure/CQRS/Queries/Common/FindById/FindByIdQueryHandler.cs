using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Data;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Common.FindById
{
    [UsedImplicitly]
    public class FindByIdQueryHandler<T> : IQueryHandler<FindByIdQuery<T>, T> where T : class, ITableObject
    {
        private readonly UniDbContext _context;

        public FindByIdQueryHandler([NotNull] UniDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<T> Handle(FindByIdQuery<T> request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var entity = await _context.Set<T>().SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                    return entity;
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

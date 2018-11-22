using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Data;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Common.FindAll
{
    [UsedImplicitly]
    public class FindAllQueryHandler<T> : IQueryHandler<FindAllQuery<T>, List<T>> where T : class, ITableObject
    {
        private readonly UniDbContext _context;

        public FindAllQueryHandler(UniDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<T>> Handle(FindAllQuery<T> request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var entity = await _context.Set<T>().AsNoTracking().ToListAsync(cancellationToken);

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
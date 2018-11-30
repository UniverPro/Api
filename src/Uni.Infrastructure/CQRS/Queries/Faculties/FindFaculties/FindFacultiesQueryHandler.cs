using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Contexts;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Faculties.FindFaculties
{
    [UsedImplicitly]
    public class FindFacultiesQueryHandler : IQueryHandler<FindFacultiesQuery, IEnumerable<Faculty>>
    {
        private readonly UniDbContext _dbContext;

        public FindFacultiesQueryHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Faculty>> Handle(
            FindFacultiesQuery query,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction =
                await _dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken))
            {
                try
                {
                    var faculties = await _dbContext
                        .Faculties
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    return faculties;
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

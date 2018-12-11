using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LinqBuilder.Core;
using Microsoft.EntityFrameworkCore;
using Uni.Api.Core.Exceptions;
using Uni.Api.DataAccess.Contexts;
using Uni.Api.DataAccess.Models;
using Uni.Api.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Api.Infrastructure.CQRS.Queries.Faculties.FindFaculties
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

            var specification = query.ToSpecification();

            using (var transaction =
                await _dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken))
            {
                try
                {
                    if (query.UniversityId != null)
                    {
                        var universityId = query.UniversityId.Value;

                        var universityExists = await _dbContext
                            .Universities
                            .AsNoTracking()
                            .AnyAsync(x => x.Id == universityId, cancellationToken);

                        if (!universityExists)
                        {
                            throw new NotFoundException("university", universityId);
                        }
                    }

                    var faculties = await _dbContext
                        .Faculties
                        .AsNoTracking()
                        .ExeSpec(specification)
                        .ToListAsync(cancellationToken);

                    transaction.Commit();
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

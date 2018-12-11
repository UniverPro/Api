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

namespace Uni.Api.Infrastructure.CQRS.Queries.Subjects.FindSubjects
{
    [UsedImplicitly]
    public class FindSubjectsQueryHandler : IQueryHandler<FindSubjectsQuery, IEnumerable<Subject>>
    {
        private readonly UniDbContext _dbContext;

        public FindSubjectsQueryHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Subject>> Handle(
            FindSubjectsQuery query,
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
                    if (query.GroupId != null)
                    {
                        var groupId = query.GroupId.Value;

                        var groupExists = await _dbContext
                            .Groups
                            .AsNoTracking()
                            .AnyAsync(x => x.Id == groupId, cancellationToken);

                        if (!groupExists)
                        {
                            throw new NotFoundException("group", groupId);
                        }
                    }

                    var subjects = await _dbContext
                        .Subjects
                        .AsNoTracking()
                        .ExeSpec(specification)
                        .ToListAsync(cancellationToken);

                    transaction.Commit();
                    return subjects;
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

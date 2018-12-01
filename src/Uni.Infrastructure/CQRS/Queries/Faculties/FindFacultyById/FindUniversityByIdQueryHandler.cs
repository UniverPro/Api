using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Contexts;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Faculties.FindFacultyById
{
    [UsedImplicitly]
    public class FindFacultyByIdQueryHandler : IQueryHandler<FindFacultyByIdQuery, Faculty>
    {
        private readonly UniDbContext _dbContext;

        public FindFacultyByIdQueryHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Faculty> Handle(
            FindFacultyByIdQuery query,
            CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction =
                await _dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken))
            {
                try
                {
                    var faculty = await _dbContext
                        .Faculties
                        .AsNoTracking()
                        .SingleOrDefaultAsync(x => x.Id == query.FacultyId, cancellationToken);
                    
                    return faculty;
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

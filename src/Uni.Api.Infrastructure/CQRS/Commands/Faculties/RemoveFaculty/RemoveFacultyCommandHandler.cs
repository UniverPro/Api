using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni.Api.Core.Exceptions;
using Uni.Api.DataAccess.Contexts;
using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Api.Infrastructure.CQRS.Commands.Faculties.RemoveFaculty
{
    [UsedImplicitly]
    public class RemoveFacultyCommandHandler : ICommandHandler<RemoveFacultyCommand>
    {
        private readonly UniDbContext _dbContext;

        public RemoveFacultyCommandHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(
            RemoveFacultyCommand command,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var faculty =
                        await _dbContext.Faculties.SingleOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

                    if (faculty == null)
                    {
                        throw new NotFoundException(nameof(faculty), command.Id);
                    }

                    _dbContext.Remove(faculty);

                    await _dbContext.SaveChangesAsync(cancellationToken);

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

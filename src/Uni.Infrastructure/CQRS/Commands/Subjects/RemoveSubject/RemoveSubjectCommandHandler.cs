using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni.Core.Exceptions;
using Uni.DataAccess.Contexts;
using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Subjects.RemoveSubject
{
    [UsedImplicitly]
    public class RemoveSubjectCommandHandler : ICommandHandler<RemoveSubjectCommand>
    {
        private readonly UniDbContext _dbContext;

        public RemoveSubjectCommandHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(
            RemoveSubjectCommand command,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var subject =
                        await _dbContext.Subjects.SingleOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

                    if (subject == null)
                    {
                        throw new NotFoundException(nameof(subject), command.Id);
                    }

                    _dbContext.Remove(subject);

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

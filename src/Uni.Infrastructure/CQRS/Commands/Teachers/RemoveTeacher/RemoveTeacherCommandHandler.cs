using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Contexts;
using Uni.Infrastructure.Exceptions;
using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Teachers.RemoveTeacher
{
    [UsedImplicitly]
    public class RemoveTeacherCommandHandler : ICommandHandler<RemoveTeacherCommand>
    {
        private readonly UniDbContext _dbContext;

        public RemoveTeacherCommandHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(
            RemoveTeacherCommand command,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var teacher =
                        await _dbContext.Teachers.SingleOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

                    if (teacher == null)
                    {
                        throw new NotFoundException(nameof(teacher), command.Id);
                    }

                    _dbContext.Remove(teacher);

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

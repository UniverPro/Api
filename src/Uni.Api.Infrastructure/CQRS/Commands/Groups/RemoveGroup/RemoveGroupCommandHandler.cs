using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni.Api.Core.Exceptions;
using Uni.Api.DataAccess.Contexts;
using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Api.Infrastructure.CQRS.Commands.Groups.RemoveGroup
{
    [UsedImplicitly]
    public class RemoveGroupCommandHandler : ICommandHandler<RemoveGroupCommand>
    {
        private readonly UniDbContext _dbContext;

        public RemoveGroupCommandHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(
            RemoveGroupCommand command,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var group =
                        await _dbContext.Groups.SingleOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

                    if (group == null)
                    {
                        throw new NotFoundException(nameof(group), command.Id);
                    }

                    _dbContext.Remove(group);

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

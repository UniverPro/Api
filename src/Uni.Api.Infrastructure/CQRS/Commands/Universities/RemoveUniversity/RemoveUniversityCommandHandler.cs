﻿using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni.Api.Core.Exceptions;
using Uni.Api.DataAccess.Contexts;
using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Api.Infrastructure.CQRS.Commands.Universities.RemoveUniversity
{
    [UsedImplicitly]
    public class RemoveUniversityCommandHandler : ICommandHandler<RemoveUniversityCommand>
    {
        private readonly UniDbContext _dbContext;

        public RemoveUniversityCommandHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(
            RemoveUniversityCommand command,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var university = await _dbContext.Universities.SingleOrDefaultAsync(
                        x => x.Id == command.Id,
                        cancellationToken
                    );

                    if (university == null)
                    {
                        throw new NotFoundException(nameof(university), command.Id);
                    }

                    _dbContext.Remove(university);

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

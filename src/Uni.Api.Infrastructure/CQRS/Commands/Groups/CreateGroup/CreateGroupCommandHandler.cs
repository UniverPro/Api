﻿using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Uni.Api.DataAccess.Contexts;
using Uni.Api.DataAccess.Models;
using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Api.Infrastructure.CQRS.Commands.Groups.CreateGroup
{
    [UsedImplicitly]
    public class CreateGroupCommandHandler : ICommandHandler<CreateGroupCommand, int>
    {
        private readonly UniDbContext _dbContext;

        public CreateGroupCommandHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<int> Handle(
            CreateGroupCommand command,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    // TODO: check FacultyId exists
                    var group = new Group
                    {
                        Name = command.Name,
                        FacultyId = command.FacultyId,
                        CourseNumber = command.CourseNumber
                    };

                    _dbContext.Groups.Add(group);

                    await _dbContext.SaveChangesAsync(cancellationToken);

                    transaction.Commit();

                    return group.Id;
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

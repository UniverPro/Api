﻿using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni.Core.Exceptions;
using Uni.DataAccess.Contexts;
using Uni.Infrastructure.Interfaces.CQRS.Commands;
using Uni.Infrastructure.Interfaces.Services;

namespace Uni.Infrastructure.CQRS.Commands.Students.UpdateStudent
{
    [UsedImplicitly]
    public class UpdateStudentCommandHandler : ICommandHandler<UpdateStudentCommand>
    {
        private readonly IBlobStorageUploader _blobStorageUploader;
        private readonly UniDbContext _dbContext;

        public UpdateStudentCommandHandler([NotNull] UniDbContext dbContext, IBlobStorageUploader blobStorageUploader)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _blobStorageUploader = blobStorageUploader;
        }

        public async Task<Unit> Handle(
            UpdateStudentCommand command,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var student = await _dbContext.Students.SingleOrDefaultAsync(
                        x => x.Id == command.Id,
                        cancellationToken
                    );

                    if (student == null)
                    {
                        throw new NotFoundException(nameof(student), command.Id);
                    }

                    student.FirstName = command.FirstName;
                    student.LastName = command.LastName;
                    student.MiddleName = command.MiddleName;
                    student.GroupId = command.GroupId;

                    await _dbContext.SaveChangesAsync(cancellationToken);

                    if (command.Avatar != null)
                    {
                        var avatarUri = await _blobStorageUploader.UploadImageAsync(
                            command.Avatar,
                            cancellationToken
                        );

                        student.AvatarPath = avatarUri;

                        await _dbContext.SaveChangesAsync(cancellationToken);
                    }

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

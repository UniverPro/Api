﻿using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Uni.Api.DataAccess.Contexts;
using Uni.Api.DataAccess.Models;
using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;
using Uni.Api.Infrastructure.Interfaces.Services;

namespace Uni.Api.Infrastructure.CQRS.Commands.Teachers.CreateTeacher
{
    [UsedImplicitly]
    public class CreateTeacherCommandHandler : ICommandHandler<CreateTeacherCommand, int>
    {
        private readonly IBlobStorageUploader _blobStorageUploader;
        private readonly UniDbContext _dbContext;

        public CreateTeacherCommandHandler(
            [NotNull] UniDbContext dbContext,
            [NotNull] IBlobStorageUploader blobStorageUploader
            )
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _blobStorageUploader = blobStorageUploader ?? throw new ArgumentNullException(nameof(blobStorageUploader));
        }

        public async Task<int> Handle(
            CreateTeacherCommand command,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    // TODO: Check if FacultyId exists
                    var teacher = new Teacher
                    {
                        FirstName = command.FirstName,
                        LastName = command.LastName,
                        MiddleName = command.MiddleName,
                        Email = command.Email,
                        FacultyId = command.FacultyId
                    };

                    _dbContext.Teachers.Add(teacher);

                    await _dbContext.SaveChangesAsync(cancellationToken);

                    if (command.Avatar != null)
                    {
                        var avatarUri = await _blobStorageUploader.UploadImageAsync(
                            command.Avatar,
                            cancellationToken
                        );

                        teacher.AvatarPath = avatarUri;

                        await _dbContext.SaveChangesAsync(cancellationToken);
                    }

                    transaction.Commit();

                    return teacher.Id;
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

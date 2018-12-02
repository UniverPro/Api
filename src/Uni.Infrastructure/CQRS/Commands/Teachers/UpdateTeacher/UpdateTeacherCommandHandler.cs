using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni.Core.Exceptions;
using Uni.DataAccess.Contexts;
using Uni.Infrastructure.Interfaces.CQRS.Commands;
using Uni.Infrastructure.Interfaces.Services;

namespace Uni.Infrastructure.CQRS.Commands.Teachers.UpdateTeacher
{
    [UsedImplicitly]
    public class UpdateTeacherCommandHandler : ICommandHandler<UpdateTeacherCommand>
    {
        private readonly IBlobStorageUploader _blobStorageUploader;
        private readonly UniDbContext _dbContext;

        public UpdateTeacherCommandHandler(
            [NotNull] UniDbContext dbContext,
            [NotNull] IBlobStorageUploader blobStorageUploader
            )
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _blobStorageUploader = blobStorageUploader ?? throw new ArgumentNullException(nameof(blobStorageUploader));
        }

        public async Task<Unit> Handle(
            UpdateTeacherCommand command,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var teacher = await _dbContext.Teachers.SingleOrDefaultAsync(
                        x => x.Id == command.Id,
                        cancellationToken
                    );

                    if (teacher == null)
                    {
                        throw new NotFoundException(nameof(teacher), command.Id);
                    }

                    teacher.FirstName = command.FirstName;
                    teacher.LastName = command.LastName;
                    teacher.MiddleName = command.MiddleName;
                    teacher.FacultyId = command.FacultyId;

                    await _dbContext.SaveChangesAsync(cancellationToken);

                    if (command.Avatar != null)
                    {
                        var avatarUri = await _blobStorageUploader.UploadImageToStorageAsync(
                            command.Avatar,
                            cancellationToken
                        );

                        teacher.AvatarPath = avatarUri;

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

using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Uni.DataAccess.Contexts;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Commands;
using Uni.Infrastructure.Interfaces.Services;

namespace Uni.Infrastructure.CQRS.Commands.Students.CreateStudent
{
    [UsedImplicitly]
    public class CreateStudentCommandHandler : ICommandHandler<CreateStudentCommand, int>
    {
        private readonly IBlobStorageUploader _blobStorageUploader;
        private readonly UniDbContext _dbContext;

        public CreateStudentCommandHandler(
            [NotNull] UniDbContext dbContext,
            [NotNull] IBlobStorageUploader blobStorageUploader
            )
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _blobStorageUploader = blobStorageUploader ?? throw new ArgumentNullException(nameof(blobStorageUploader));
        }

        public async Task<int> Handle(
            CreateStudentCommand command,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var student = new Student
                    {
                        FirstName = command.FirstName,
                        LastName = command.LastName,
                        MiddleName = command.MiddleName,
                        GroupId = command.GroupId
                    };

                    _dbContext.Students.Add(student);

                    await _dbContext.SaveChangesAsync(cancellationToken);

                    if (command.Avatar != null)
                    {
                        var avatarUri = await _blobStorageUploader.UploadImageToStorageAsync(
                            command.Avatar,
                            cancellationToken
                        );

                        student.AvatarPath = avatarUri.ToString();

                        await _dbContext.SaveChangesAsync(cancellationToken);
                    }

                    transaction.Commit();

                    return student.Id;
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

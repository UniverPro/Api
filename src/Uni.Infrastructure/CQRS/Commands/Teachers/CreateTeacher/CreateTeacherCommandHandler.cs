using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Uni.DataAccess.Contexts;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Teachers.CreateTeacher
{
    [UsedImplicitly]
    public class CreateTeacherCommandHandler : ICommandHandler<CreateTeacherCommand, int>
    {
        private readonly UniDbContext _dbContext;

        public CreateTeacherCommandHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
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
                    var teacher = new Teacher
                    {
                        FirstName = command.FirstName,
                        LastName = command.LastName,
                        MiddleName = command.MiddleName,
                        AvatarPath = command.AvatarPath,
                        FacultyId = command.FacultyId
                    };

                    _dbContext.Teachers.Add(teacher);

                    await _dbContext.SaveChangesAsync(cancellationToken);

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

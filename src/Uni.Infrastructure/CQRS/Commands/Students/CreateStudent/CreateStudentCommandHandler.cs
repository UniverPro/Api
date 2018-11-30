using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Uni.DataAccess.Contexts;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Students.CreateStudent
{
    [UsedImplicitly]
    public class CreateStudentCommandHandler : ICommandHandler<CreateStudentCommand, int>
    {
        private readonly UniDbContext _dbContext;

        public CreateStudentCommandHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
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
                        AvatarPath = command.AvatarPath,
                        GroupId = command.GroupId
                    };

                    _dbContext.Students.Add(student);

                    await _dbContext.SaveChangesAsync(cancellationToken);

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

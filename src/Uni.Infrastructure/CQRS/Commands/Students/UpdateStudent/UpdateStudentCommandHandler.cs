using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Contexts;
using Uni.Infrastructure.Exceptions;
using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Students.UpdateStudent
{
    [UsedImplicitly]
    public class UpdateStudentCommandHandler : ICommandHandler<UpdateStudentCommand>
    {
        private readonly UniDbContext _dbContext;

        public UpdateStudentCommandHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
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
                        throw new NotFoundException();
                    }

                    student.FirstName = command.FirstName;
                    student.LastName = command.LastName;
                    student.MiddleName = command.MiddleName;
                    student.AvatarPath = command.AvatarPath;
                    student.GroupId = command.GroupId;

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

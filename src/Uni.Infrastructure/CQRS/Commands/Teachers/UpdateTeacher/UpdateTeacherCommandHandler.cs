using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Contexts;
using Uni.Infrastructure.Exceptions;
using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Teachers.UpdateTeacher
{
    [UsedImplicitly]
    public class UpdateTeacherCommandHandler : ICommandHandler<UpdateTeacherCommand>
    {
        private readonly UniDbContext _dbContext;

        public UpdateTeacherCommandHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
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
                    var teacher = await _dbContext.Teachers.SingleOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

                    if (teacher == null)
                    {
                        throw new NotFoundException();
                    }
                    
                    teacher.FirstName = command.FirstName;
                    teacher.LastName = command.LastName;
                    teacher.MiddleName = command.MiddleName;
                    teacher.AvatarPath = command.AvatarPath;
                    teacher.FacultyId = command.FacultyId;

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

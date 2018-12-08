using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Uni.DataAccess.Contexts;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Subjects.CreateSubject
{
    [UsedImplicitly]
    public class CreateSubjectCommandHandler : ICommandHandler<CreateSubjectCommand, int>
    {
        private readonly UniDbContext _dbContext;

        public CreateSubjectCommandHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<int> Handle(
            CreateSubjectCommand command,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    // TODO: Check if GroupId exists
                    var subject = new Subject
                    {
                        Name = command.Name,
                        GroupId = command.GroupId
                    };

                    _dbContext.Subjects.Add(subject);

                    await _dbContext.SaveChangesAsync(cancellationToken);

                    transaction.Commit();

                    return subject.Id;
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

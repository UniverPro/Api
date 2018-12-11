using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Uni.Api.DataAccess.Contexts;
using Uni.Api.DataAccess.Models;
using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Api.Infrastructure.CQRS.Commands.Universities.CreateUniversity
{
    [UsedImplicitly]
    public class CreateUniversityCommandHandler : ICommandHandler<CreateUniversityCommand, int>
    {
        private readonly UniDbContext _dbContext;

        public CreateUniversityCommandHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<int> Handle(
            CreateUniversityCommand command,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var university = new University
                    {
                        Name = command.Name,
                        ShortName = command.ShortName,
                        Description = command.Description
                    };

                    _dbContext.Universities.Add(university);

                    await _dbContext.SaveChangesAsync(cancellationToken);

                    transaction.Commit();

                    return university.Id;
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

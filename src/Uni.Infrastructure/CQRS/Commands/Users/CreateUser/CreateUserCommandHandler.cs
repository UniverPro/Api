using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Uni.DataAccess.Contexts;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Commands;
using Uni.Infrastructure.Interfaces.Services;

namespace Uni.Infrastructure.CQRS.Commands.Users.CreateUser
{
    [UsedImplicitly]
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, int>
    {
        private readonly UniDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCommandHandler([NotNull] UniDbContext dbContext, [NotNull] IPasswordHasher passwordHasher)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<int> Handle(
            CreateUserCommand command,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    // TODO check wether PersonId exists
                    var user = new User
                    {
                        Login = command.Login,
                        Password = _passwordHasher.HashPassword(command.Password),
                        PersonId = command.PersonId
                    };

                    _dbContext.Users.Add(user);

                    await _dbContext.SaveChangesAsync(cancellationToken);

                    transaction.Commit();

                    return user.Id;
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

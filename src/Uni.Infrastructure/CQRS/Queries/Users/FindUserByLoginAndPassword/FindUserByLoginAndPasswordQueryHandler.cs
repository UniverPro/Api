using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Uni.Core.Exceptions;
using Uni.DataAccess;
using Uni.DataAccess.Contexts;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;
using Uni.Infrastructure.Interfaces.Services;

namespace Uni.Infrastructure.CQRS.Queries.Users.FindUserByLoginAndPassword
{
    [UsedImplicitly]
    public class FindUserByLoginAndPasswordQueryHandler : IQueryHandler<FindUserByLoginAndPasswordQuery, User>
    {
        private readonly UniDbContext _dbContext;
        private readonly IPasswordValidator _passwordValidator;

        public FindUserByLoginAndPasswordQueryHandler(
            [NotNull] UniDbContext dbContext,
            [NotNull] IPasswordValidator passwordValidator
            )
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _passwordValidator = passwordValidator ?? throw new ArgumentNullException(nameof(passwordValidator));
        }

        public async Task<User> Handle(
            FindUserByLoginAndPasswordQuery query,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var transaction =
                await _dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken))
            {
                try
                {
                    var user = await _dbContext
                        .Users
                        .IncludeDefault()
                        .AsNoTracking()
                        .SingleOrDefaultAsync(
                            x => EF.Functions.Like(x.Login, query.Login),
                            cancellationToken
                        );

                    if (user != null && !_passwordValidator.Verify(user.PasswordHash, query.Password))
                    {
                        throw new HttpStatusCodeException(422, "Wrong password.", "The user found, but the password was wrong.");
                    }

                    transaction.Commit();
                    return user;
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

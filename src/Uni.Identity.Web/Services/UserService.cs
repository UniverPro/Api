using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Contexts;
using Uni.DataAccess.Models;

namespace Uni.Identity.Web.Services
{
    public class UserService : IUserService
    {
        private readonly UniDbContext _context;

        public UserService([NotNull] UniDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Person> FindUserByIdAsync(int userId)
        {
            return await _context.Set<Person>().SingleAsync(x => x.Id == userId);
        }

        public async Task<Person> FindAsync(string login, string password)
        {
            return await _context.Set<Person>().FirstAsync();
        }
    }
}
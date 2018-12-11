using System.Security.Cryptography;

namespace Uni.Api.Infrastructure.Services
{
    public class PasswordHasherOptions
    {
        private static readonly RandomNumberGenerator DefaultRandomNumberGenerator = RandomNumberGenerator.Create();

        public int IterationsCount { get; set; } = 10000;

        internal RandomNumberGenerator RandomNumberGenerator { get; set; } = DefaultRandomNumberGenerator;
    }
}

namespace Uni.Api.Infrastructure.Interfaces.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
    }
}

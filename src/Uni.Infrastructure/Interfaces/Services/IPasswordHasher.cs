namespace Uni.Infrastructure.Interfaces.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
    }
}
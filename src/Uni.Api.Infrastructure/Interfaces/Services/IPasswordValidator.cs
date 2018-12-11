namespace Uni.Api.Infrastructure.Interfaces.Services
{
    public interface IPasswordValidator
    {
        bool Verify(string hashedPassword, string providedPassword);
    }
}

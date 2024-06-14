namespace SharedKernel.Contracts.Infrastructure
{
    public interface IEncryptionService
    {
        string HashPassword(string pass);
    }
}

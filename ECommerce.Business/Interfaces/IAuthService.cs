namespace ECommerce.Business.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(
            string name,
            string email,
            string password);

        Task<string?> LoginAsync(
            string email,
            string password);
    }
}
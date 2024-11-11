namespace RAuth.Application.Repository
{
    public interface IRAuthRepository
    {
        Task<string> VerifyClient(string ClientId, string ClientSecret);
    }
}

using RAuth.Application.Repository;
using RAuth.Infrastructure.Data;

namespace RAuth.Infrastructure.Repository
{
    public class RAuthRepository(ApplicationDbContext db) : IRAuthRepository
    {
        private readonly ApplicationDbContext _db = db;
        public Task<string> VerifyClient(string ClientId, string ClientSecret)
        {
            throw new NotImplementedException();
        }
    }
}

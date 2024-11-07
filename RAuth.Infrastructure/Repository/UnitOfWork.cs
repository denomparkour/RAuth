using RAuth.Application.Repository;

namespace RAuth.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IAuthRepository authRepository)
        {
            AuthRepository = authRepository;
        }
        public IAuthRepository AuthRepository { get; private set; }
    }
}

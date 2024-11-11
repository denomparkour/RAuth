using RAuth.Application.Repository;

namespace RAuth.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IAuthRepository authRepository, IUserRepository userRepository, IRAuthRepository rauthRepository)
        {
            AuthRepository = authRepository;
            UserRepository = userRepository;
            RAuthRepository = rauthRepository;

        }
        public IAuthRepository AuthRepository { get; private set; }

        public IUserRepository UserRepository { get; private set; }

        public IRAuthRepository RAuthRepository { get; private set; }
    }
}

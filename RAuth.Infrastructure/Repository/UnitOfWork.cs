using RAuth.Application.Repository;

namespace RAuth.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IAuthRepository authRepository, IUserRepository userRepository)
        {
            AuthRepository = authRepository;
            UserRepository = userRepository;

        }
        public IAuthRepository AuthRepository { get; private set; }

        public IUserRepository UserRepository { get; private set; }
    }
}

using RAuth.Application.Repository;
using RAuth.Infrastructure.Data;

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

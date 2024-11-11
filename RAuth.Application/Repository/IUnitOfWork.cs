namespace RAuth.Application.Repository
{
    public interface IUnitOfWork
    {
        IAuthRepository AuthRepository { get; }
        IUserRepository UserRepository { get; }
    }
}

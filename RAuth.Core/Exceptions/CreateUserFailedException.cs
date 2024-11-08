namespace RAuth.Core.Exceptions
{
    public class CreateUserFailedException : Exception
    {
        public CreateUserFailedException()
        {

        }
        public CreateUserFailedException(string message) : base(message)
        {

        }
    }
}

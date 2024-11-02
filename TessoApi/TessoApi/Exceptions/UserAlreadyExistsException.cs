namespace TessoApi.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() : base() { }
        public UserAlreadyExistsException(string message) : base(message) { }
    }
}

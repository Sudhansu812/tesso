namespace TessoApi.Exceptions
{
    public class ProjectAlreadyExistException : Exception
    {
        public ProjectAlreadyExistException() : base() { }
        public ProjectAlreadyExistException(string? message) : base(message)
        {
        }
    }
}

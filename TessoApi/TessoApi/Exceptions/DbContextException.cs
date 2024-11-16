namespace TessoApi.Exceptions
{
    public class DbContextException : Exception
    {
        public DbContextException() : base() { }
        public DbContextException(string message) : base(message) { }
    }
}

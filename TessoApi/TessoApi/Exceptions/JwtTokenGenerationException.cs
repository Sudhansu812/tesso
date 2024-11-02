namespace TessoApi.Exceptions
{
    public class JwtTokenGenerationException : Exception
    {
        public JwtTokenGenerationException() : base() { }
        public JwtTokenGenerationException(string message) : base(message) { }
    }
}

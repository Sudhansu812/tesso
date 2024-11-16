namespace TessoApi.Exceptions
{
    public class JwtTokenExtractionException : Exception
    {
        public JwtTokenExtractionException() : base()
        {
        }

        public JwtTokenExtractionException(string? message) : base(message)
        {
        }
    }
}

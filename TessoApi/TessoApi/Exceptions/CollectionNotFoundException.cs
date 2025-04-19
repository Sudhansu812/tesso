namespace TessoApi.Exceptions
{
    public class CollectionNotFoundException : Exception
    {
        public CollectionNotFoundException() : base()
        {
        }

        public CollectionNotFoundException(string? message) : base(message)
        {
        }
    }
}

using System.Net;

namespace TessoApi.Models.Http
{
    public class CustomHttpResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T? Data { get; set; }
        public string? Error { get; set; }
    }
}

namespace TessoApi.DTOs.Auth
{
    public class AuthenticationResponse
    {
        public bool IsAuthenticated { get; set; }
        public string StatusMessage { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}

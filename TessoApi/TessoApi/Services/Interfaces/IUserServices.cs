namespace TessoApi.Services.Interfaces
{
    public interface IUserServices
    {
        public Task<Guid> GetUserId();
        public Task<string> GetUserFullName(Guid userId);
    }
}

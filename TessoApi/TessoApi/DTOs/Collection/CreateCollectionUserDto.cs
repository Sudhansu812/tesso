using TessoApi.Models.Identity;

namespace TessoApi.DTOs.Collection
{
    public class CreateCollectionUserDto
    {
        public Guid CollectionId { get; set; }
        public Guid UserId { get; set; }
        public bool IsOwner { get; set; } = false;
        public bool IsReadOnly { get; set; } = true;
        public bool HasDeleteAccess { get; set; } = false;
    }
}

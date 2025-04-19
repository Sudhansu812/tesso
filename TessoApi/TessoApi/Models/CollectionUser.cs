using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TessoApi.Models.Identity;

namespace TessoApi.Models
{
    [Table(nameof(CollectionUser), Schema = "dbo")]
    public class CollectionUser
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CollectionId { get; set; }
        public Collection Collection { get; set; }
        public Guid UserId { get; set; }
        public bool IsOwner { get; set; } = false;
        public bool IsReadOnly { get; set; } = true;
        public bool HasDeleteAccess { get; set; } = false;
    }
}

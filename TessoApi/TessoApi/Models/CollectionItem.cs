using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TessoApi.Models
{
    [Table(nameof(CollectionItem), Schema = "dbo")]
    public class CollectionItem
    {
        [Key, Required]
        public Guid Id { get; set; }
        [Required]
        public Guid CollectionId { get; set; }
        public Collection Collection { get; set; }
        [Required]
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace TessoApi.DTOs.Collection
{
    public class UpdateCollectionDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace TessoApi.DTOs.Collection
{
    public class CreateCollectionDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}

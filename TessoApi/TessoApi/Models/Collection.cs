using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TessoApi.Models
{
    [Table(nameof(Collection), Schema = "dbo")]
    public class Collection
    {
        [Key, Required, Column("Id")]
        public Guid Id { get; set; }

        [Required, Column("Name")]
        public string Name { get; set; }

        [Required, Column("Description")]
        public string Description { get; set; }

        public List<Project> Projects { get; } = [];

        public List<CollectionUser> CollectionUsers { get; } = [];
    }
}

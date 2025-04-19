using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TessoApi.Models.Identity;

namespace TessoApi.Models
{
    [Table(nameof(Project), Schema = "dbo")]
    public class Project
    {
        [Key, Column("Id")]
        public Guid Id { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("CreatorId")]
        public Guid CreatorId { get; set; }
        public List<ProjectOwner> ProjectOwners { get; set; }
        public List<Collection> Collections { get; } = [];
    }
}

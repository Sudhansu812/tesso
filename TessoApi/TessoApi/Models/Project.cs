using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TessoApi.Models.Identity;

namespace TessoApi.Models
{
    [Table("tblProjects", Schema = "dbo")]
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

        [Column("OwnerId")]
        public Guid OwnerId { get; set; }
    }
}

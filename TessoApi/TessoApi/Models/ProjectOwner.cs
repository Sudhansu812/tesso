using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TessoApi.Models
{
    [Table(nameof(ProjectOwner), Schema = "dbo")]
    public class ProjectOwner
    {
        [Key, Column("Id")]
        public Guid Id { get; set; }

        [Column("ProjectId")]
        public Guid ProjectId { get;set; }

        [Column("UserId")]
        public Guid UserId { get; set; }
        public Project Project { get; set; }
    }
}

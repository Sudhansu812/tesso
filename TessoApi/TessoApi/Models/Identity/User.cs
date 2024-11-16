using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TessoApi.Models.Identity
{
    [Table("tblUser", Schema = "dbo")]
    public class User : IdentityUser<Guid>
    {
        [Required, Column("FirstName")]
        public string FirstName { get; set; }

        [Column("LastName")]
        public string LastName { get; set; }
    }
}

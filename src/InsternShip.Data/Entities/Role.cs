using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsternShip.Data.Entities
{
    /*[Table("Roles")]*/
    public class Roles : IdentityRole<Guid>
    {
        [Column("Description")]
        public string? Description { get; set; }
    }

    //[Table("RoleClaims")]
    public class RoleClaims : IdentityRoleClaim<Guid> { }

    /*[Table("UserRoles")]*/
    public class UserRoles : IdentityUserRole<Guid> { }
}

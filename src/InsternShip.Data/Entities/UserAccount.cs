using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsternShip.Data.Entities
{
    /*[Table("User")]*/
    public class UserAccount : IdentityUser<Guid>
    {
        [Required]
        public Guid InfoId { get; set; }

        [Column("RegistrationDate")]
        public DateTime? RegistrationDate { get; set; }

        [Column("ActiveStatus")]
        public bool? ActiveStatus { get; set; }

        [Column("IsDeleted")]
        public bool IsDeleted { get; set; }

        [ForeignKey("InfoId")]
        public virtual UserInfo? UserInfo { get; set; }
    }

    [Table("UserInfo")]
    public class UserInfo
    {
        [Key]
        [Column("InfoId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid InfoId { get; set; }
        public string? Avatar { get; set; }

        [Column("FirstName")]
        public string? FirstName { get; set; }

        [Column("LastName")]
        public string? LastName { get; set; }

        [Column("Gender")]
        public string? Gender { get; set; }

        [Column("DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }
        [Column("PhoneNumber")]
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
    }

}
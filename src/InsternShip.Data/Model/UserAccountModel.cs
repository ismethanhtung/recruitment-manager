using System.ComponentModel.DataAnnotations;

namespace InsternShip.Data.Model
{
    public class UserModel
    {
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public bool? ActiveStatus { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class UserInfoModel
    {
        public int InfoId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        //public bool IsDeleted { get; set; }
    }
    public class UserInfoUpdateModel
    {
        public string? Avatar { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; }
    }
    public class CandidateUserInfoUpdateModel
    {
        public virtual UserInfoUpdateModel? UserInfoUpdate { get; set; }
        public virtual CandidateUpdateModel? CandidateUpdate { get; set; }
    }
    public class RecruiterUserInfoUpdateModel
    {
        public virtual UserInfoUpdateModel? UserInfoUpdate { get; set; }
        public virtual RecruiterUpdateModel? RecruiterUpdate { get; set; }
    }
    public class InterviewerUserInfoUpdateModel
    {
        public virtual UserInfoUpdateModel? UserInfoUpdate { get; set; }
        public virtual InterviewerUpdateModel? InterviewerUpdate { get; set; }
    }
    public class CreateUserModel
    {
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string ConfirmPassword { get; set; } = null!;
        public string? Avatar { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Location { get; set; }
        public string Role { get; set; }

    }
    public class SignUpModel
    {
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string ConfirmPassword { get; set; } = null!;
        public string? Avatar { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Location { get; set; }

    }
    public class SignInModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }

    public class SignInTokenModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        public string? UserId { get; set; }
        public string[]? Roles { get; set; }
    }
    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
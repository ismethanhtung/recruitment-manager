namespace InsternShip.Data.ViewModels
{
    public class UserAccountViewModel
    {
        public Guid UserId { get; set; }
        //public Guid RoleId { get; set; }
        public Guid InfoId { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public bool? ActiveStatus { get; set; }
        public virtual UserInfoViewModel UserInfo { get; set; }
        public string? Role { get; set; }
    }
    public class UserInfoViewModel
    {
        public Guid InfoId { get; set; }
        public string? Avatar { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; }
    }

    public class AllInfoUser
    {
        public string Role { get; set; }
        public object InfoCurrentUser { get; set; }
        public object InfoUser { get; set; }
    }

    public class AllInfoUserUpdate
    {
        public object InfoCurrentUser { get; set; }
        public object InfoUser { get; set; }
    }

    public class ResultUpdateInfo
    {
        public object ResultUpdate { get; set; }
        public bool ResultUserUpdate { get; set; }
    }

    public class SignInViewModel
    {
        public string? Token { get; set; }
        public string? Roles { get; set; }
    }
    public class UserAccountListViewModel
    {
        public int TotalCount { get; set; }
        public ICollection<UserAccountViewModel>? AccountList { get; set; }
    }
}

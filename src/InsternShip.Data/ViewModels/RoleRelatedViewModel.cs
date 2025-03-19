namespace InsternShip.Data.ViewModels
{
    public class RoleViewModel
    {
        public Guid? RoleId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class RoleClaimsViewModel
    {
        public int? RoleClaimsId { get; set; }
        public Guid? RoleId { get; set; }
        public string? ClaimType { get; set; }
        public string? ClaimValue { get; set; }
    }

    public class AllowedRolesViewModel
    {
        public virtual RoleViewModel? Roles { get; set; }
        public virtual ICollection<RoleClaimsViewModel>? RoleClaims { get; set; }
    }

    public class UserRolesViewModel<T>
    {
        // -> đến user claim bất kỳ
        public T? CurrentUserClaim { get; set; }
        public string Role { get; set; }
    }
}

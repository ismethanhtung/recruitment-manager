namespace InsternShip.Data.Model
{
    public class RoleModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class RoleClaimsModel
    {
        public Guid? RoleId { get; set; }
        public string? ClaimType { get; set; }
    }

    public class UserRolesModel
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }

    public class UserRolesUpdateModel
    {
        public IEnumerable<Guid> RoleId { get; set; }
    }
}

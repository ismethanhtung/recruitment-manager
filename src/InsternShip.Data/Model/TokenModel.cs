namespace InsternShip.Data.Model
{
    public class TokenModel
    {
        public Guid UserId { get; set; }
        public Guid UserClaimId { get; set; }
        public object? CurrentUser { get; set; }
    }

    public class DecodeModel
    {
        public string? UserId { get; set; }
        public string? UserClaimId { get; set; }
    }
}

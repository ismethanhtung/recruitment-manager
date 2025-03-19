namespace InsternShip.Data.Model
{
    public class RecruiterModel
    {
        public Guid UserId { get; set; }
        public string? Description { get; set; }
        public string? UrlContact { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class CreateRecruiterModel
    {
        public Guid UserId { get; set; }
        public string? Description { get; set; }
        public string? UrlContact { get; set; }
    }
    public class RecruiterUpdateModel
    {
        //public Guid RecruiterId { get; set; }
        //public Guid UserId { get; set; }
        public string? Description { get; set; }
        public string? UrlContact { get; set; }
        //public bool IsDeleted { get; set; }
    }
}

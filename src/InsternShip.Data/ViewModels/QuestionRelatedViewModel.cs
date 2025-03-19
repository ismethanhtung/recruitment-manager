namespace InsternShip.Data.ViewModels
{
    public class QuestionBankViewModel
    {
        //public Guid QuestionBankId { get; set; }
        public Guid TestId { get; set; }
        public Guid QuestionId { get; set; }
    }
    public class QuestionViewModel
    {
        public Guid QuestionId { get; set; }
        public string? Detail { get; set; }
        public int? MaxScore { get; set; }
        public string? Note { get; set; }
        public string? Tag { get; set; }
        public int? Level { get; set; }
    }
    public class TestViewModel
    {
        public Guid TestId { get; set; }
        public int? TotalScore { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }

    public class TestQuestionListViewModel
    {
        public Guid TestId { get; set; }
        public int? TotalScore { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public virtual ICollection<QuestionViewModel> Questions { get; set; } = new List<QuestionViewModel>();
    }
    public class TestListViewModel
    {
        //public int QuestionId { get; set; }
        //new List<TestViewModel>();
        public int? TotalCount { get; set; }
        public virtual ICollection<TestViewModel>? TestList { get; set; }
    }
    public class QuestionListViewModel
    {
        //public int QuestionId { get; set; }
        //new List<TestViewModel>();
        public int? TotalCount { get; set; }
        public virtual ICollection<QuestionViewModel>? QuestionList { get; set; }
    }
}

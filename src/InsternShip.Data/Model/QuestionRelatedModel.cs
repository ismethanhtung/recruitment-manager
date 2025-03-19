using InsternShip.Data.Entities;
using InsternShip.Data.ViewModels;

namespace InsternShip.Data.Model
{
    public class QuestionModel
    {
        public Guid QuestionId { get; set; }
        public string? Detail { get; set; }
        public int? MaxScore { get; set; }
        public string? Tag { get; set; }
        public int? Level { get; set; }
        public string? Note { get; set; }
        public QuestionModel() { }
    }
    public class QuestionBankModel
    {
        public Guid QuestionBankId { get; set; }
        public Guid TestId { get; set; }
        public Guid QuestionId { get; set; }
    }
    public class TestModel
    {
        public Guid TestId { get; set; }
        public int? TotalScore { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsDeleted { get; set; }
        public TestModel() { }

    }
    //Create Model
    public class CreateQuestionModel
    {
        //public int QuestionDetailId { get; set; }
        public string? Detail { get; set; }
        public int? MaxScore { get; set; }
        public string? Note { get; set; }
        public string? Tag { get; set; }
        public int? Level { get; set; }
    }
    public class CreateTestModel
    {
        //public int QuestionDetailId { get; set; }
        public int? TotalScore { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
    public class CreateQuestionBankModel
    {
        //public int QuestionDetailId { get; set; }
        public Guid TestId { get; set; }
        public Guid QuestionId { get; set; }
    }
    public class QuestionBankDetailModel
    {
        //public int QuestionId { get; set; }
        public Test? Test { get; set; }
        public virtual ICollection<QuestionViewModel> Questions { get; set; } = new List<QuestionViewModel>();
    }

    public class QuestionUpdateModel
    {
        public string? Detail { get; set; }
        public int? MaxScore { get; set; }
        public string? Note { get; set; }
        public string? Tag { get; set; }
        public int? Level { get; set; }
    }
    public class TestUpdateModel
    {
        public int? TotalScore { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
